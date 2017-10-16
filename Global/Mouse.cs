using System;
using Microsoft.Xna.Framework.Input;

namespace DuckGame
{
	// Token: 0x0200049A RID: 1178
	public class Mouse : InputDevice
	{
		// Token: 0x060020D2 RID: 8402 RVA: 0x00139D20 File Offset: 0x00137F20
		public override void Update()
		{
			Mouse._mouseStatePrev = Mouse._mouseState;
			Mouse._mouseState = Mouse.GetState();
			Vec3 mousePos = new Vec3((float)Mouse._mouseState.X, (float)Mouse._mouseState.Y, 0f);
			Mouse._mouseScreenPos = new Vec2(mousePos.x / (float)Graphics.width * Layer.HUD.camera.width, mousePos.y / (float)Graphics.height * Layer.HUD.camera.height);
			Mouse._mouseScreenPos.x = (float)((int)Mouse._mouseScreenPos.x);
			Mouse._mouseScreenPos.y = (float)((int)Mouse._mouseScreenPos.y);
			Mouse._mousePos = new Vec2((float)Mouse._mouseState.X, (float)Mouse._mouseState.Y);
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060020D3 RID: 8403 RVA: 0x00139DF8 File Offset: 0x00137FF8
		public static InputState left
		{
			get
			{
				if (Mouse._mouseState.LeftButton == ButtonState.Pressed && Mouse._mouseStatePrev.LeftButton == ButtonState.Released)
				{
					return InputState.Pressed;
				}
				if (Mouse._mouseState.LeftButton == ButtonState.Pressed && Mouse._mouseStatePrev.LeftButton == ButtonState.Pressed)
				{
					return InputState.Down;
				}
				if (Mouse._mouseState.LeftButton == ButtonState.Released && Mouse._mouseStatePrev.LeftButton == ButtonState.Pressed)
				{
					return InputState.Released;
				}
				return InputState.None;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x00139E58 File Offset: 0x00138058
		public static InputState middle
		{
			get
			{
				if (Mouse._mouseState.MiddleButton == ButtonState.Pressed && Mouse._mouseStatePrev.MiddleButton == ButtonState.Released)
				{
					return InputState.Pressed;
				}
				if (Mouse._mouseState.MiddleButton == ButtonState.Pressed && Mouse._mouseStatePrev.MiddleButton == ButtonState.Pressed)
				{
					return InputState.Down;
				}
				if (Mouse._mouseState.MiddleButton == ButtonState.Released && Mouse._mouseStatePrev.MiddleButton == ButtonState.Pressed)
				{
					return InputState.Released;
				}
				return InputState.None;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x00139EB8 File Offset: 0x001380B8
		public static InputState right
		{
			get
			{
				if (Mouse._mouseState.RightButton == ButtonState.Pressed && Mouse._mouseStatePrev.RightButton == ButtonState.Released)
				{
					return InputState.Pressed;
				}
				if (Mouse._mouseState.RightButton == ButtonState.Pressed && Mouse._mouseStatePrev.RightButton == ButtonState.Pressed)
				{
					return InputState.Down;
				}
				if (Mouse._mouseState.RightButton == ButtonState.Released && Mouse._mouseStatePrev.RightButton == ButtonState.Pressed)
				{
					return InputState.Released;
				}
				return InputState.None;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060020D6 RID: 8406 RVA: 0x000033E5 File Offset: 0x000015E5
		public static bool available
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060020D7 RID: 8407 RVA: 0x00016E52 File Offset: 0x00015052
		public static float scroll
		{
			get
			{
				return (float)(Mouse._mouseStatePrev.ScrollWheelValue - Mouse._mouseState.ScrollWheelValue);
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x00016E6A File Offset: 0x0001506A
		public static float x
		{
			get
			{
				return Mouse._mouseScreenPos.x;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060020D9 RID: 8409 RVA: 0x00016E76 File Offset: 0x00015076
		public static float y
		{
			get
			{
				return Mouse._mouseScreenPos.y;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x00016E82 File Offset: 0x00015082
		public static float xScreen
		{
			get
			{
				return Mouse.positionScreen.x;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060020DB RID: 8411 RVA: 0x00016E8E File Offset: 0x0001508E
		public static float yScreen
		{
			get
			{
				return Mouse.positionScreen.y;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x00016E9A File Offset: 0x0001509A
		public static float xConsole
		{
			get
			{
				return Mouse.positionConsole.x;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060020DD RID: 8413 RVA: 0x00016EA6 File Offset: 0x000150A6
		public static float yConsole
		{
			get
			{
				return Mouse.positionConsole.y;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x00016EB2 File Offset: 0x000150B2
		public static Vec2 position
		{
			get
			{
				return new Vec2(Mouse.x, Mouse.y);
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060020DF RID: 8415 RVA: 0x00016EC3 File Offset: 0x000150C3
		public static Vec2 mousePos
		{
			get
			{
				return Mouse._mousePos;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x00016ECA File Offset: 0x000150CA
		public static Vec2 positionScreen
		{
			get
			{
				return Level.current.camera.transformScreenVector(Mouse._mousePos);
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060020E1 RID: 8417 RVA: 0x00016EE0 File Offset: 0x000150E0
		public static Vec2 positionConsole
		{
			get
			{
				return Layer.Console.camera.transformScreenVector(Mouse._mousePos);
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x0000AE1C File Offset: 0x0000901C
		public Mouse() : base(0)
		{
		}

		// Token: 0x04001F71 RID: 8049
		private static Vec2 _mousePos;

		// Token: 0x04001F72 RID: 8050
		private static Vec2 _mouseScreenPos;

		// Token: 0x04001F73 RID: 8051
		private static MouseState _mouseState;

		// Token: 0x04001F74 RID: 8052
		private static MouseState _mouseStatePrev;
	}
}
