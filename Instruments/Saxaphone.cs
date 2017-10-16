using System;
using System.Collections.Generic;

namespace DuckGame
{
	// Token: 0x0200024A RID: 586
	[EditorGroup("guns|misc")]
	[BaggedProperty("isFatal", false)]
	public class Saxaphone : Gun
	{
		// Token: 0x060011C2 RID: 4546
		public Saxaphone(float xval, float yval) : base(xval, yval)
		{
			this.ammo = 4;
			this._ammoType = new ATLaser();
			this._ammoType.range = 170f;
			this._ammoType.accuracy = 0.8f;
			this._type = "gun";
			this.graphic = new Sprite("saxaphone", 0f, 0f);
			this.center = new Vec2(20f, 18f);
			this.collisionOffset = new Vec2(-4f, -7f);
			this.collisionSize = new Vec2(8f, 16f);
			base.depth = 0.6f;
			this._barrelOffsetTL = new Vec2(24f, 16f);
			this._fireSound = "smg";
			this._fullAuto = true;
			this._fireWait = 1f;
			this._kickForce = 3f;
			this._holdOffset = new Vec2(6f, 2f);
			this._notePitchBinding.skipLerp = true;
		}

		// Token: 0x060011C3 RID: 4547
		public override void Initialize()
		{
			base.Initialize();
		}

		// Token: 0x060011C4 RID: 4548
		public override void Update()
		{
			Duck duck = this.owner as Duck;
			if (duck != null)
			{
				if (base.isServerForObject)
				{
					this.handPitch = 1f - Mouse.x / Layer.HUD.camera.width * 2f;
					if (duck.inputProfile.Down("SHOOT"))
					{
						this.notePitch = this.handPitch + 0.01f;
					}
					else
					{
						this.notePitch = 0f;
					}
				}
				if (this.notePitch != this.prevNotePitch)
				{
					if (this.notePitch != 0f)
					{
						int num = (int)Math.Round((double)(this.notePitch * 12f));
						if (num < 0)
						{
							num = 0;
						}
						if (num > 12)
						{
							num = 12;
						}
						if (this.noteSound == null)
						{
							this.hitPitch = this.notePitch;
							Sound sound = SFX.Play("sax" + Change.ToString(num), 1f, 0f, 0f, false);
							this.noteSound = sound;
							Level.Add(new MusicNote(base.barrelPosition.x, base.barrelPosition.y, base.barrelVector));
						}
						else
						{
							this.noteSound.Pitch = Maths.Clamp((this.notePitch - this.hitPitch) * 0.1f, -1f, 1f);
						}
					}
					else if (this.noteSound != null)
					{
						this.noteSound.Stop();
						this.noteSound = null;
					}
				}
				if (this._raised)
				{
					this.handAngle = 0f;
					this.handOffset = new Vec2(0f, 0f);
					this._holdOffset = new Vec2(0f, 2f);
					this.collisionOffset = new Vec2(-4f, -7f);
					this.collisionSize = new Vec2(8f, 16f);
					this.OnReleaseAction();
				}
				else
				{
					this.handOffset = new Vec2(5f + (1f - this.handPitch) * 2f, -2f + (1f - this.handPitch) * 4f);
					this.handAngle = (1f - this.handPitch) * 0.4f * (float)this.offDir;
					this._holdOffset = new Vec2(4f + this.handPitch * 2f, this.handPitch * 2f);
					this.collisionOffset = new Vec2(-1f, -7f);
					this.collisionSize = new Vec2(2f, 16f);
				}
			}
			else
			{
				this.collisionOffset = new Vec2(-4f, -7f);
				this.collisionSize = new Vec2(8f, 16f);
			}
			this.prevNotePitch = this.notePitch;
			base.Update();
		}

		// Token: 0x060011C5 RID: 4549
		public override void OnPressAction()
		{
		}

		// Token: 0x060011C6 RID: 4550
		public override void OnReleaseAction()
		{
		}

		// Token: 0x060011C7 RID: 4551
		public override void Fire()
		{
		}

		// Token: 0x04000FE5 RID: 4069
		public StateBinding _notePitchBinding = new StateBinding("notePitch", -1, false, false);

		// Token: 0x04000FE6 RID: 4070
		public StateBinding _handPitchBinding = new StateBinding("handPitch", -1, false, false);

		// Token: 0x04000FE7 RID: 4071
		public float notePitch;

		// Token: 0x04000FE8 RID: 4072
		public float handPitch;

		// Token: 0x04000FE9 RID: 4073
		private float prevNotePitch;

		// Token: 0x04000FEA RID: 4074
		private float hitPitch;

		// Token: 0x04000FEB RID: 4075
		private Sound noteSound;

		// Token: 0x04000FEC RID: 4076
		private List<InstrumentNote> _notes = new List<InstrumentNote>();
	}
}
