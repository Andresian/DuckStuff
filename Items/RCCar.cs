using System;
using System.Collections.Generic;

namespace DuckGame
{
	// Token: 0x0200035C RID: 860
	[BaggedProperty("canSpawn", false)]
	public class RCCar : Holdable, IPlatform
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001983 RID: 6531
		// (set) Token: 0x06001984 RID: 6532
		public bool receivingSignal
		{
			get
			{
				return this._receivingSignal;
			}
			set
			{
				if (this._receivingSignal != value && !this.destroyed)
				{
					if (value)
					{
						SFX.Play("rcConnect", 0.5f, 0f, 0f, false);
					}
					else
					{
						SFX.Play("rcDisconnect", 0.5f, 0f, 0f, false);
					}
				}
				this._receivingSignal = value;
			}
		}

		// Token: 0x06001985 RID: 6533
		public RCCar(float xpos, float ypos) : base(xpos, ypos)
		{
			this._sprite = new SpriteMap("rcBody", 32, 32, false);
			SpriteMap sprite = this._sprite;
			string name = "idle";
			float speed = 1f;
			bool looping = true;
			int[] frames = new int[1];
			sprite.AddAnimation(name, speed, looping, frames);
			this._sprite.AddAnimation("beep", 0.2f, true, new int[]
			{
				0,
				1
			});
			this.graphic = this._sprite;
			this.center = new Vec2(16f, 24f);
			this.collisionOffset = new Vec2(-8f, 0f);
			this.collisionSize = new Vec2(16f, 11f);
			base.depth = -0.5f;
			this._editorName = "RC Car";
			this.thickness = 2f;
			this.weight = 5f;
			this.flammable = 0.3f;
			this._wheel = new Sprite("rcWheel", 0f, 0f);
			this._wheel.center = new Vec2(4f, 4f);
			this.weight = 0.5f;
			this.physicsMaterial = PhysicsMaterial.Metal;
		}

		// Token: 0x06001986 RID: 6534
		public override void Initialize()
		{
		}

		// Token: 0x06001987 RID: 6535
		public override void Terminate()
		{
			this._idle.Kill();
			this._idle.lerpVolume = 0f;
		}

		// Token: 0x06001988 RID: 6536
		protected override bool OnDestroy(DestroyType type = null)
		{
			if (!base.isServerForObject)
			{
				return false;
			}
			ATRCShrapnel atrcshrapnel = new ATRCShrapnel();
			atrcshrapnel.MakeNetEffect(this.position, false);
			List<Bullet> list = new List<Bullet>();
			for (int i = 0; i < 20; i++)
			{
				float num = (float)i * 18f - 5f + Rando.Float(10f);
				atrcshrapnel = new ATRCShrapnel();
				atrcshrapnel.range = 55f + Rando.Float(14f);
				Bullet bullet = new Bullet(base.x + (float)(Math.Cos((double)Maths.DegToRad(num)) * 6.0), base.y - (float)(Math.Sin((double)Maths.DegToRad(num)) * 6.0), atrcshrapnel, num, null, false, -1f, false, true);
				bullet.firedFrom = this;
				list.Add(bullet);
				Level.Add(bullet);
			}
			if (Network.isActive)
			{
				Send.Message(new NMFireGun(null, list, 0, false, 4, false), NetMessagePriority.ReliableOrdered, null);
				list.Clear();
			}
			Level.Remove(this);
			FollowCam followCam = Level.current.camera as FollowCam;
			if (followCam != null)
			{
				followCam.Remove(this);
			}
			if (Recorder.currentRecording != null)
			{
				Recorder.currentRecording.LogBonus();
			}
			return true;
		}

		// Token: 0x06001989 RID: 6537
		public override bool Hit(Bullet bullet, Vec2 hitPos)
		{
			if (bullet.isLocal && this.owner == null)
			{
				Thing.Fondle(this, DuckNetwork.localConnection);
			}
			if (bullet.isLocal)
			{
				this.Destroy(new DTShot(bullet));
			}
			return false;
		}

