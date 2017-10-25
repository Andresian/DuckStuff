using System;
using System.Collections.Generic;

namespace DuckGame
{
	// Token: 0x02000509 RID: 1289
	[BaggedProperty("isSuperWeapon", true)]
	[EditorGroup("guns|melee")]
	public class Chainsaw : Gun
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002300 RID: 8960
		// (set) Token: 0x06002301 RID: 8961
		public override float angle
		{
			get
			{
				return base.angle + this._hold * (float)this.offDir + this._animRot * (float)this.offDir + this._rotSway * (float)this.offDir;
			}
			set
			{
				this._angle = value;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002302 RID: 8962
		public Vec2 barrelStartPos
		{
			get
			{
				return this.position + (this.Offset(base.barrelOffset) - this.position).normalized * 2f;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002303 RID: 8963
		public bool throttle
		{
			get
			{
				return this._throttle;
			}
		}

		// Token: 0x06002304 RID: 8964
		public Chainsaw(float xval, float yval) : base(xval, yval)
		{
			this.ammo = 4;
			this._ammoType = new ATLaser();
			this._ammoType.range = 170f;
			this._ammoType.accuracy = 0.8f;
			this._type = "gun";
			this._sprite = new SpriteMap("chainsaw", 29, 13, false);
			this.graphic = this._sprite;
			this.center = new Vec2(8f, 7f);
			this.collisionOffset = new Vec2(-8f, -6f);
			this.collisionSize = new Vec2(20f, 11f);
			this._barrelOffsetTL = new Vec2(27f, 8f);
			this._sprite.AddAnimation("empty", 1f, true, new int[]
			{
				1
			});
			this._fireSound = "smg";
			this._fullAuto = true;
			this._fireWait = 1f;
			this._kickForce = 3f;
			this._holdOffset = new Vec2(-4f, 4f);
			this.weight = 5f;
			this.physicsMaterial = PhysicsMaterial.Metal;
			this._swordSwing = new SpriteMap("swordSwipe", 32, 32, false);
			this._swordSwing.AddAnimation("swing", 0.6f, false, new int[]
			{
				0,
				1,
				1,
				2
			});
			this._swordSwing.currentAnimation = "swing";
			this._swordSwing.speed = 0f;
			this._swordSwing.center = new Vec2(9f, 25f);
			this.throwSpeedMultiplier = 0.5f;
			this._bouncy = 0.5f;
			this._impactThreshold = 0.3f;
			base.collideSounds.Add("landTV");
		}

		// Token: 0x06002305 RID: 8965
		public override void Initialize()
		{
			this._sprite = new SpriteMap("chainsaw", 29, 13, false);
			if (this.souped)
			{
				this._sprite = new SpriteMap("turbochainsaw", 29, 13, false);
			}
			this.graphic = this._sprite;
			base.Initialize();
		}

		// Token: 0x06002306 RID: 8966
		public override void Terminate()
		{
			this._sound.Kill();
			this._bladeSound.Kill();
			this._bladeSoundLow.Kill();
		}

		// Token: 0x06002307 RID: 8967
		public void Shing(Thing wall)
		{
			if (!this._shing)
			{
				this._struggling = true;
				this._shing = true;
				if (!Chainsaw._playedShing)
				{
					Chainsaw._playedShing = true;
					SFX.Play("chainsawClash", Rando.Float(0.4f, 0.55f), Rando.Float(-0.2f, 0.2f), Rando.Float(-0.1f, 0.1f), false);
				}
				Vec2 normalized = (this.position - base.barrelPosition).normalized;
				Vec2 value = base.barrelPosition;
				for (int i = 0; i < 6; i++)
				{
					Level.Add(Spark.New(value.x, value.y, new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f)), 0.02f));
					value += normalized * 4f;
				}
				this._swordSwing.speed = 0f;
				if (Recorder.currentRecording != null)
				{
					Recorder.currentRecording.LogAction(7);
				}
				if (base.duck != null)
				{
					Duck duck = base.duck;
					if (wall.bottom < duck.top)
					{
						duck.vSpeed += 2f;
						return;
					}
					if (duck.sliding)
					{
						duck.sliding = false;
					}
					if (wall.x > duck.x)
					{
						duck.hSpeed -= 5f;
					}
					else
					{
						duck.hSpeed += 5f;
					}
					duck.vSpeed -= 2f;
				}
			}
		}

		// Token: 0x06002308 RID: 8968
		public override bool Hit(Bullet bullet, Vec2 hitPos)
		{
			SFX.Play("ting", 1f, 0f, 0f, false);
			return base.Hit(bullet, hitPos);
		}

		// Token: 0x06002309 RID: 8969
		public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
		{
			if (this.owner == null && with is Block)
			{
				this.Shing(with);
				if (base.totalImpactPower > 3f)
				{
					this._started = false;
				}
			}
		}

		// Token: 0x0600230A RID: 8970
		public override void ReturnToWorld()
		{
			this._throwSpin = 90f;
		}

		// Token: 0x0600230B RID: 8971
		public void PullEngine()
		{
			float pitch = this.souped ? 0.3f : 0f;
			if (!this._flooded && this._gas > 0f && (this._warmUp > 0.5f || this._engineResistance < 1f))
			{
				SFX.Play("chainsawFire", 1f, 0f, 0f, false);
				this._started = true;
				this._engineSpin = 1.5f;
				for (int i = 0; i < 2; i++)
				{
					Level.Add(SmallSmoke.New(base.x + (float)(this.offDir * 4), base.y + 5f));
				}
				this._flooded = false;
				this._flood = 0f;
			}
			else
			{
				if (this._flooded && this._gas > 0f)
				{
					SFX.Play("chainsawFlooded", 0.9f, Rando.Float(-0.2f, 0.2f), 0f, false);
					this._engineSpin = 1.6f;
				}
				else
				{
					if (this._gas == 0f || Rando.Float(1f) > 0.3f)
					{
						SFX.Play("chainsawPull", 1f, pitch, 0f, false);
					}
					else
					{
						SFX.Play("chainsawFire", 1f, pitch, 0f, false);
					}
					this._engineSpin = 0.8f;
				}
				if (Rando.Float(1f) > 0.8f)
				{
					this._flooded = false;
					this._flood = 0f;
				}
			}
			this._engineResistance -= 0.5f;
			if (this._gas > 0f)
			{
				int num = this._flooded ? 4 : 2;
				for (int j = 0; j < num; j++)
				{
					Level.Add(SmallSmoke.New(base.x + (float)(this.offDir * 4), base.y + 5f));
				}
			}
		}

		// Token: 0x0600230C RID: 8972
		public override void Update()
		{
			base.Update();
			float num = 1f;
			if (this.souped)
			{
				num = 1.3f;
			}
			if (this._swordSwing.finished)
			{
				this._swordSwing.speed = 0f;
			}
			if (this._hitWait > 0)
			{
				this._hitWait--;
			}
			if (this._gas < 0.01f)
			{
				this.ammo = 0;
			}
			this._framesExisting++;
			if (this._framesExisting > 100)
			{
				this._framesExisting = 100;
			}
			float pitch = this.souped ? 0.3f : 0f;
			this._sound.lerpVolume = ((this._started && !this._throttle) ? 0.6f : 0f);
			this._sound.pitch = pitch;
			if (this._started)
			{
				this._warmUp += 0.001f;
				if (this._warmUp > 1f)
				{
					this._warmUp = 1f;
				}
				if (!this._puffClick && this._idleWave > 0.9f)
				{
					this._skipSmoke = !this._skipSmoke;
					if (this._throttle || !this._skipSmoke)
					{
						Level.Add(SmallSmoke.New(base.x + (float)(this.offDir * 4), base.y + 5f, this._smokeFlipper ? -0.1f : 0.8f, 0.7f));
						this._smokeFlipper = !this._smokeFlipper;
						this._puffClick = true;
					}
				}
				else if (this._puffClick && this._idleWave < 0f)
				{
					this._puffClick = false;
				}
				if (this._pullState < 0)
				{
					float num2 = 1f + Maths.NormalizeSection(this._engineSpin, 1f, 2f) * 2f;
					float num3 = this._idleWave;
					if (num2 > 1f)
					{
						num3 = this._spinWave;
					}
					this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(0f, 2f + num3 * num2), 0.23f);
					this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f + num3 * num2), 0.23f);
					float num4 = Maths.NormalizeSection(this._engineSpin, 1f, 2f) * 3f;
					this._rotSway = this._idleWave.normalized * num4 * 0.03f;
				}
				else
				{
					this._rotSway = 0f;
				}
				this._gas -= 3E-05f;
				if (this._throttle)
				{
					this._gas -= 0.0002f;
				}
				if (this._gas < 0f)
				{
					this._gas = 0f;
					this._started = false;
					this._throttle = false;
				}
				if (this._triggerHeld)
				{
					if (this._releasedSincePull)
					{
						if (!this._throttle)
						{
							this._throttle = true;
							SFX.Play("chainsawBladeRevUp", 0.5f, pitch, 0f, false);
						}
						this._engineSpin = Lerp.FloatSmooth(this._engineSpin, 4f, 0.1f, 1f);
					}
				}
				else
				{
					if (this._throttle)
					{
						this._throttle = false;
						if (this._engineSpin > 1.7f)
						{
							SFX.Play("chainsawBladeRevDown", 0.5f, pitch, 0f, false);
						}
					}
					this._engineSpin = Lerp.FloatSmooth(this._engineSpin, 0f, 0.1f, 1f);
					this._releasedSincePull = true;
				}
			}
			else
			{
				this._warmUp -= 0.001f;
				if (this._warmUp < 0f)
				{
					this._warmUp = 0f;
				}
				this._releasedSincePull = false;
				this._throttle = false;
			}
			this._bladeSound.lerpSpeed = 0.1f;
			this._throttleWait = Lerp.Float(this._throttleWait, this._throttle ? 1f : 0f, 0.07f);
			this._bladeSound.lerpVolume = ((this._throttleWait > 0.96f) ? 0.6f : 0f);
			if (this._struggling)
			{
				this._bladeSound.lerpVolume = 0f;
			}
			this._bladeSoundLow.lerpVolume = ((this._throttleWait > 0.96f && this._struggling) ? 0.6f : 0f);
			this._bladeSound.pitch = pitch;
			this._bladeSoundLow.pitch = pitch;
			if (this.owner == null)
			{
				this.collisionOffset = new Vec2(-8f, -6f);
				this.collisionSize = new Vec2(13f, 11f);
			}
			else if (base.duck != null && (base.duck.sliding || base.duck.crouch))
			{
				this.collisionOffset = new Vec2(-8f, -6f);
				this.collisionSize = new Vec2(6f, 11f);
			}
			else
			{
				this.collisionOffset = new Vec2(-8f, -6f);
				this.collisionSize = new Vec2(10f, 11f);
			}
			if (this.owner != null)
			{
				this._resetDuck = false;
				if (this._pullState == -1)
				{
					if (!this._started)
					{
						this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(0f, 2f), 0.25f);
						this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
					}
					this._upWait = 0f;
				}
				else if (this._pullState == 0)
				{
					this._animRot = Lerp.FloatSmooth(this._animRot, -0.4f, 0.15f, 1f);
					this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-2f, -2f), 0.25f);
					this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(-4f, 4f), 0.23f);
					if (this._animRot <= -0.35f)
					{
						this._animRot = -0.4f;
						this._pullState = 1;
						this.PullEngine();
					}
					this._upWait = 0f;
				}
				else if (this._pullState == 1)
				{
					this._releasePull = false;
					this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(2f, 3f), 0.23f);
					this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-4f, -2f), 0.23f);
					this._animRot = Lerp.FloatSmooth(this._animRot, -0.5f, 0.07f, 1f);
					if (this._animRot < -0.45f)
					{
						this._animRot = -0.5f;
						this._pullState = 2;
					}
					this._upWait = 0f;
				}
				else if (this._pullState == 2)
				{
					if (this._releasePull || !this._triggerHeld)
					{
						this._releasePull = true;
						if (this._started)
						{
							this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(0f, 2f + this._idleWave.normalized), 0.23f);
							this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f + this._idleWave.normalized), 0.23f);
							this._animRot = Lerp.FloatSmooth(this._animRot, 0f, 0.1f, 1f);
							if (this._animRot > -0.07f)
							{
								this._animRot = 0f;
								this._pullState = -1;
							}
						}
						else
						{
							this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(-4f, 4f), 0.24f);
							this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-2f, -2f), 0.24f);
							this._animRot = Lerp.FloatSmooth(this._animRot, -0.4f, 0.12f, 1f);
							if (this._animRot > -0.44f)
							{
								this._releasePull = false;
								this._animRot = -0.4f;
								this._pullState = 3;
								this._holdOffset = new Vec2(-4f, 4f);
								this.handOffset = new Vec2(-2f, -2f);
							}
						}
					}
					this._upWait = 0f;
				}
				else if (this._pullState == 3)
				{
					this._releasePull = false;
					this._upWait += 0.1f;
					if (this._upWait > 6f)
					{
						this._pullState = -1;
					}
				}
				this._bladeSpin += this._engineSpin;
				while (this._bladeSpin >= 1f)
				{
					this._bladeSpin -= 1f;
					int num5 = this._sprite.frame + 1;
					if (num5 > 15)
					{
						num5 = 0;
					}
					this._sprite.frame = num5;
				}
				this._engineSpin = Lerp.FloatSmooth(this._engineSpin, 0f, 0.1f, 1f);
				this._engineResistance = Lerp.FloatSmooth(this._engineResistance, 1f, 0.01f, 1f);
				this._hold = -0.4f;
				this.center = new Vec2(8f, 7f);
				this._framesSinceThrown = 0;
			}
			else
			{
				this._rotSway = 0f;
				this._shing = false;
				this._animRot = Lerp.FloatSmooth(this._animRot, 0f, 0.18f, 1f);
				if (this._framesSinceThrown == 1)
				{
					this._throwSpin = base.angleDegrees;
				}
				this._hold = 0f;
				base.angleDegrees = this._throwSpin;
				this.center = new Vec2(8f, 7f);
				bool flag = false;
				bool flag2 = false;
				if ((Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed) > 2f || !base.grounded) && this.gravMultiplier > 0f)
				{
					if (!base.grounded && Level.CheckRect<Block>(this.position + new Vec2(-8f, -6f), this.position + new Vec2(8f, -2f), null) != null)
					{
						flag2 = true;
					}
					if (!flag2 && !this._grounded && Level.CheckPoint<IPlatform>(this.position + new Vec2(0f, 8f), null, null) == null)
					{
						if (this.offDir > 0)
						{
							this._throwSpin += (Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed)) * 1f + 5f;
						}
						else
						{
							this._throwSpin -= (Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed)) * 1f + 5f;
						}
						flag = true;
					}
				}
				if (!flag || flag2)
				{
					this._throwSpin %= 360f;
					if (this._throwSpin < 0f)
					{
						this._throwSpin += 360f;
					}
					if (flag2)
					{
						if (Math.Abs(this._throwSpin - 90f) < Math.Abs(this._throwSpin + 90f))
						{
							this._throwSpin = Lerp.Float(this._throwSpin, 90f, 16f);
						}
						else
						{
							this._throwSpin = Lerp.Float(-90f, 0f, 16f);
						}
					}
					else if (this._throwSpin > 90f && this._throwSpin < 270f)
					{
						this._throwSpin = Lerp.Float(this._throwSpin, 180f, 14f);
					}
					else
					{
						if (this._throwSpin > 180f)
						{
							this._throwSpin -= 360f;
						}
						else if (this._throwSpin < -180f)
						{
							this._throwSpin += 360f;
						}
						this._throwSpin = Lerp.Float(this._throwSpin, 0f, 14f);
					}
				}
			}
			if (Math.Abs(this._angle) > 1f)
			{
				this._flood += 0.005f;
				if (this._flood > 1f)
				{
					this._flooded = true;
					this._started = false;
				}
				this._gasDripFrames++;
				if (this._gas > 0f && this._flooded && this._gasDripFrames > 2)
				{
					FluidData gas = Fluid.Gas;
					gas.amount = 0.003f;
					this._gas -= 0.005f;
					if (this._gas < 0f)
					{
						this._gas = 0f;
					}
					Level.Add(new Fluid(base.x, base.y, Vec2.Zero, gas, null, 1f));
					this._gasDripFrames = 0;
				}
				if (this._gas <= 0f)
				{
					this._started = false;
				}
			}
			else
			{
				this._flood -= 0.008f;
				if (this._flood < 0f)
				{
					this._flood = 0f;
				}
			}
			if (base.duck != null)
			{
				base.duck.frictionMult = 1f;
				if (this._skipSpark > 0)
				{
					this._skipSpark++;
					if (this._skipSpark > 2)
					{
						this._skipSpark = 0;
					}
				}
				if (base.duck.sliding && this._throttle && this._skipSpark == 0)
				{
					if (Level.CheckLine<Block>(this.barrelStartPos + new Vec2(0f, 8f), base.barrelPosition + new Vec2(0f, 8f), null) != null)
					{
						this._skipSpark = 1;
						Vec2 value = this.position + base.barrelVector * 5f;
						for (int i = 0; i < 2; i++)
						{
							Level.Add(Spark.New(value.x, value.y, new Vec2((float)this.offDir * Rando.Float(0f, 2f), Rando.Float(0.5f, 1.5f)), 0.02f));
							value += base.barrelVector * 2f;
							this._fireTrailWait -= 0.5f;
							if (this.souped && this._fireTrailWait <= 0f)
							{
								this._fireTrailWait = 1f;
								SmallFire smallFire = SmallFire.New(value.x, value.y, (float)this.offDir * Rando.Float(0f, 2f), Rando.Float(0.5f, 1.5f), false, null, true, null, false);
								smallFire.waitToHurt = Rando.Float(1f, 2f);
								smallFire.whoWait = (this.owner as Duck);
								Level.Add(smallFire);
							}
						}
						if (this.offDir > 0 && this.owner.hSpeed < (float)(this.offDir * 6) * num)
						{
							this.owner.hSpeed = (float)(this.offDir * 6) * num;
						}
						else if (this.offDir < 0 && this.owner.hSpeed > (float)(this.offDir * 6) * num)
						{
							this.owner.hSpeed = (float)(this.offDir * 6) * num;
						}
					}
					else if (this.offDir > 0 && this.owner.hSpeed < (float)(this.offDir * 3) * num)
					{
						this.owner.hSpeed = (float)(this.offDir * 3) * num;
					}
					else if (this.offDir < 0 && this.owner.hSpeed > (float)(this.offDir * 3) * num)
					{
						this.owner.hSpeed = (float)(this.offDir * 3) * num;
					}
				}
				if (this._pullState == -1)
				{
					if (!this._throttle)
					{
						this._animRot = MathHelper.Lerp(this._animRot, 0.3f, 0.2f);
						this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-2f, 2f), 0.25f);
						this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(-3f, 4f), 0.23f);
					}
					else if (this._shing)
					{
						this._animRot = MathHelper.Lerp(this._animRot, -1.8f, 0.4f);
						this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0f), 0.25f);
						this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
						if (this._animRot < -1.5f)
						{
							this._shing = false;
						}
					}
					else if (base.duck.crouch)
					{
						this._animRot = MathHelper.Lerp(this._animRot, 0.4f, 0.2f);
						this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0f), 0.25f);
						this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
					}
					else if (base.duck.inputProfile.Down("UP"))
					{
						this._animRot = MathHelper.Lerp(this._animRot, -0.9f, 0.2f);
						this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0f), 0.25f);
						this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
					}
					else
					{
						this._animRot = MathHelper.Lerp(this._animRot, 0f, 0.2f);
						this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0f), 0.25f);
						this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
					}
				}
			}
			else if (!this._resetDuck && base.prevOwner != null)
			{
				PhysicsObject physicsObject = base.prevOwner as PhysicsObject;
				if (physicsObject != null)
				{
					physicsObject.frictionMult = 1f;
				}
				this._resetDuck = true;
			}
			if (this._skipDebris > 0)
			{
				this._skipDebris++;
			}
			if (this._skipDebris > 3)
			{
				this._skipDebris = 0;
			}
			this._struggling = false;
			if (this.owner != null && this._started && this._throttle && !this._shing)
			{
				(this.Offset(base.barrelOffset) - this.position).Normalize();
				this.Offset(base.barrelOffset);
				IEnumerable<IAmADuck> enumerable = Level.CheckLineAll<IAmADuck>(this.barrelStartPos, base.barrelPosition);
				Block block3 = Level.CheckLine<Block>(this.barrelStartPos, base.barrelPosition, null);
				if (this.owner != null)
				{
					foreach (MaterialThing materialThing in Level.CheckLineAll<MaterialThing>(this.barrelStartPos, base.barrelPosition))
					{
						if (materialThing.Hurt((materialThing is Door) ? 1.8f : 0.5f))
						{
							if (base.duck != null && base.duck.sliding && materialThing is Door && (materialThing as Door)._jammed)
							{
								materialThing.Destroy(new DTImpale(this));
							}
							else
							{
								this._struggling = true;
								if (base.duck != null)
								{
									base.duck.frictionMult = 4f;
								}
								if (this._skipDebris == 0)
								{
									this._skipDebris = 1;
									Vec2 value2 = Collision.LinePoint(this.barrelStartPos, base.barrelPosition, materialThing.rectangle);
									if (value2 != Vec2.Zero)
									{
										value2 += base.barrelVector * Rando.Float(0f, 3f);
										Vec2 vec = -base.barrelVector.Rotate(Rando.Float(-0.2f, 0.2f), Vec2.Zero);
										if (materialThing.physicsMaterial == PhysicsMaterial.Wood)
										{
											WoodDebris woodDebris = WoodDebris.New(value2.x, value2.y);
											woodDebris.hSpeed = vec.x * 3f;
											woodDebris.vSpeed = vec.y * 3f;
											Level.Add(woodDebris);
										}
										else if (materialThing.physicsMaterial == PhysicsMaterial.Metal)
										{
											Spark spark = Spark.New(value2.x, value2.y, Vec2.Zero, 0.02f);
											spark.hSpeed = vec.x * 3f;
											spark.vSpeed = vec.y * 3f;
											Level.Add(spark);
										}
									}
								}
							}
						}
					}
				}
				bool flag3 = false;
				if (block3 != null && !(block3 is Door))
				{
					this.Shing(block3);
					if (block3 is Window)
					{
						block3.Destroy(new DTImpact(this));
					}
				}
				else
				{
					foreach (Thing thing in Level.current.things[typeof(Sword)])
					{
						Sword sword = (Sword)thing;
						if (sword.owner != null && sword.crouchStance && !sword.jabStance && Collision.LineIntersect(this.barrelStartPos, base.barrelPosition, sword.barrelStartPos, sword.barrelPosition))
						{
							this.Shing(sword);
							sword.Shing();
							sword.owner.hSpeed += (float)this.offDir * 3f;
							sword.owner.vSpeed -= 2f;
							base.duck.hSpeed += -(float)this.offDir * 3f;
							base.duck.vSpeed -= 2f;
							sword.duck.crippleTimer = 1f;
							base.duck.crippleTimer = 1f;
							flag3 = true;
						}
					}
					if (!flag3)
					{
						Thing ignore = null;
						if (base.duck != null)
						{
							ignore = base.duck.GetEquipment(typeof(Helmet));
						}
						QuadLaserBullet quadLaserBullet = Level.CheckLine<QuadLaserBullet>(this.position, base.barrelPosition, null);
						if (quadLaserBullet != null)
						{
							this.Shing(quadLaserBullet);
							Vec2 travel = quadLaserBullet.travel;
							float length = travel.length;
							float num6 = 1f;
							if (this.offDir > 0 && travel.x < 0f)
							{
								num6 = 1.5f;
							}
							else if (this.offDir < 0 && travel.x > 0f)
							{
								num6 = 1.5f;
							}
							if (this.offDir > 0)
							{
								travel = new Vec2(length * num6, 0f);
							}
							else
							{
								travel = new Vec2(-length * num6, 0f);
							}
							quadLaserBullet.travel = travel;
						}
						else
						{
							Helmet helmet = Level.CheckLine<Helmet>(this.barrelStartPos, base.barrelPosition, ignore);
							if (helmet != null && helmet.equippedDuck != null)
							{
								this.Shing(helmet);
								helmet.owner.hSpeed += (float)this.offDir * 3f;
								helmet.owner.vSpeed -= 2f;
								helmet.duck.crippleTimer = 1f;
								helmet.Hurt(0.53f);
								flag3 = true;
							}
							else
							{
								if (base.duck != null)
								{
									ignore = base.duck.GetEquipment(typeof(ChestPlate));
								}
								ChestPlate chestPlate = Level.CheckLine<ChestPlate>(this.barrelStartPos, base.barrelPosition, ignore);
								if (chestPlate != null && chestPlate.equippedDuck != null)
								{
									this.Shing(chestPlate);
									chestPlate.owner.hSpeed += (float)this.offDir * 3f;
									chestPlate.owner.vSpeed -= 2f;
									chestPlate.duck.crippleTimer = 1f;
									chestPlate.Hurt(0.53f);
									flag3 = true;
								}
							}
						}
					}
				}
				if (!flag3)
				{
					foreach (Thing thing2 in Level.current.things[typeof(Chainsaw)])
					{
						Chainsaw chainsaw = (Chainsaw)thing2;
						if (chainsaw != this && chainsaw.owner != null && Collision.LineIntersect(this.barrelStartPos, base.barrelPosition, chainsaw.barrelStartPos, chainsaw.barrelPosition))
						{
							this.Shing(chainsaw);
							chainsaw.Shing(this);
							chainsaw.owner.hSpeed += (float)this.offDir * 2f;
							chainsaw.owner.vSpeed -= 1.5f;
							base.duck.hSpeed += -(float)this.offDir * 2f;
							base.duck.vSpeed -= 1.5f;
							chainsaw.duck.crippleTimer = 1f;
							base.duck.crippleTimer = 1f;
							flag3 = true;
							if (Recorder.currentRecording != null)
							{
								Recorder.currentRecording.LogBonus();
							}
						}
					}
				}
				if (!flag3)
				{
					foreach (IAmADuck amADuck in enumerable)
					{
						if (amADuck != base.duck)
						{
							MaterialThing materialThing2 = amADuck as MaterialThing;
							if (materialThing2 != null)
							{
								materialThing2.velocity += new Vec2((float)this.offDir * 0.8f, -0.8f);
								materialThing2.Destroy(new DTImpale(this));
								if (base.duck != null)
								{
									base.duck._timeSinceChainKill = 0;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600230D RID: 8973
		public override void Draw()
		{
			Chainsaw._playedShing = false;
			if (this._swordSwing.speed > 0f)
			{
				if (base.duck != null)
				{
					this._swordSwing.flipH = (base.duck.offDir <= 0);
				}
				this._swordSwing.alpha = 0.4f;
				this._swordSwing.position = this.position;
				this._swordSwing.depth = base.depth + 1;
				this._swordSwing.Draw();
			}
			if (base.duck != null && (this._pullState == 1 || this._pullState == 2))
			{
				Graphics.DrawLine(this.Offset(new Vec2(-2f, -2f)), base.duck.armPosition + new Vec2(this.handOffset.x * (float)this.offDir, this.handOffset.y), Color.White, 1f, base.duck.depth + 10 - 1);
			}
			base.Draw();
		}

		// Token: 0x0600230E RID: 8974
		public override void OnPressAction()
		{
			Duck duck = this.owner as Duck;
			if (duck.inputProfile.Down("STRAFE") && duck.inputProfile.Down("QUACK"))
			{
				this.souped = true;
				this._sprite.SetAnimation("empty");
			}
			if (this.souped && new Random().Next(1, 11) == 9)
			{
				this.OnDestroybam(null);
			}
			if (!this._started)
			{
				if (this._pullState == -1)
				{
					this._pullState = 0;
					return;
				}
				if (this._pullState == 3)
				{
					this._pullState = 1;
					this.PullEngine();
				}
			}
		}

		// Token: 0x0600230F RID: 8975
		public override void Fire()
		{
		}

		// Token: 0x0600254F RID: 9551
		protected bool OnDestroybam(DestroyType type = null)
		{
			if (!base.isServerForObject)
			{
				return false;
			}
			ATRCShrapnel shrap = new ATRCShrapnel();
			shrap.MakeNetEffect(this.position, false);
			List<Bullet> firedBullets = new List<Bullet>();
			for (int i = 0; i < 20; i++)
			{
				float dir = (float)i * 18f - 5f + Rando.Float(10f);
				shrap = new ATRCShrapnel();
				shrap.range = 55f + Rando.Float(14f);
				Bullet bullet = new Bullet(base.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * 6.0), base.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * 6.0), shrap, dir, null, false, -1f, false, true);
				bullet.firedFrom = this;
				firedBullets.Add(bullet);
				Level.Add(bullet);
			}
			if (Network.isActive)
			{
				Send.Message(new NMFireGun(null, firedBullets, 0, false, 4, false), NetMessagePriority.ReliableOrdered, null);
				firedBullets.Clear();
			}
			Level.Remove(this);
			return true;
		}

		// Token: 0x040021F9 RID: 8697
		public StateBinding _angleOffsetBinding = new StateBinding("_hold", -1, false, false);

		// Token: 0x040021FA RID: 8698
		public StateBinding _throwSpinBinding = new StateBinding("_throwSpin", -1, false, false);

		// Token: 0x040021FB RID: 8699
		public StateBinding _gasBinding = new StateBinding("_gas", -1, false, false);

		// Token: 0x040021FC RID: 8700
		public StateBinding _floodBinding = new StateBinding("_flood", -1, false, false);

		// Token: 0x040021FD RID: 8701
		public StateBinding _chainsawStateBinding = new StateFlagBinding(new string[]
		{
			"_flooded",
			"_started",
			"_throttle"
		});

		// Token: 0x040021FE RID: 8702
		public EditorProperty<bool> souped = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);

		// Token: 0x040021FF RID: 8703
		private float _hold;

		// Token: 0x04002200 RID: 8704
		private bool _shing;

		// Token: 0x04002201 RID: 8705
		private static bool _playedShing;

		// Token: 0x04002202 RID: 8706
		public float _throwSpin;

		// Token: 0x04002203 RID: 8707
		private int _framesExisting;

		// Token: 0x04002204 RID: 8708
		private int _hitWait;

		// Token: 0x04002205 RID: 8709
		private SpriteMap _swordSwing;

		// Token: 0x04002206 RID: 8710
		private SpriteMap _sprite;

		// Token: 0x04002207 RID: 8711
		private float _rotSway;

		// Token: 0x04002208 RID: 8712
		public bool _started;

		// Token: 0x04002209 RID: 8713
		private int _pullState = -1;

		// Token: 0x0400220A RID: 8714
		private float _animRot;

		// Token: 0x0400220B RID: 8715
		private float _upWait;

		// Token: 0x0400220C RID: 8716
		private float _engineSpin;

		// Token: 0x0400220D RID: 8717
		private float _bladeSpin;

		// Token: 0x0400220E RID: 8718
		private float _engineResistance = 1f;

		// Token: 0x0400220F RID: 8719
		private SinWave _idleWave = 0.6f;

		// Token: 0x04002210 RID: 8720
		private SinWave _spinWave = 1f;

		// Token: 0x04002211 RID: 8721
		private bool _puffClick;

		// Token: 0x04002212 RID: 8722
		private float _warmUp;

		// Token: 0x04002213 RID: 8723
		public bool _flooded;

		// Token: 0x04002214 RID: 8724
		private int _gasDripFrames;

		// Token: 0x04002215 RID: 8725
		public float _flood;

		// Token: 0x04002216 RID: 8726
		private bool _releasePull;

		// Token: 0x04002217 RID: 8727
		public float _gas = 1f;

		// Token: 0x04002218 RID: 8728
		private bool _struggling;

		// Token: 0x04002219 RID: 8729
		private bool _throttle;

		// Token: 0x0400221A RID: 8730
		private float _throttleWait;

		// Token: 0x0400221B RID: 8731
		private bool _releasedSincePull;

		// Token: 0x0400221C RID: 8732
		private int _skipDebris;

		// Token: 0x0400221D RID: 8733
		private bool _resetDuck;

		// Token: 0x0400221E RID: 8734
		private int _skipSpark;

		// Token: 0x0400221F RID: 8735
		private ConstantSound _sound = new ConstantSound("chainsawIdle", 0f, 0f, "chainsawIdleMulti");

		// Token: 0x04002220 RID: 8736
		private ConstantSound _bladeSound = new ConstantSound("chainsawBladeLoop", 0f, 0f, "chainsawBladeLoopMulti");

		// Token: 0x04002221 RID: 8737
		private ConstantSound _bladeSoundLow = new ConstantSound("chainsawBladeLoopLow", 0f, 0f, "chainsawBladeLoopLowMulti");

		// Token: 0x04002222 RID: 8738
		private bool _smokeFlipper;

		// Token: 0x04002223 RID: 8739
		private float _fireTrailWait;

		// Token: 0x04002224 RID: 8740
		private bool _skipSmoke;
	}
}
