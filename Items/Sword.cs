using System;
using System.Collections.Generic;

namespace DuckGame
{
	// Token: 0x02000177 RID: 375
	[EditorGroup("guns|melee")]
	public class Sword : Gun
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000CBD RID: 3261
		// (set) Token: 0x06000CBE RID: 3262
		public override float angle
		{
			get
			{
				if (this._drawing)
				{
					return this._angle;
				}
				return base.angle + (this._swing + this._hold) * (float)this.offDir;
			}
			set
			{
				this._angle = Mouse.x;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000CBF RID: 3263
		public bool jabStance
		{
			get
			{
				return this._jabStance;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000CC0 RID: 3264
		public bool crouchStance
		{
			get
			{
				return this._crouchStance;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000CC1 RID: 3265
		public Vec2 barrelStartPos
		{
			get
			{
				if (this.owner == null)
				{
					return this.position - (this.Offset(base.barrelOffset) - this.position).normalized * 6f;
				}
				if (this._slamStance)
				{
					return this.position + (this.Offset(base.barrelOffset) - this.position).normalized * 12f;
				}
				return this.position + (this.Offset(base.barrelOffset) - this.position).normalized * 2f;
			}
		}

		// Token: 0x06000CC2 RID: 3266
		public Sword(float xval, float yval) : base(xval, yval)
		{
			this.ammo = 4;
			this._ammoType = new ATLaser();
			this._ammoType.range = 170f;
			this._ammoType.accuracy = 0.8f;
			this._type = "gun";
			this.graphic = new Sprite("sword", 0f, 0f);
			this.center = new Vec2(4f, 21f);
			this.collisionOffset = new Vec2(-2f, -16f);
			this.collisionSize = new Vec2(4f, 18f);
			this._barrelOffsetTL = new Vec2(4f, 1f);
			this._fireSound = "smg";
			this._fullAuto = true;
			this._fireWait = 1f;
			this._kickForce = 3f;
			this._holdOffset = new Vec2(-4f, 4f);
			this.weight = 0.9f;
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
			this._bouncy = 0.5f;
			this._impactThreshold = 0.3f;
		}

		// Token: 0x06000CC3 RID: 3267
		public override void Initialize()
		{
			base.Initialize();
		}

		// Token: 0x06000CC4 RID: 3268
		public override void CheckIfHoldObstructed()
		{
			Duck duckOwner = this.owner as Duck;
			if (duckOwner != null)
			{
				duckOwner.holdObstructed = false;
			}
		}

		// Token: 0x06000CC5 RID: 3269
		public override void Thrown()
		{
		}

		// Token: 0x06000CC6 RID: 3270
		public void Shing()
		{
			if (!this._shing)
			{
				this._pullBack = false;
				this._swinging = false;
				this._shing = true;
				this._swingPress = false;
				if (!Sword._playedShing)
				{
					Sword._playedShing = true;
					SFX.Play("swordClash", Rando.Float(0.6f, 0.7f), Rando.Float(-0.1f, 0.1f), Rando.Float(-0.1f, 0.1f), false);
				}
				Vec2 vec = (this.position - base.barrelPosition).normalized;
				Vec2 start = base.barrelPosition;
				for (int i = 0; i < 6; i++)
				{
					Level.Add(Spark.New(start.x, start.y, new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f)), 0.02f));
					start += vec * 4f;
				}
				this._swung = false;
				this._swordSwing.speed = 0f;
			}
		}

		// Token: 0x06000CC7 RID: 3271
		public override bool Hit(Bullet bullet, Vec2 hitPos)
		{
			if (base.duck != null)
			{
				if (this.blocked == 0)
				{
					base.duck.AddCoolness(1);
				}
				else
				{
					this.blocked += 1;
					if (this.blocked > 4)
					{
						this.blocked = 1;
						base.duck.AddCoolness(1);
					}
				}
				SFX.Play("ting", 1f, 0f, 0f, false);
				return base.Hit(bullet, hitPos);
			}
			return false;
		}

		// Token: 0x06000CC8 RID: 3272
		public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
		{
			if (this._wasLifted && this.owner == null && with is Block)
			{
				this.Shing();
				this._framesSinceThrown = 15;
			}
		}

		// Token: 0x06000CC9 RID: 3273
		public override void ReturnToWorld()
		{
			this._throwSpin = 90f;
			this.collisionOffset = new Vec2(-2f, -16f);
			this.collisionSize = new Vec2(4f, 18f);
			if (this._wasLifted)
			{
				this.collisionOffset = new Vec2(-4f, -2f);
				this.collisionSize = new Vec2(8f, 4f);
			}
		}

		// Token: 0x06000CCA RID: 3274
		public override void Update()
		{
			base.Update();
			if (this._swordSwing.finished)
			{
				this._swordSwing.speed = 0f;
			}
			if (this._hitWait > 0)
			{
				this._hitWait--;
			}
			this._framesExisting++;
			if (this._framesExisting > 100)
			{
				this._framesExisting = 100;
			}
			if (Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed) > 4f && this._framesExisting > 10)
			{
				this._wasLifted = true;
			}
			if (this.owner != null)
			{
				this._hold = -0.4f;
				this._wasLifted = true;
				this.center = new Vec2(4f, 21f);
				this._framesSinceThrown = 0;
			}
			else
			{
				if (this._framesSinceThrown == 1)
				{
					this._throwSpin = Maths.RadToDeg(this.angle) - 90f;
					this._hold = 0f;
					this._swing = 0f;
				}
				if (this._wasLifted)
				{
					base.angleDegrees = 90f + this._throwSpin;
					this.center = new Vec2(4f, 11f);
				}
				this._volatile = false;
				bool spinning = false;
				bool againstWall = false;
				if (Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed) > 2f || !base.grounded)
				{
					if (!base.grounded && Level.CheckRect<Block>(this.position + new Vec2(-6f, -6f), this.position + new Vec2(6f, -2f), null) != null)
					{
						againstWall = true;
						if (this.vSpeed > 4f)
						{
							this._volatile = true;
						}
					}
					if (!againstWall && !this._grounded && Level.CheckPoint<IPlatform>(this.position + new Vec2(0f, 8f), null, null) == null)
					{
						if (this.hSpeed > 0f)
						{
							this._throwSpin += (Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed)) * 2f + 4f;
						}
						else
						{
							this._throwSpin -= (Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed)) * 2f + 4f;
						}
						spinning = true;
					}
				}
				if (this._framesExisting > 15 && Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed) > 3f)
				{
					this._volatile = true;
				}
				if (!spinning || againstWall)
				{
					this._throwSpin %= 360f;
					if (againstWall)
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
				if (this._volatile && this._hitWait == 0)
				{
					(this.Offset(base.barrelOffset) - this.position).Normalize();
					this.Offset(base.barrelOffset);
					bool rebound = false;
					foreach (Thing thing in Level.current.things[typeof(Sword)])
					{
						Sword s = (Sword)thing;
						if (s != this && s.owner != null && s._crouchStance && !s._jabStance && !s._jabStance && ((this.hSpeed > 0f && s.x > base.x - 4f) || (this.hSpeed < 0f && s.x < base.x + 4f)) && Collision.LineIntersect(this.barrelStartPos, base.barrelPosition, s.barrelStartPos, s.barrelPosition))
						{
							this.Shing();
							s.Shing();
							s.owner.hSpeed += (float)this.offDir * 1f;
							s.owner.vSpeed -= 1f;
							rebound = true;
							this._hitWait = 4;
							this.hSpeed = -this.hSpeed * 0.6f;
						}
					}
					int waitFrames = 12;
					if (!rebound)
					{
						foreach (Thing thing2 in Level.current.things[typeof(Chainsaw)])
						{
							Chainsaw s2 = (Chainsaw)thing2;
							if (s2.owner != null && s2.throttle && Collision.LineIntersect(this.barrelStartPos, base.barrelPosition, s2.barrelStartPos, s2.barrelPosition))
							{
								this.Shing();
								s2.Shing(this);
								s2.owner.hSpeed += (float)this.offDir * 1f;
								s2.owner.vSpeed -= 1f;
								rebound = true;
								this.hSpeed = -this.hSpeed * 0.6f;
								this._hitWait = 4;
								if (Recorder.currentRecording != null)
								{
									Recorder.currentRecording.LogBonus();
								}
							}
						}
						if (!rebound)
						{
							Helmet helmetHit = Level.CheckLine<Helmet>(this.barrelStartPos, base.barrelPosition, null);
							if (helmetHit != null && helmetHit.equippedDuck != null && (helmetHit.owner != base.prevOwner || (int)this._framesSinceThrown > waitFrames))
							{
								this.hSpeed = -this.hSpeed * 0.6f;
								this.Shing();
								rebound = true;
								this._hitWait = 4;
							}
							else
							{
								ChestPlate chestHit = Level.CheckLine<ChestPlate>(this.barrelStartPos, base.barrelPosition, null);
								if (chestHit != null && chestHit.equippedDuck != null && (chestHit.owner != base.prevOwner || (int)this._framesSinceThrown > waitFrames))
								{
									this.hSpeed = -this.hSpeed * 0.6f;
									this.Shing();
									rebound = true;
									this._hitWait = 4;
								}
							}
						}
					}
					if (!rebound && base.isServerForObject)
					{
						foreach (IAmADuck d in Level.CheckLineAll<IAmADuck>(this.barrelStartPos, base.barrelPosition))
						{
							if (d != base.duck)
							{
								MaterialThing realThing = d as MaterialThing;
								if (realThing != null && (realThing != base.prevOwner || (int)this._framesSinceThrown > waitFrames))
								{
									realThing.Destroy(new DTImpale(this));
									if (Recorder.currentRecording != null)
									{
										Recorder.currentRecording.LogBonus();
									}
								}
							}
						}
					}
				}
			}
			if (this.owner == null)
			{
				this._swinging = false;
				this._jabStance = false;
				this._crouchStance = false;
				this._pullBack = false;
				this._swung = false;
				this._shing = false;
				this._swing = 0f;
				this._swingPress = false;
				this._slamStance = false;
				this._unslam = 0;
			}
			if (base.isServerForObject)
			{
				if (this._unslam > 1)
				{
					this._unslam--;
					this._slamStance = true;
				}
				else if (this._unslam == 1)
				{
					this._unslam = 0;
					this._slamStance = false;
				}
				if (this._pullBack)
				{
					if (base.duck != null)
					{
						if (this._jabStance)
						{
							this._pullBack = false;
							this._swinging = true;
						}
						else
						{
							this._swinging = true;
							this._pullBack = false;
						}
					}
				}
				else if (this._swinging)
				{
					if (this._jabStance)
					{
						this._addOffsetX = MathHelper.Lerp(this._addOffsetX, 3f, 0.4f);
						if (this._addOffsetX > 2f && !this.action)
						{
							this._swinging = false;
						}
					}
					else if (base.raised)
					{
						this._swing = MathHelper.Lerp(this._swing, -2.8f, 0.2f);
						if (this._swing < -2.4f && !this.action)
						{
							this._swinging = false;
							this._swing = 1.8f;
						}
					}
					else
					{
						this._swing = MathHelper.Lerp(this._swing, 2.1f, 0.4f);
						if (this._swing > 1.8f && !this.action)
						{
							this._swinging = false;
							this._swing = 1.8f;
						}
					}
				}
				else
				{
					if (!this._swinging && (!this._swingPress || this._shing || (this._jabStance && this._addOffsetX < 1f) || (!this._jabStance && this._swing < 1.6f)))
					{
						if (this._jabStance)
						{
							this._swing = MathHelper.Lerp(this._swing, 1.75f, 0.4f);
							if (this._swing > 1.55f)
							{
								this._swing = 1.55f;
								this._shing = false;
								this._swung = false;
							}
							this._addOffsetX = MathHelper.Lerp(this._addOffsetX, -12f, 0.45f);
							if (this._addOffsetX < -12f)
							{
								this._addOffsetX = -12f;
							}
							this._addOffsetY = MathHelper.Lerp(this._addOffsetY, -4f, 0.35f);
							if (this._addOffsetX < -3f)
							{
								this._addOffsetY = -3f;
							}
						}
						else if (this._slamStance)
						{
							this._swing = MathHelper.Lerp(this._swing, 3.14f, 0.8f);
							if (this._swing > 3.1f && this._unslam == 0)
							{
								this._swing = 3.14f;
								this._shing = false;
								this._swung = true;
							}
							this._addOffsetX = MathHelper.Lerp(this._addOffsetX, -5f, 0.45f);
							if (this._addOffsetX < -4.6f)
							{
								this._addOffsetX = -5f;
							}
							this._addOffsetY = MathHelper.Lerp(this._addOffsetY, -6f, 0.35f);
							if (this._addOffsetX < -5.5f)
							{
								this._addOffsetY = -6f;
							}
						}
						else
						{
							this._swing = MathHelper.Lerp(this._swing, -0.22f, 0.36f);
							this._addOffsetX = MathHelper.Lerp(this._addOffsetX, 1f, 0.2f);
							if (this._addOffsetX > 0f)
							{
								this._addOffsetX = 0f;
							}
							this._addOffsetY = MathHelper.Lerp(this._addOffsetY, 1f, 0.2f);
							if (this._addOffsetY > 0f)
							{
								this._addOffsetY = 0f;
							}
						}
					}
					if ((this._swing < 0f || this._jabStance) && this._swing < 0f)
					{
						this._swing = 0f;
						this._shing = false;
						this._swung = false;
					}
				}
			}
			if (base.duck != null)
			{
				this.collisionOffset = new Vec2(-4f, 0f);
				this.collisionSize = new Vec2(4f, 4f);
				if (this._crouchStance && !this._jabStance)
				{
					this.collisionOffset = new Vec2(-2f, -19f);
					this.collisionSize = new Vec2(4f, 16f);
					this.thickness = 3f;
				}
				this._swingPress = false;
				if (!this._pullBack && !this._swinging)
				{
					this._crouchStance = false;
					this._jabStance = false;
					if (base.duck.crouch)
					{
						if (!this._pullBack && !this._swinging && base.duck.inputProfile.Down((this.offDir > 0) ? "LEFT" : "RIGHT"))
						{
							this._jabStance = true;
						}
						this._crouchStance = true;
					}
					if (!this._crouchStance || this._jabStance)
					{
						this._slamStance = false;
					}
				}
				if (!this._crouchStance)
				{
					this._hold = -0.4f;
					this.handOffset = new Vec2(this._addOffsetX, this._addOffsetY);
					this._holdOffset = new Vec2(-4f + this._addOffsetX, 4f + this._addOffsetY);
				}
				else
				{
					this._hold = 0f;
					this._holdOffset = new Vec2(0f + this._addOffsetX, 4f + this._addOffsetY);
					this.handOffset = new Vec2(3f + this._addOffsetX, this._addOffsetY);
				}
			}
			else
			{
				this.collisionOffset = new Vec2(-2f, -16f);
				this.collisionSize = new Vec2(4f, 18f);
				if (this._wasLifted)
				{
					this.collisionOffset = new Vec2(-4f, -2f);
					this.collisionSize = new Vec2(8f, 4f);
				}
				this.thickness = 0f;
			}
			if ((this._swung || this._swinging) && !this._shing)
			{
				(this.Offset(base.barrelOffset) - this.position).Normalize();
				this.Offset(base.barrelOffset);
				IEnumerable<IAmADuck> hit2 = Level.CheckLineAll<IAmADuck>(this.barrelStartPos, base.barrelPosition);
				Block wallHit = Level.CheckLine<Block>(this.barrelStartPos, base.barrelPosition, null);
				if (wallHit != null && !this._slamStance)
				{
					if (this.offDir < 0 && wallHit.x > base.x)
					{
						wallHit = null;
					}
					else if (this.offDir > 0 && wallHit.x < base.x)
					{
						wallHit = null;
					}
				}
				bool clashed = false;
				if (wallHit != null)
				{
					this.Shing();
					if (this._slamStance)
					{
						this._swung = false;
						this._unslam = 20;
						this.owner.vSpeed = -5f;
					}
					if (wallHit is Window)
					{
						wallHit.Destroy(new DTImpact(this));
					}
				}
				else if (!this._jabStance && !this._slamStance)
				{
					Thing ignore = null;
					if (base.duck != null)
					{
						ignore = base.duck.GetEquipment(typeof(Helmet));
					}
					Vec2 barrel = base.barrelPosition + base.barrelVector * 3f;
					Vec2 p3 = new Vec2((this.position.x < barrel.x) ? this.position.x : barrel.x, (this.position.y < barrel.y) ? this.position.y : barrel.y);
					Vec2 p2 = new Vec2((this.position.x > barrel.x) ? this.position.x : barrel.x, (this.position.y > barrel.y) ? this.position.y : barrel.y);
					QuadLaserBullet laserHit = Level.CheckRect<QuadLaserBullet>(p3, p2, null);
					if (laserHit != null)
					{
						this.Shing();
						base.Fondle(laserHit);
						laserHit.safeFrames = 8;
						laserHit.safeDuck = base.duck;
						Vec2 travel = laserHit.travel;
						float mag = travel.length;
						float mul = 1f;
						if (this.offDir > 0 && travel.x < 0f)
						{
							mul = 1.5f;
						}
						else if (this.offDir < 0 && travel.x > 0f)
						{
							mul = 1.5f;
						}
						if (this.offDir > 0)
						{
							travel = new Vec2(mag * mul, 0f);
						}
						else
						{
							travel = new Vec2(-mag * mul, 0f);
						}
						laserHit.travel = travel;
					}
					else
					{
						Helmet helmetHit2 = Level.CheckLine<Helmet>(this.barrelStartPos, base.barrelPosition, ignore);
						if (helmetHit2 != null && helmetHit2.equippedDuck != null)
						{
							this.Shing();
							helmetHit2.owner.hSpeed += (float)this.offDir * 3f;
							helmetHit2.owner.vSpeed -= 2f;
							helmetHit2.duck.crippleTimer = 1f;
							helmetHit2.Hurt(0.53f);
							clashed = true;
						}
						else
						{
							if (base.duck != null)
							{
								ignore = base.duck.GetEquipment(typeof(ChestPlate));
							}
							ChestPlate chestHit2 = Level.CheckLine<ChestPlate>(this.barrelStartPos, base.barrelPosition, ignore);
							if (chestHit2 != null && chestHit2.equippedDuck != null)
							{
								this.Shing();
								chestHit2.owner.hSpeed += (float)this.offDir * 3f;
								chestHit2.owner.vSpeed -= 2f;
								chestHit2.duck.crippleTimer = 1f;
								chestHit2.Hurt(0.53f);
								clashed = true;
							}
						}
					}
				}
				if (!clashed)
				{
					foreach (Thing thing3 in Level.current.things[typeof(Sword)])
					{
						Sword s3 = (Sword)thing3;
						if (s3 != this && s3.duck != null && !this._jabStance && !s3._jabStance && base.duck != null && Collision.LineIntersect(this.barrelStartPos, base.barrelPosition, s3.barrelStartPos, s3.barrelPosition))
						{
							this.Shing();
							s3.Shing();
							s3.owner.hSpeed += (float)this.offDir * 3f;
							s3.owner.vSpeed -= 2f;
							base.duck.hSpeed += -(float)this.offDir * 3f;
							base.duck.vSpeed -= 2f;
							s3.duck.crippleTimer = 1f;
							base.duck.crippleTimer = 1f;
							clashed = true;
						}
					}
				}
				if (clashed)
				{
					return;
				}
				using (IEnumerator<IAmADuck> enumerator5 = hit2.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						IAmADuck d2 = enumerator5.Current;
						if (d2 != base.duck)
						{
							MaterialThing realThing2 = d2 as MaterialThing;
							if (realThing2 != null)
							{
								realThing2.Destroy(new DTImpale(this));
							}
						}
					}
					return;
				}
			}
			if (this._crouchStance && base.duck != null)
			{
				foreach (IAmADuck d3 in Level.CheckLineAll<IAmADuck>(this.barrelStartPos, base.barrelPosition))
				{
					if (d3 != base.duck)
					{
						MaterialThing realThing3 = d3 as MaterialThing;
						if (realThing3 != null)
						{
							if (realThing3.vSpeed > 0.5f && realThing3.bottom < this.position.y - 8f && realThing3.left < base.barrelPosition.x && realThing3.right > base.barrelPosition.x)
							{
								realThing3.Destroy(new DTImpale(this));
							}
							else if (!this._jabStance && !realThing3.destroyed && ((this.offDir > 0 && realThing3.x > base.duck.x) || (this.offDir < 0 && realThing3.x < base.duck.x)))
							{
								if (realThing3 is Duck)
								{
									(realThing3 as Duck).crippleTimer = 1f;
								}
								else if ((base.duck.x > realThing3.x && realThing3.hSpeed > 1.5f) || (base.duck.x < realThing3.x && realThing3.hSpeed < -1.5f))
								{
									realThing3.Destroy(new DTImpale(this));
								}
								base.Fondle(realThing3);
								realThing3.hSpeed = (float)this.offDir * 3f;
								realThing3.vSpeed = -2f;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000CCB RID: 3275
		public override void Draw()
		{
			Sword._playedShing = false;
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
			base.alpha = 1f;
			Vec2 pos = this.position;
			Depth d = base.depth;
			this.graphic.color = Color.White;
			if ((this.owner == null && base.velocity.length > 1f) || this._swing != 0f)
			{
				float rlAngle = this.angle;
				this._drawing = true;
				float a = this._angle;
				this.angle = rlAngle;
				for (int i = 0; i < 7; i++)
				{
					base.Draw();
					if (this._lastAngles.Count > i)
					{
						this._angle = this._lastAngles[i];
					}
					if (this._lastPositions.Count <= i)
					{
						break;
					}
					this.position = this._lastPositions[i];
					if (this.owner != null)
					{
						this.position += this.owner.velocity;
					}
					base.depth -= 2;
					base.alpha -= 0.15f;
					this.graphic.color = Color.Red;
				}
				this.position = pos;
				base.depth = d;
				base.alpha = 1f;
				this._angle = a;
				base.xscale = 1f;
				this._drawing = false;
			}
			else
			{
				base.Draw();
			}
			this._lastAngles.Insert(0, this.angle);
			this._lastPositions.Insert(0, this.position);
			if (this._lastAngles.Count > 2)
			{
				this._lastAngles.Insert(0, (this._lastAngles[0] + this._lastAngles[2]) / 2f);
				this._lastPositions.Insert(0, (this._lastPositions[0] + this._lastPositions[2]) / 2f);
			}
			if (this._lastAngles.Count > 8)
			{
				this._lastAngles.RemoveAt(this._lastAngles.Count - 1);
			}
			if (this._lastPositions.Count > 8)
			{
				this._lastPositions.RemoveAt(this._lastPositions.Count - 1);
			}
		}

		// Token: 0x06000CCC RID: 3276
		public override void OnPressAction()
		{
			if ((this._crouchStance && this._jabStance && !this._swinging) || (!this._crouchStance && !this._swinging && this._swing < 0.1f))
			{
				this._pullBack = true;
				this._swung = true;
				this._shing = false;
				SFX.Play("swipe", Rando.Float(0.8f, 1f), Rando.Float(-0.1f, 0.1f), 0f, false);
				if (!this._jabStance)
				{
					this._swordSwing.speed = 1f;
					this._swordSwing.frame = 0;
					return;
				}
			}
			else if (this._crouchStance && !this._jabStance && base.duck != null && !base.duck.grounded)
			{
				this._slamStance = true;
			}
		}

		// Token: 0x06000CCD RID: 3277
		public override void Fire()
		{
		}

		// Token: 0x04000AA2 RID: 2722
		public StateBinding _swingBinding = new StateBinding(true, "_swing", -1, false, false);

		// Token: 0x04000AA3 RID: 2723
		public StateBinding _holdBinding = new StateBinding(true, "_hold", -1, false, false);

		// Token: 0x04000AA4 RID: 2724
		public StateBinding _jabStanceBinding = new StateBinding("_jabStance", -1, false, false);

		// Token: 0x04000AA5 RID: 2725
		public StateBinding _crouchStanceBinding = new StateBinding("_crouchStance", -1, false, false);

		// Token: 0x04000AA6 RID: 2726
		public StateBinding _slamStanceBinding = new StateBinding("_slamStance", -1, false, false);

		// Token: 0x04000AA7 RID: 2727
		public StateBinding _pullBackBinding = new StateBinding(true, "_pullBack", -1, false, false);

		// Token: 0x04000AA8 RID: 2728
		public StateBinding _swingingBinding = new StateBinding("_swinging", -1, false, false);

		// Token: 0x04000AA9 RID: 2729
		public StateBinding _throwSpinBinding = new StateBinding(true, "_throwSpin", -1, false, false);

		// Token: 0x04000AAA RID: 2730
		public StateBinding _volatileBinding = new StateBinding("_volatile", -1, false, false);

		// Token: 0x04000AAB RID: 2731
		public StateBinding _addOffsetXBinding = new StateBinding("_addOffsetX", -1, false, false);

		// Token: 0x04000AAC RID: 2732
		public StateBinding _addOffsetYBinding = new StateBinding("_addOffsetY", -1, false, false);

		// Token: 0x04000AAD RID: 2733
		public float _swing;

		// Token: 0x04000AAE RID: 2734
		public float _hold;

		// Token: 0x04000AAF RID: 2735
		private bool _drawing;

		// Token: 0x04000AB0 RID: 2736
		public bool _pullBack;

		// Token: 0x04000AB1 RID: 2737
		public bool _jabStance;

		// Token: 0x04000AB2 RID: 2738
		public bool _crouchStance;

		// Token: 0x04000AB3 RID: 2739
		public bool _slamStance;

		// Token: 0x04000AB4 RID: 2740
		public bool _swinging;

		// Token: 0x04000AB5 RID: 2741
		public float _addOffsetX;

		// Token: 0x04000AB6 RID: 2742
		public float _addOffsetY;

		// Token: 0x04000AB7 RID: 2743
		public bool _swingPress;

		// Token: 0x04000AB8 RID: 2744
		public bool _shing;

		// Token: 0x04000AB9 RID: 2745
		public static bool _playedShing;

		// Token: 0x04000ABA RID: 2746
		public bool _atRest = true;

		// Token: 0x04000ABB RID: 2747
		public bool _swung;

		// Token: 0x04000ABC RID: 2748
		public bool _wasLifted;

		// Token: 0x04000ABD RID: 2749
		public float _throwSpin;

		// Token: 0x04000ABE RID: 2750
		public int _framesExisting;

		// Token: 0x04000ABF RID: 2751
		public int _hitWait;

		// Token: 0x04000AC0 RID: 2752
		private SpriteMap _swordSwing;

		// Token: 0x04000AC1 RID: 2753
		private int _unslam;

		// Token: 0x04000AC2 RID: 2754
		private byte blocked;

		// Token: 0x04000AC3 RID: 2755
		public bool _volatile;

		// Token: 0x04000AC4 RID: 2756
		private List<float> _lastAngles = new List<float>();

		// Token: 0x04000AC5 RID: 2757
		private List<Vec2> _lastPositions = new List<Vec2>();
	}
}
