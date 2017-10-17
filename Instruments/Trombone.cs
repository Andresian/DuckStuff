using System;
using System.Collections.Generic;

namespace DuckGame
{
	// Token: 0x020002C9 RID: 713
	[BaggedProperty("isFatal", false)]
	[EditorGroup("guns|misc")]
	public class Trombone : Gun
	{
		// Token: 0x060015CA RID: 5578
		public Trombone(float xval, float yval) : base(xval, yval)
		{
			this.ammo = 4;
			this._ammoType = new ATLaser();
			this._ammoType.range = 170f;
			this._ammoType.accuracy = 0.8f;
			this._type = "gun";
			this.graphic = new Sprite("tromboneBody", 0f, 0f);
			this.center = new Vec2(10f, 16f);
			this.collisionOffset = new Vec2(-4f, -5f);
			this.collisionSize = new Vec2(8f, 11f);
			this._barrelOffsetTL = new Vec2(19f, 14f);
			this._fireSound = "smg";
			this._fullAuto = true;
			this._fireWait = 1f;
			this._kickForce = 3f;
			this._holdOffset = new Vec2(6f, 2f);
			this._slide = new Sprite("tromboneSlide", 0f, 0f);
			this._slide.CenterOrigin();
			this._notePitchBinding.skipLerp = true;
		}

		// Token: 0x060015CB RID: 5579
		public override void Initialize()
		{
			base.Initialize();
		}

		// Token: 0x060015CC RID: 5580
		public float NormalizePitch(float val)
		{
			return val;
		}

		// Token: 0x060015CD RID: 5581
		public override void Update() //When used
		{
			Duck duck = this.owner as Duck;
			if (duck != null)
			{
				if (base.isServerForObject)
				{
					this.handPitch = 1f - Mouse.x / Layer.HUD.camera.width * 2f;
					if (this.handPitch < 0f) //Prevents hand and pitch going too far
					{
						float currentpitch = this.handPitch;
						float neededpitch = 0f - this.handPitch;
						this.handPitch = currentpitch + neededpitch;
					}
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
							Sound sound = SFX.Play("trombone" + Change.ToString(num), 1f, 0f, 0f, false);
							this.noteSound = sound;
							Level.Add(new MusicNote(base.barrelPosition.x, base.barrelPosition.y, base.barrelVector));
						}
						else
						{
							this.noteSound.Pitch = Maths.Clamp(this.notePitch - this.hitPitch, -1f, 1f);
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
				}
				else //Hand pitch
				{
					this.handOffset = new Vec2(6f + (1f - this.handPitch) * 4f, -4f + (1f - this.handPitch) * 4f);
					this.handAngle = (1f - this.handPitch) * 0.4f * (float)this.offDir;
					this._holdOffset = new Vec2(5f + this.handPitch * 2f, -9f + this.handPitch * 2f);
					this.collisionOffset = new Vec2(-4f, -7f);
					this.collisionSize = new Vec2(2f, 16f);
					this._slideVal = 1f - this.handPitch;
				}
			}
			else
			{
				this.collisionOffset = new Vec2(-4f, -5f);
				this.collisionSize = new Vec2(8f, 11f);
			}
			this.prevNotePitch = this.notePitch;
			base.Update();
		}

		// Token: 0x060015CE RID: 5582
		public override void OnPressAction()
		{
		}

		// Token: 0x060015CF RID: 5583
		public override void OnReleaseAction()
		{
		}

		// Token: 0x060015D0 RID: 5584
		public override void Fire()
		{
		}

		// Token: 0x060015D1 RID: 5585
		public override void Draw()
		{
			base.Draw();
			base.Draw(this._slide, new Vec2(6f + this._slideVal * 8f, 0f), -1);
		}

		// Token: 0x0400132D RID: 4909
		public StateBinding _notePitchBinding = new StateBinding("notePitch", -1, false, false);

		// Token: 0x0400132E RID: 4910
		public StateBinding _handPitchBinding = new StateBinding("handPitch", -1, false, false);

		// Token: 0x0400132F RID: 4911
		public float notePitch;

		// Token: 0x04001330 RID: 4912
		public float handPitch;

		// Token: 0x04001331 RID: 4913
		private float prevNotePitch;

		// Token: 0x04001332 RID: 4914
		private float hitPitch;

		// Token: 0x04001333 RID: 4915
		private Sound noteSound;

		// Token: 0x04001334 RID: 4916
		private List<InstrumentNote> _notes = new List<InstrumentNote>();

		// Token: 0x04001335 RID: 4917
		private Sprite _slide;

		// Token: 0x04001336 RID: 4918
		private float _slideVal;
	}
}