		// Token: 0x0600198A RID: 6538
		public override void Update()
		{
			base.Update();
			this._sprite.currentAnimation = (this._receivingSignal ? "beep" : "idle");
			this._idle.lerpVolume = Math.Min(this._idleSpeed * 10f, 0.7f);
			if (base._destroyed)
			{
				this._idle.lerpVolume = 0f;
				this._idle.lerpSpeed = 1f;
			}
			this._idle.pitch = 0.5f + this._idleSpeed * 0.5f;
			if (this.moveLeft)
			{
				if (this.hSpeed > -this._maxSpeed)
				{
					this.hSpeed -= 0.4f;
				}
				else
				{
					this.hSpeed = -this._maxSpeed;
				}
				this.offDir = -1;
				this._idleSpeed += 0.03f;
				this._inc++;
			}
			if (this.moveRight)
			{
				if (this.hSpeed < this._maxSpeed)
				{
					this.hSpeed += 0.4f;
				}
				else
				{
					this.hSpeed = this._maxSpeed;
				}
				this.offDir = 1;
				this._idleSpeed += 0.03f;
				this._inc++;
			}
			if (this._idleSpeed > 0.1f)
			{
				this._inc = 0;
				Level.Add(SmallSmoke.New(base.x - (float)(this.offDir * 10), base.y));
			}
			if (!this.moveLeft && !this.moveRight)
			{
				this._idleSpeed -= 0.03f;
			}
			if (this._idleSpeed > 1f)
			{
				this._idleSpeed = 1f;
			}
			if (this._idleSpeed < 0f)
			{
				this._idleSpeed = 0f;
			}
			if (this.jump && base.grounded)
			{
				this.vSpeed -= 4.8f;
			}
			this._tilt = MathHelper.Lerp(this._tilt, -this.hSpeed, 0.4f);
			this._waveMult = MathHelper.Lerp(this._waveMult, -this.hSpeed, 0.1f);
			base.angleDegrees = this._tilt * 2f + this._wave.value * (this._waveMult * (this._maxSpeed - Math.Abs(this.hSpeed)));
			if (base.isServerForObject && base.y > Level.current.lowestPoint + 100f && !this.destroyed)
			{
				this.Destroy(new DTFall());
			}
		}

		// Token: 0x0600198B RID: 6539
		public override void Draw()
		{
			if (this.owner == null)
			{
				this._sprite.flipH = ((float)this.offDir < 0f);
			}
			base.Draw();
			Graphics.Draw(this._wheel, base.x - 7f, base.y + 9f);
			Graphics.Draw(this._wheel, base.x + 7f, base.y + 9f);
		}

		// Token: 0x0400170D RID: 5901
		public StateBinding _signalBinding = new StateBinding("receivingSignal", -1, false, false);

		// Token: 0x0400170E RID: 5902
		public StateBinding _idleSpeedBinding = new CompressedFloatBinding("_idleSpeed", 1f, 4, false, false);

		// Token: 0x0400170F RID: 5903
		private SpriteMap _sprite;

		// Token: 0x04001710 RID: 5904
		private float _tilt;

		// Token: 0x04001711 RID: 5905
		private float _maxSpeed = 6f;

		// Token: 0x04001712 RID: 5906
		private SinWave _wave = new SinWave(0.1f, 0f);

		// Token: 0x04001713 RID: 5907
		private float _waveMult;

		// Token: 0x04001714 RID: 5908
		private Sprite _wheel;

		// Token: 0x04001715 RID: 5909
		public bool moveLeft;

		// Token: 0x04001716 RID: 5910
		public bool moveRight;

		// Token: 0x04001717 RID: 5911
		public bool jump;

		// Token: 0x04001718 RID: 5912
		private bool _receivingSignal;

		// Token: 0x04001719 RID: 5913
		private int _inc;

		// Token: 0x0400171A RID: 5914
		public float _idleSpeed;

		// Token: 0x0400171B RID: 5915
		private ConstantSound _idle = new ConstantSound("rcDrive", 0f, 0f, null);
	}
}
