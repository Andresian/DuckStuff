// DuckGame.Duck
// Token: 0x0600022A RID: 554
private void UpdateQuack()
{
	if (!this.dead)
	{
		if (this.inputProfile.Pressed("QUACK", false))
		{
			if (Network.isActive)
			{
				this._netQuack.Play(1f, 1f - Mouse.x / Layer.HUD.camera.width * 2f);
			}
			else
			{
				Hat h = this.GetEquipment(typeof(Hat)) as Hat;
				if (h != null)
				{
					h.Quack(1f, 1f - Mouse.x / Layer.HUD.camera.width * 2f);
				}
				else
				{
					this._netQuack.Play(1f, 1f - Mouse.x / Layer.HUD.camera.width * 2f);
				}
			}
			int num;
			if (base.isServerForObject)
			{
				StatBinding quacks = Global.data.quacks;
				num = quacks.valueInt;
				quacks.valueInt = num + 1;
			}
			ProfileStats stats = this.profile.stats;
			num = stats.quacks;
			stats.quacks = num + 1;
			this.quack = 20;
		}
		if (!this.inputProfile.Down("QUACK"))
		{
			this.quack = Maths.CountDown(this.quack, 1, 0);
		}
		if (this.inputProfile.Released("QUACK"))
		{
			this.quack = 0;
		}
	}
}
