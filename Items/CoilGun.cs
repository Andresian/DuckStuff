using System;

namespace DuckGame
{
	// Token: 0x020004E4 RID: 1252
	[BaggedProperty("canSpawn", true)]
	[BaggedProperty("isOnlineCapable", true)]
	[EditorGroup("guns|laser")]
	public class CoilGun : Gun
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x0600222E RID: 8750
		// (set) Token: 0x0600222F RID: 8751
		private byte netAnimationIndex
		{
			get
			{
				if (this._chargeAnim == null)
				{
					return 0;
				}
				return (byte)this._chargeAnim.animationIndex;
			}
			set
			{
				if (this._chargeAnim != null && this._chargeAnim.animationIndex != (int)value)
				{
					this._chargeAnim.animationIndex = (int)value;
				}
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002230 RID: 8752
		// (set) Token: 0x06002231 RID: 8753
		public byte spriteFrame
		{
			get
			{
				if (this._chargeAnim == null)
				{
					return 0;
				}
				return (byte)this._chargeAnim._frame;
			}
			set
			{
				if (this._chargeAnim != null)
				{
					this._chargeAnim._frame = (int)value;
				}
			}
		}

		// Token: 0x06002232 RID: 8754
		public CoilGun(float xval, float yval) : base(xval, yval)
		{
			this.ammo = 30;
			this._type = "gun";
			this.center = new Vec2(16f, 16f);
			this.collisionOffset = new Vec2(-11f, -8f);
			this.collisionSize = new Vec2(22f, 12f);
			this._barrelOffsetTL = new Vec2(25f, 13f);
			this._fireSound = "";
			this._fullAuto = false;
			this._fireWait = 1f;
			this._kickForce = 1f;
			this._holdOffset = new Vec2(3f, 1f);
			this._editorName = "Death Laser";
			this._chargeAnim = new SpriteMap("coilGun", 32, 32, false);
			SpriteMap chargeAnim = this._chargeAnim;
			string name = "idle";
			float speed = 1f;
			bool looping = true;
			int[] frames = new int[1];
			chargeAnim.AddAnimation(name, speed, looping, frames);
			this._chargeAnim.AddAnimation("charge", 0.38f, false, new int[]
			{
				1,
				2,
				3,
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				4,
				5,
				6,
				7
			});
			this._chargeAnim.AddAnimation("charged", 1f, true, new int[]
			{
				8,
				9,
				10,
				11
			});
			this._chargeAnim.AddAnimation("uncharge", 1.2f, false, new int[]
			{
				7,
				6,
				5,
				4,
				7,
				6,
				5,
				4,
				3,
				2,
				1,
				0,
				3,
				2,
				1,
				0
			});
			this._chargeAnim.AddAnimation("drain", 2f, false, new int[]
			{
				7,
				6,
				5,
				4,
				7,
				6,
				5,
				4,
				3,
				2,
				1,
				0,
				3,
				2,
				1,
				0
			});
			this._chargeAnim.SetAnimation("idle");
			this.graphic = this._chargeAnim;
		}

		// Token: 0x06002233 RID: 8755
		public override void Initialize()
		{
			this._chargeSound = SFX.Get("laserCharge", 0f, 0f, 0f, false);
			this._chargeSoundShort = SFX.Get("laserChargeShort", 0f, 0f, 0f, false);
			this._unchargeSound = SFX.Get("laserUncharge", 0f, 0f, 0f, false);
			this._unchargeSoundShort = SFX.Get("laserUnchargeShort", 0f, 0f, 0f, false);
		}

		// Token: 0x06002234 RID: 8756
		public override void Update()
		{
			base.Update();
			if (this._charge > 0f)
			{
				this._charge -= 0.1f;
			}
			else
			{
				this._charge = 0f;
			}
			if (this._chargeAnim.currentAnimation == "uncharge" && this._chargeAnim.finished)
			{
				this._chargeAnim.SetAnimation("idle");
			}
			if ((Network.isActive && this.doBlast && !this._lastDoBlast) || (this._chargeAnim.currentAnimation == "charge" && this._chargeAnim.finished && base.isServerForObject))
			{
				this._chargeAnim.SetAnimation("charged");
			}
			if (this.doBlast && base.isServerForObject)
			{
				this._framesSinceBlast++;
				if (this._framesSinceBlast > 10)
				{
					this._framesSinceBlast = 0;
					this.doBlast = false;
				}
			}
			if (this._chargeAnim.currentAnimation == "drain" && this._chargeAnim.finished)
			{
				this._chargeAnim.SetAnimation("idle");
			}
			this._lastDoBlast = this.doBlast;
		}

		// Token: 0x06002235 RID: 8757
		public override void Draw()
		{
			base.Draw();
		}

		// Token: 0x06002236 RID: 8758
		public override void OnPressAction()
		{
			if (this._chargeAnim.currentAnimation == "idle")
			{
				this._chargeSound.Volume = 1f;
				this._chargeSound.Play();
				this._chargeAnim.SetAnimation("charge");
				this._unchargeSound.Stop();
				this._unchargeSound.Volume = 0f;
				this._unchargeSoundShort.Stop();
				this._unchargeSoundShort.Volume = 0f;
				return;
			}
			if (this._chargeAnim.currentAnimation == "uncharge")
			{
				if (this._chargeAnim.frame > 18)
				{
					this._chargeSound.Volume = 1f;
					this._chargeSound.Play();
				}
				else
				{
					this._chargeSoundShort.Volume = 1f;
					this._chargeSoundShort.Play();
				}
				int index = this._chargeAnim.frame;
				this._chargeAnim.SetAnimation("charge");
				this._chargeAnim.frame = 22 - index;
				this._unchargeSound.Stop();
				this._unchargeSound.Volume = 0f;
				this._unchargeSoundShort.Stop();
				this._unchargeSoundShort.Volume = 0f;
			}
		}

		// Token: 0x06002237 RID: 8759
		public override void OnHoldAction()
		{
		}

		// Token: 0x06002238 RID: 8760
		public override void OnReleaseAction()
		{
			if (this._chargeAnim.currentAnimation == "charge")
			{
				if (this._chargeAnim.frame > 20)
				{
					this._unchargeSound.Volume = 1f;
					this._unchargeSound.Play();
				}
				else
				{
					this._unchargeSoundShort.Volume = 1f;
					this._unchargeSoundShort.Play();
				}
				int index = this._chargeAnim.frame;
				this._chargeAnim.SetAnimation("uncharge");
				this._chargeAnim.frame = 22 - index;
				this._chargeSound.Stop();
				this._chargeSound.Volume = 0f;
				this._chargeSoundShort.Stop();
				this._chargeSoundShort.Volume = 0f;
			}
			if (this._chargeAnim.currentAnimation == "charged")
			{
				Graphics.FlashScreen();
				this._chargeAnim.SetAnimation("drain");
				SFX.Play("laserBlast", 1f, 0f, 0f, false);
				for (int i = 0; i < 4; i++)
				{
					Level.Add(new ElectricalCharge(base.barrelPosition.x, base.barrelPosition.y, (int)this.offDir, this));
				}
			}
		}

		// Token: 0x04002105 RID: 8453
		public StateBinding _laserStateBinding = new StateFlagBinding(new string[]
		{
			"_charging",
			"_fired",
			"doBlast"
		});

		// Token: 0x04002106 RID: 8454
		public StateBinding _animationIndexBinding = new StateBinding("netAnimationIndex", 4, false, false);

		// Token: 0x04002107 RID: 8455
		public StateBinding _frameBinding = new StateBinding("spriteFrame", -1, false, false);

		// Token: 0x04002108 RID: 8456
		public bool doBlast;

		// Token: 0x04002109 RID: 8457
		private bool _lastDoBlast;

		// Token: 0x0400210A RID: 8458
		private float _charge;

		// Token: 0x0400210B RID: 8459
		public bool _charging;

		// Token: 0x0400210C RID: 8460
		public bool _fired;

		// Token: 0x0400210D RID: 8461
		private SpriteMap _chargeAnim;

		// Token: 0x0400210E RID: 8462
		private Sound _chargeSound;

		// Token: 0x0400210F RID: 8463
		private Sound _chargeSoundShort;

		// Token: 0x04002110 RID: 8464
		private Sound _unchargeSound;

		// Token: 0x04002111 RID: 8465
		private Sound _unchargeSoundShort;

		// Token: 0x04002112 RID: 8466
		private int _framesSinceBlast;
	}
}
