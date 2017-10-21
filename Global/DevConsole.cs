using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
	// Token: 0x020003FA RID: 1018
	public class DevConsole
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001EA1 RID: 7841
		// (set) Token: 0x06001EA2 RID: 7842
		public static DevConsoleCore core
		{
			get
			{
				return DevConsole._core;
			}
			set
			{
				DevConsole._core = value;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001EA3 RID: 7843
		public static bool open
		{
			get
			{
				return DevConsole._core.open;
			}
		}

		// Token: 0x06001EA4 RID: 7844
		public static void SuppressDevConsole()
		{
			DevConsole._oldConsole = DevConsole._enableNetworkDebugging;
			DevConsole._enableNetworkDebugging = false;
		}

		// Token: 0x06001EA5 RID: 7845
		public static void RestoreDevConsole()
		{
			DevConsole._enableNetworkDebugging = DevConsole._oldConsole;
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001EA6 RID: 7846
		// (set) Token: 0x06001EA7 RID: 7847
		public static bool enableNetworkDebugging
		{
			get
			{
				return DevConsole._enableNetworkDebugging;
			}
			set
			{
				DevConsole._enableNetworkDebugging = value;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001EA8 RID: 7848
		// (set) Token: 0x06001EA9 RID: 7849
		public static bool splitScreen
		{
			get
			{
				return DevConsole._core.splitScreen;
			}
			set
			{
				DevConsole._core.splitScreen = value;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001EAA RID: 7850
		// (set) Token: 0x06001EAB RID: 7851
		public static bool rhythmMode
		{
			get
			{
				return DevConsole._core.rhythmMode;
			}
			set
			{
				DevConsole._core.rhythmMode = value;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001EAC RID: 7852
		// (set) Token: 0x06001EAD RID: 7853
		public static bool qwopMode
		{
			get
			{
				return DevConsole._core.qwopMode;
			}
			set
			{
				DevConsole._core.qwopMode = value;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001EAE RID: 7854
		// (set) Token: 0x06001EAF RID: 7855
		public static bool showIslands
		{
			get
			{
				return DevConsole._core.showIslands;
			}
			set
			{
				DevConsole._core.showIslands = value;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001EB0 RID: 7856
		// (set) Token: 0x06001EB1 RID: 7857
		public static bool showCollision
		{
			get
			{
				return DevConsole._core.showCollision;
			}
			set
			{
				DevConsole._core.showCollision = value;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001EB2 RID: 7858
		// (set) Token: 0x06001EB3 RID: 7859
		public static bool shieldMode
		{
			get
			{
				return DevConsole._core.shieldMode;
			}
			set
			{
				DevConsole._core.shieldMode = value;
			}
		}

		// Token: 0x06001EB4 RID: 7860
		public static void Draw()
		{
			if (DevConsole._core.font == null)
			{
				DevConsole._core.font = new BitmapFont("biosFont", 8, -1);
				DevConsole._core.font.scale = new Vec2(2f, 2f);
				DevConsole._core.fancyFont = new FancyBitmapFont("smallFont");
				DevConsole._core.fancyFont.scale = new Vec2(2f, 2f);
			}
			if (DevConsole._core.alpha > 0.01f)
			{
				float num = 256f;
				int width = Graphics.width;
				DevConsole._core.font.alpha = DevConsole._core.alpha;
				DevConsole._core.font.Draw(DevConsole._core.typing, 16f, num + 20f, Color.White, 0.9f, null, false);
				int num2 = DevConsole._core.lines.Count - 1;
				float num3 = 0f;
				int num4 = 0;
				while ((float)num4 < num / 18f && num2 >= 0)
				{
					DCLine dcline = DevConsole._core.lines.ElementAt(num2);
					if (!NetworkDebugger.enabled || dcline.threadIndex == NetworkDebugger.networkDrawingIndex)
					{
						DevConsole._core.font.scale = new Vec2(dcline.scale);
						DevConsole._core.font.Draw(dcline.SectionString() + dcline.line, 16f, num - 20f - num3, dcline.color * 0.8f, 0.9f, null, false);
						num3 += 18f * (dcline.scale * 0.5f);
						DevConsole._core.font.scale = new Vec2(2f);
					}
					num2--;
					num4++;
				}
				if (DevConsole._tray != null)
				{
					DevConsole._tray.alpha = DevConsole._core.alpha;
					DevConsole._tray.scale = new Vec2(4f, 4f);
					DevConsole._tray.depth = 0.75f;
					Graphics.Draw(DevConsole._tray, 0f, 0f);
					DevConsole._scan.alpha = DevConsole._core.alpha;
					DevConsole._scan.scale = new Vec2(2f, 2f);
					DevConsole._scan.depth = 0.95f;
					Graphics.Draw(DevConsole._scan, 0f, 0f);
					DevConsole._core.fancyFont.depth = 0.98f;
					DevConsole._core.fancyFont.alpha = DevConsole._core.alpha;
					string version = DG.version;
					DevConsole._core.fancyFont.Draw(version, new Vec2(1116f, 284f), Colors.SuperDarkBlueGray, 0.98f, false);
				}
			}
		}

		// Token: 0x06001EB5 RID: 7861
		public static Profile ProfileByName(string findName)
		{
			foreach (Profile profile in Profiles.all)
			{
				if (profile.team != null)
				{
					string a = profile.name.ToLower();
					if (findName == "player1" && (profile.persona == Persona.Duck1 || (profile.duck != null && profile.duck.persona == Persona.Duck1)))
					{
						a = findName;
					}
					else if (findName == "player2" && (profile.persona == Persona.Duck2 || (profile.duck != null && profile.duck.persona == Persona.Duck2)))
					{
						a = findName;
					}
					else if (findName == "player3" && (profile.persona == Persona.Duck3 || (profile.duck != null && profile.duck.persona == Persona.Duck3)))
					{
						a = findName;
					}
					else if (findName == "player4" && (profile.persona == Persona.Duck4 || (profile.duck != null && profile.duck.persona == Persona.Duck4)))
					{
						a = findName;
					}
					if (a == findName)
					{
						return profile;
					}
				}
			}
			return null;
		}

		// Token: 0x06001EB6 RID: 7862
		public static void RunCommand(string command)
		{
			if (DG.buildExpired)
			{
				return;
			}
			DevConsole._core.logScores = -1;
			if (command != "")
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				command = command.ToLower(currentCulture);
				bool flag = false;
				ConsoleCommand consoleCommand = new ConsoleCommand(command);
				string text = consoleCommand.NextWord();
				if (text == "spawn")
				{
					flag = true;
					string text2 = consoleCommand.NextWord();
					float x = 0f;
					float y = 0f;
					try
					{
						x = Change.ToSingle(consoleCommand.NextWord());
						y = Change.ToSingle(consoleCommand.NextWord());
					}
					catch
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Parameters in wrong format.",
							color = Color.Red
						});
						return;
					}
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					Type type = null;
					foreach (Type type2 in Editor.ThingTypes)
					{
						if (type2.Name.ToLower(currentCulture) == text2)
						{
							type = type2;
							break;
						}
					}
					if (type == null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "The type " + text2 + " does not exist!",
							color = Color.Red
						});
						return;
					}
					if (!Editor.HasConstructorParameter(type))
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = text2 + " can not be spawned this way.",
							color = Color.Red
						});
						return;
					}
					Thing thing = Editor.CreateThing(type) as PhysicsObject;
					if (thing != null)
					{
						thing.x = x;
						thing.y = y;
						Level.Add(thing);
						SFX.Play("hitBox", 1f, 0f, 0f, false);
					}
				}
				if (text == "modhash")
				{
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = ModLoader._modString,
						color = Color.Red
					});
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = ModLoader.modHash,
						color = Color.Red
					});
					return;
				}
				if (text == "netdebug")
				{
					DevConsole._enableNetworkDebugging = !DevConsole._enableNetworkDebugging;
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = "Network Debugging Enabled",
						color = Color.Green
					});
					return;
				}
				if (text == "record")
				{
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = "Recording Started",
						color = Color.Green
					});
					Recorder.globalRecording = new FileRecording();
					MonoMain.StartRecording(consoleCommand.NextWord().ToLower(currentCulture));
					return;
				}
				if (text == "close")
				{
					DevConsole._core.open = !DevConsole._core.open;
				}
				if (text == "stop")
				{
					if (Recorder.globalRecording != null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Recording Stopped",
							color = Color.Green
						});
						MonoMain.StopRecording();
					}
					return;
				}
				if (text == "playback")
				{
					flag = true;
					string text3 = consoleCommand.NextWord().ToLower(currentCulture);
					if (text3 == "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Parameters in wrong format.",
							color = Color.Red
						});
						return;
					}
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					if (!File.Exists(text3 + ".vid"))
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Could not find recording called " + text3 + "!",
							color = Color.Red
						});
						return;
					}
					Recorder.globalRecording = new FileRecording();
					Recorder.globalRecording.fileName = text3;
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = text3 + " set for playback.",
						color = Color.Green
					});
					MonoMain.StartPlayback();
				}
				if (text == "level")
				{
					flag = true;
					string text4 = consoleCommand.NextWord().ToLower(currentCulture);
					if (text4 == "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Parameters in wrong format.",
							color = Color.Red
						});
						return;
					}
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					bool flag2 = false;
					string str = "deathmatch/" + text4;
					LevelData levelData = DuckFile.LoadLevel(Content.path + "/levels/" + str + ".lev");
					if (levelData != null)
					{
						Level.current = new GameLevel(levelData.metaData.guid, 0, false, false);
						flag2 = true;
					}
					if (!flag2)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "The level \"" + text4 + "\" does not exist!",
							color = Color.Red
						});
						return;
					}
				}
				if (text == "team")
				{
					flag = true;
					string text5 = consoleCommand.NextWord();
					Profile profile = DevConsole.ProfileByName(text5);
					if (profile == null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "No profile named " + text5 + ".",
							color = Color.Red
						});
						return;
					}
					string text6 = consoleCommand.NextWord();
					if (text6 == "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Parameters in wrong format.",
							color = Color.Red
						});
						return;
					}
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					text6 = text6.ToLower();
					bool flag3 = false;
					foreach (Team team in Teams.all)
					{
						if (team.name.ToLower() == text6)
						{
							flag3 = true;
							profile.team = team;
							break;
						}
					}
					if (!flag3)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "No team named " + text6 + ".",
							color = Color.Red
						});
						return;
					}
				}
				if (text == "give")
				{
					flag = true;
					string text7 = consoleCommand.NextWord();
					Profile profile2 = DevConsole.ProfileByName(text7);
					if (profile2 == null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "No profile named " + text7 + ".",
							color = Color.Red
						});
						return;
					}
					if (profile2.duck == null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = text7 + " is not in the game!",
							color = Color.Red
						});
						return;
					}
					string text8 = consoleCommand.NextWord();
					if (text8 == "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Parameters in wrong format.",
							color = Color.Red
						});
						return;
					}
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					Type type3 = null;
					foreach (Type type4 in Editor.ThingTypes)
					{
						if (type4.Name.ToLower(currentCulture) == text8)
						{
							type3 = type4;
							break;
						}
					}
					if (type3 == null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "The type " + text8 + " does not exist!",
							color = Color.Red
						});
						return;
					}
					if (!Editor.HasConstructorParameter(type3))
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = text8 + " can not be spawned this way.",
							color = Color.Red
						});
						return;
					}
					Holdable holdable = Editor.CreateThing(type3) as Holdable;
					if (holdable == null)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = text8 + " can not be held.",
							color = Color.Red
						});
						return;
					}
					Level.Add(holdable);
					profile2.duck.GiveHoldable(holdable);
					SFX.Play("hitBox", 1f, 0f, 0f, false);
				}
				if (text == "call")
				{
					flag = true;
					string text9 = consoleCommand.NextWord();
					bool flag4 = false;
					foreach (Profile profile3 in Profiles.all)
					{
						if (profile3.name.ToLower(currentCulture) == text9)
						{
							if (profile3.duck == null)
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = text9 + " is not in the game!",
									color = Color.Red
								});
								return;
							}
							flag4 = true;
							string text10 = consoleCommand.NextWord();
							if (text10 == "")
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = "Parameters in wrong format.",
									color = Color.Red
								});
								return;
							}
							if (consoleCommand.NextWord() != "")
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = "Too many parameters!",
									color = Color.Red
								});
								return;
							}
							MethodInfo[] methods = typeof(Duck).GetMethods();
							bool flag5 = false;
							foreach (MethodInfo methodInfo in methods)
							{
								if (methodInfo.Name.ToLower(currentCulture) == text10)
								{
									flag5 = true;
									if (methodInfo.GetParameters().Count<ParameterInfo>() > 0)
									{
										DevConsole._core.lines.Enqueue(new DCLine
										{
											line = "You can only call functions with no parameters.",
											color = Color.Red
										});
										return;
									}
									try
									{
										methodInfo.Invoke(profile3.duck, null);
									}
									catch
									{
										DevConsole._core.lines.Enqueue(new DCLine
										{
											line = "The function threw an exception.",
											color = Color.Red
										});
										return;
									}
								}
							}
							if (!flag5)
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = "Duck has no function called " + text10 + ".",
									color = Color.Red
								});
								return;
							}
						}
					}
					if (!flag4)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "No profile named " + text9 + ".",
							color = Color.Red
						});
						return;
					}
				}
				if (text == "set")
				{
					flag = true;
					string text11 = consoleCommand.NextWord();
					bool flag6 = false;
					foreach (Profile profile4 in Profiles.all)
					{
						if (profile4.name.ToLower(currentCulture) == text11)
						{
							if (profile4.duck == null)
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = text11 + " is not in the game!",
									color = Color.Red
								});
								return;
							}
							flag6 = true;
							string text12 = consoleCommand.NextWord();
							if (text12 == "")
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = "Parameters in wrong format.",
									color = Color.Red
								});
								return;
							}
							Type typeFromHandle = typeof(Duck);
							PropertyInfo[] properties = typeFromHandle.GetProperties();
							bool flag7 = false;
							foreach (PropertyInfo propertyInfo in properties)
							{
								if (propertyInfo.Name.ToLower(currentCulture) == text12)
								{
									flag7 = true;
									if (propertyInfo.PropertyType == typeof(float))
									{
										float num = 0f;
										try
										{
											num = Change.ToSingle(consoleCommand.NextWord());
										}
										catch
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Parameters in wrong format.",
												color = Color.Red
											});
											return;
										}
										if (consoleCommand.NextWord() != "")
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Too many parameters!",
												color = Color.Red
											});
											return;
										}
										propertyInfo.SetValue(profile4.duck, num, null);
									}
									if (propertyInfo.PropertyType == typeof(bool))
									{
										bool flag8 = false;
										try
										{
											flag8 = Convert.ToBoolean(consoleCommand.NextWord());
										}
										catch
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Parameters in wrong format.",
												color = Color.Red
											});
											return;
										}
										if (consoleCommand.NextWord() != "")
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Too many parameters!",
												color = Color.Red
											});
											return;
										}
										propertyInfo.SetValue(profile4.duck, flag8, null);
									}
									if (propertyInfo.PropertyType == typeof(int))
									{
										int num2 = 0;
										try
										{
											num2 = Convert.ToInt32(consoleCommand.NextWord());
										}
										catch
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Parameters in wrong format.",
												color = Color.Red
											});
											return;
										}
										if (consoleCommand.NextWord() != "")
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Too many parameters!",
												color = Color.Red
											});
											return;
										}
										propertyInfo.SetValue(profile4.duck, num2, null);
									}
									if (propertyInfo.PropertyType == typeof(Vec2))
									{
										float x2 = 0f;
										float y2 = 0f;
										try
										{
											x2 = Change.ToSingle(consoleCommand.NextWord());
											y2 = Change.ToSingle(consoleCommand.NextWord());
										}
										catch
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Parameters in wrong format.",
												color = Color.Red
											});
											return;
										}
										if (consoleCommand.NextWord() != "")
										{
											DevConsole._core.lines.Enqueue(new DCLine
											{
												line = "Too many parameters!",
												color = Color.Red
											});
											return;
										}
										propertyInfo.SetValue(profile4.duck, new Vec2(x2, y2), null);
									}
								}
							}
							if (!flag7)
							{
								foreach (FieldInfo fieldInfo in typeFromHandle.GetFields())
								{
									if (fieldInfo.Name.ToLower(currentCulture) == text12)
									{
										flag7 = true;
										if (fieldInfo.FieldType == typeof(float))
										{
											float num3 = 0f;
											try
											{
												num3 = Change.ToSingle(consoleCommand.NextWord());
											}
											catch
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Parameters in wrong format.",
													color = Color.Red
												});
												return;
											}
											if (consoleCommand.NextWord() != "")
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Too many parameters!",
													color = Color.Red
												});
												return;
											}
											fieldInfo.SetValue(profile4.duck, num3);
										}
										if (fieldInfo.FieldType == typeof(bool))
										{
											bool flag9 = false;
											try
											{
												flag9 = Convert.ToBoolean(consoleCommand.NextWord());
											}
											catch
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Parameters in wrong format.",
													color = Color.Red
												});
												return;
											}
											if (consoleCommand.NextWord() != "")
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Too many parameters!",
													color = Color.Red
												});
												return;
											}
											fieldInfo.SetValue(profile4.duck, flag9);
										}
										if (fieldInfo.FieldType == typeof(int))
										{
											int num4 = 0;
											try
											{
												num4 = Convert.ToInt32(consoleCommand.NextWord());
											}
											catch
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Parameters in wrong format.",
													color = Color.Red
												});
												return;
											}
											if (consoleCommand.NextWord() != "")
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Too many parameters!",
													color = Color.Red
												});
												return;
											}
											fieldInfo.SetValue(profile4.duck, num4);
										}
										if (fieldInfo.FieldType == typeof(Vec2))
										{
											float x3 = 0f;
											float y3 = 0f;
											try
											{
												x3 = Change.ToSingle(consoleCommand.NextWord());
												y3 = Change.ToSingle(consoleCommand.NextWord());
											}
											catch
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Parameters in wrong format.",
													color = Color.Red
												});
												return;
											}
											if (consoleCommand.NextWord() != "")
											{
												DevConsole._core.lines.Enqueue(new DCLine
												{
													line = "Too many parameters!",
													color = Color.Red
												});
												return;
											}
											fieldInfo.SetValue(profile4.duck, new Vec2(x3, y3));
										}
									}
								}
								if (!flag7)
								{
									DevConsole._core.lines.Enqueue(new DCLine
									{
										line = "Duck has no variable called " + text12 + ".",
										color = Color.Red
									});
									return;
								}
							}
						}
					}
					if (!flag6)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "No profile named " + text11 + ".",
							color = Color.Red
						});
						return;
					}
				}
				if (text == "kill")
				{
					flag = true;
					string text13 = consoleCommand.NextWord();
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					bool flag10 = false;
					foreach (Profile profile5 in Profiles.all)
					{
						if (profile5.name.ToLower(currentCulture) == text13)
						{
							if (profile5.duck == null)
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = text13 + " is not in the game!",
									color = Color.Red
								});
								return;
							}
							profile5.duck.Kill(new DTIncinerate(null));
							flag10 = true;
						}
					}
					if (!flag10)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "No profile named " + text13 + ".",
							color = Color.Red
						});
						return;
					}
				}
				if (text == "kick")
				{
					flag = true;
					string text14 = consoleCommand.NextWord();
					foreach (Profile profile6 in Profiles.all)
					{
						if (profile6.name.ToLower(currentCulture) == text14)
						{
							if (profile6.duck == null)
							{
								DevConsole._core.lines.Enqueue(new DCLine
								{
									line = text14 + " is not in the game!",
									color = Color.Red
								});
								return;
							}
							DevConsole.Kicka(profile6);
						}
					}
				}
				if (text == "globalscores")
				{
					flag = true;
					using (List<Profile>.Enumerator enumerator4 = Profiles.active.GetEnumerator())
					{
						if (enumerator4.MoveNext())
						{
							Profile profile7 = enumerator4.Current;
							DevConsole._core.lines.Enqueue(new DCLine
							{
								line = profile7.name + ": " + profile7.stats.CalculateProfileScore(false).ToString("0.000"),
								color = Color.Red
							});
						}
					}
				}
				if (text == "scorelog")
				{
					flag = true;
					string text15 = consoleCommand.NextWord();
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					if (text15 == "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You need to provide a player number.",
							color = Color.Red
						});
						return;
					}
					int logScores = 0;
					try
					{
						logScores = Convert.ToInt32(text15);
					}
					catch
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Parameters in wrong format.",
							color = Color.Red
						});
						return;
					}
					DevConsole._core.logScores = logScores;
				}
				if (text == "toggle")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					flag = true;
					string a = consoleCommand.NextWord();
					if (consoleCommand.NextWord() != "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "Too many parameters!",
							color = Color.Red
						});
						return;
					}
					if (a == "")
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You need to provide a layer to toggle.",
							color = Color.Red
						});
						return;
					}
					if (a == "background")
					{
						Layer.Background.visible = !Layer.Background.visible;
					}
					else if (a == "parallax")
					{
						Layer.Parallax.visible = !Layer.Parallax.visible;
					}
					else if (a == "foreground")
					{
						Layer.Foreground.visible = !Layer.Foreground.visible;
					}
					else if (a == "game")
					{
						Layer.Game.visible = !Layer.Game.visible;
					}
					else if (a == "HUD")
					{
						Layer.HUD.visible = !Layer.HUD.visible;
					}
				}
				if (text == "splitscreen")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					flag = true;
					DevConsole._core.splitScreen = !DevConsole._core.splitScreen;
				}
				if (text == "clearmainprofile")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = "Your main account has been R U I N E D !",
						color = Color.Red
					});
					Profile profile8 = new Profile(Profiles.experienceProfile.steamID.ToString(), null, null, null, false, Profiles.experienceProfile.steamID.ToString());
					profile8.steamID = Profiles.experienceProfile.steamID;
					Profiles.Remove(Profiles.experienceProfile);
					Profiles.Add(profile8);
					flag = true;
				}
				if (text == "rhythmmode")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					flag = true;
					if (!DevConsole._core.rhythmMode)
					{
						Music.Stop();
					}
					DevConsole._core.rhythmMode = !DevConsole._core.rhythmMode;
					if (DevConsole._core.rhythmMode)
					{
						Music.Play(Music.RandomTrack("InGame", ""), true, 0f);
					}
				}
				if (text == "fancymode")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					DevConsole.fancyMode = !DevConsole.fancyMode;
					flag = true;
				}
				if (text == "shieldmode")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					DevConsole.shieldMode = !DevConsole.shieldMode;
					flag = true;
				}
				if (text == "qwopmode")
				{
					if (Network.isActive || Level.current is ChallengeLevel)
					{
						DevConsole._core.lines.Enqueue(new DCLine
						{
							line = "You can't do that here!",
							color = Color.Red
						});
						return;
					}
					flag = true;
					DevConsole._core.qwopMode = !DevConsole._core.qwopMode;
				}
				if (text == "showislands")
				{
					flag = true;
					DevConsole._core.showIslands = !DevConsole._core.showIslands;
				}
				if (text == "showcollision")
				{
					flag = true;
					DevConsole._core.showCollision = !DevConsole._core.showCollision;
				}
				if (flag)
				{
					DevConsole._core.lines.Enqueue(new DCLine
					{
						line = command,
						color = Color.White
					});
					return;
				}
				DevConsole._core.lines.Enqueue(new DCLine
				{
					line = text + " is not a valid command!",
					color = Color.Red
				});
			}
		}

		// Token: 0x06001EB7 RID: 7863
		public static void Log(string text, Color c, float scale = 2f, int index = -1)
		{
			DCLine item = new DCLine
			{
				line = text,
				color = c,
				scale = scale,
				threadIndex = ((index < 0) ? NetworkDebugger.networkDrawingIndex : index),
				timestamp = DateTime.Now
			};
			List<DCLine> pendingLines;
			if (NetworkDebugger.enabled)
			{
				pendingLines = DevConsole.debuggerLines;
				lock (pendingLines)
				{
					DevConsole.debuggerLines.Add(item);
					return;
				}
			}
			pendingLines = DevConsole._core.pendingLines;
			lock (pendingLines)
			{
				DevConsole._core.pendingLines.Add(item);
			}
		}

		// Token: 0x06001EB8 RID: 7864
		public static void Log(DCSection section, string text, int netIndex = -1)
		{
			DevConsole.Log(section, Verbosity.Normal, text, netIndex);
		}

		// Token: 0x06001EB9 RID: 7865
		public static void Log(DCSection section, Verbosity verbose, string text, int netIndex = -1)
		{
			DCLine item = new DCLine
			{
				line = text,
				section = section,
				verbosity = verbose,
				color = Color.White,
				scale = 2f,
				threadIndex = ((netIndex < 0) ? NetworkDebugger.networkDrawingIndex : netIndex),
				timestamp = DateTime.Now
			};
			List<DCLine> pendingLines;
			if (NetworkDebugger.enabled)
			{
				pendingLines = DevConsole.debuggerLines;
				lock (pendingLines)
				{
					DevConsole.debuggerLines.Add(item);
					return;
				}
			}
			pendingLines = DevConsole._core.pendingLines;
			lock (pendingLines)
			{
				DevConsole._core.pendingLines.Add(item);
			}
		}

		// Token: 0x06001EBA RID: 7866
		public static void Chart(string chart, string section, double x, double y, Color c)
		{
			List<DCChartValue> pendingChartValues = DevConsole._core.pendingChartValues;
			lock (pendingChartValues)
			{
				DevConsole._core.pendingChartValues.Add(new DCChartValue
				{
					chart = chart,
					section = section,
					x = x,
					y = y,
					color = c,
					threadIndex = NetworkDebugger.networkDrawingIndex
				});
			}
		}

		// Token: 0x06001EBB RID: 7867
		public static void UpdateGraph(int index, NetGraph target)
		{
		}

		// Token: 0x06001EBC RID: 7868
		public static void Update()
		{
			if (DevConsole._core.logScores >= 0)
			{
				DevConsole._core.lines.Clear();
				int num = 0;
				foreach (Profile profile in Profiles.active)
				{
					if (num == DevConsole._core.logScores)
					{
						float num2 = profile.endOfRoundStats.CalculateProfileScore(false);
						float num3 = (float)profile.endOfRoundStats.GetProfileScore();
						DevConsole._core.pendingLines.Clear();
						DevConsole.Log(string.Concat(new string[]
						{
							profile.name,
							": ",
							num2.ToString("0.000"),
							"   Cool: ",
							num3.ToString()
						}), Color.Green, 2f, -1);
						profile.endOfRoundStats.CalculateProfileScore(true);
					}
					num++;
				}
			}
			List<DCLine> pendingLines = DevConsole._core.pendingLines;
			lock (pendingLines)
			{
				List<DCLine> pendingLines2 = DevConsole._core.pendingLines;
				DevConsole._core.pendingLines = new List<DCLine>();
				foreach (DCLine item in pendingLines2)
				{
					DevConsole._core.lines.Enqueue(item);
				}
			}
			if (Keyboard.Pressed(Keys.OemTilde, false) && !DuckNetwork.core.enteringText)
			{
				if (DevConsole._tray == null)
				{
					DevConsole._tray = new Sprite("devTray", 0f, 0f);
					DevConsole._scan = new Sprite("devScan", 0f, 0f);
				}
				DevConsole._core.open = !DevConsole._core.open;
				if (DevConsole._core.open)
				{
					Keyboard.keyString = DevConsole._core.typing;
				}
			}
			DevConsole._core.alpha = Maths.LerpTowards(DevConsole._core.alpha, DevConsole._core.open ? 1f : 0f, 0.05f);
			if (DevConsole._core.open)
			{
				DevConsole._core.typing = Keyboard.keyString;
				if (Keyboard.Pressed(Keys.Enter, false))
				{
					DevConsole.RunCommand(DevConsole._core.typing);
					DevConsole._core.lastLine = DevConsole._core.typing;
					Keyboard.keyString = "";
				}
				if (Keyboard.Pressed(Keys.Up, false))
				{
					Keyboard.keyString = DevConsole._core.lastLine;
				}
			}
		}

		// Token: 0x06002511 RID: 9489
		public static void Kicka(Profile p)
		{
			if (p.slotType == SlotType.Local)
			{
				DuckNetwork.SendToEveryone(new NMClientDisconnect(DG.localID.ToString(), p.networkIndex));
				DuckNetwork.ResetProfile(p);
				p.team = null;
				return;
			}
			if (p != null && p.connection != null && p.connection != DuckNetwork.localConnection)
			{
				SFX.Play("little_punch", 1f, 0f, 0f, false);
				Send.Message(new NMKick(), p.connection);
				p.networkStatus = DuckNetStatus.Kicking;
			}
		}

		// Token: 0x04001DB8 RID: 7608
		public static bool fancyMode = false;

		// Token: 0x04001DB9 RID: 7609
		private static DevConsoleCore _core = new DevConsoleCore();

		// Token: 0x04001DBA RID: 7610
		private static bool _enableNetworkDebugging = false;

		// Token: 0x04001DBB RID: 7611
		private static bool _oldConsole;

		// Token: 0x04001DBC RID: 7612
		public static bool fuckUpPacketOrder = false;

		// Token: 0x04001DBD RID: 7613
		public static List<DCLine> debuggerLines = new List<DCLine>();

		// Token: 0x04001DBE RID: 7614
		public static Sprite _tray;

		// Token: 0x04001DBF RID: 7615
		public static Sprite _scan;
	}
}
