using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
	// Token: 0x020001E4 RID: 484
	public class DuckNetwork
	{
		// Token: 0x06001032 RID: 4146 RVA: 0x0009851C File Offset: 0x0009671C
		public static OnlineLevel GetLevel(int lev)
		{
			foreach (OnlineLevel i in DuckNetwork._levels)
			{
				if (i.num == lev)
				{
					return i;
				}
			}
			return DuckNetwork._levels.Last<OnlineLevel>();
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00098580 File Offset: 0x00096780
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x0009858C File Offset: 0x0009678C
		public static Dictionary<string, XPPair> _xpEarned
		{
			get
			{
				return DuckNetwork._core._xpEarned;
			}
			set
			{
				DuckNetwork._core._xpEarned = value;
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0009859C File Offset: 0x0009679C
		public static void GiveXP(string category, int num, int xp, int type = 4, int firstCap = 9999999, int secondCap = 9999999, int finalCap = 9999999)
		{
			if (Profiles.experienceProfile == null)
			{
				return;
			}
			if (!DuckNetwork._xpEarned.ContainsKey(category))
			{
				DuckNetwork._xpEarned[category] = new XPPair();
			}
			DuckNetwork._xpEarned[category].num += num;
			if (DuckNetwork._xpEarned[category].xp > secondCap)
			{
				DuckNetwork._xpEarned[category].xp += xp / 4;
			}
			else if (DuckNetwork._xpEarned[category].xp > firstCap)
			{
				DuckNetwork._xpEarned[category].xp += xp / 2;
			}
			else
			{
				DuckNetwork._xpEarned[category].xp += xp;
			}
			if (DuckNetwork._xpEarned[category].xp > finalCap)
			{
				DuckNetwork._xpEarned[category].xp = finalCap;
			}
			DuckNetwork._xpEarned[category].type = type;
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x00098695 File Offset: 0x00096895
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x000986A1 File Offset: 0x000968A1
		private static UIMenu _xpMenu
		{
			get
			{
				return DuckNetwork._core.xpMenu;
			}
			set
			{
				DuckNetwork._core.xpMenu = value;
			}
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x000986B0 File Offset: 0x000968B0
		public static bool ShowUserXPGain()
		{
			if (DuckNetwork._xpEarned.Count > 0)
			{
				DuckNetwork._xpMenu = new UILevelBox("@LWING@PAUSE@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, -1f, "@DPAD@MOVE  @SELECT@SELECT");
				MonoMain.pauseMenu = DuckNetwork._xpMenu;
				return true;
			}
			return false;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00098720 File Offset: 0x00096920
		public static KeyValuePair<string, XPPair> TakeXPStat()
		{
			if (DuckNetwork._xpEarned.Count == 0)
			{
				return default(KeyValuePair<string, XPPair>);
			}
			KeyValuePair<string, XPPair> val = DuckNetwork._xpEarned.ElementAt(0);
			DuckNetwork._xpEarned.Remove(val.Key);
			return val;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00098764 File Offset: 0x00096964
		public static int GetTotalXPEarned()
		{
			int total = 0;
			foreach (KeyValuePair<string, XPPair> pair in DuckNetwork._xpEarned)
			{
				total += pair.Value.xp;
			}
			return total;
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x000987C4 File Offset: 0x000969C4
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x000987CB File Offset: 0x000969CB
		public static DuckNetworkCore core
		{
			get
			{
				return DuckNetwork._core;
			}
			set
			{
				DuckNetwork._core = value;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x000987D3 File Offset: 0x000969D3
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x000987DF File Offset: 0x000969DF
		public static NetworkConnection localConnection
		{
			get
			{
				return DuckNetwork._core.localConnection;
			}
			set
			{
				DuckNetwork._core.localConnection = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x000987EC File Offset: 0x000969EC
		public static bool active
		{
			get
			{
				return DuckNetwork._core.status != DuckNetStatus.Disconnected;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x000987FE File Offset: 0x000969FE
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x0009880A File Offset: 0x00096A0A
		public static byte levelIndex
		{
			get
			{
				return DuckNetwork._core.levelIndex;
			}
			set
			{
				DuckNetwork._core.levelIndex = value;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00098817 File Offset: 0x00096A17
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00098823 File Offset: 0x00096A23
		public static MemoryStream compressedLevelData
		{
			get
			{
				return DuckNetwork._core.compressedLevelData;
			}
			set
			{
				DuckNetwork._core.compressedLevelData = value;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00098830 File Offset: 0x00096A30
		public static List<Profile> profiles
		{
			get
			{
				return DuckNetwork._core.profiles;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0009883C File Offset: 0x00096A3C
		public static int localDuckIndex
		{
			get
			{
				return DuckNetwork._core.localDuckIndex;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x00098848 File Offset: 0x00096A48
		public static int hostDuckIndex
		{
			get
			{
				return DuckNetwork._core.hostDuckIndex;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x00098854 File Offset: 0x00096A54
		public static DuckNetStatus status
		{
			get
			{
				return DuckNetwork._core.status;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x00098860 File Offset: 0x00096A60
		public static int randomID
		{
			get
			{
				return DuckNetwork._core.randomID;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0009886C File Offset: 0x00096A6C
		public static DuckNetErrorInfo error
		{
			get
			{
				return DuckNetwork._core.error;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x00098878 File Offset: 0x00096A78
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x00098884 File Offset: 0x00096A84
		public static bool inGame
		{
			get
			{
				return DuckNetwork._core.inGame;
			}
			set
			{
				DuckNetwork._core.inGame = value;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x00098891 File Offset: 0x00096A91
		public static bool enteringText
		{
			get
			{
				return DuckNetwork._core.enteringText;
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0009889D File Offset: 0x00096A9D
		public static void RaiseError(DuckNetErrorInfo e)
		{
			if (e != null)
			{
				DevConsole.Log(DCSection.DuckNet, e.message, -1);
				if (DuckNetwork._core.error == null)
				{
					DuckNetwork._core.error = e;
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x000988C6 File Offset: 0x00096AC6
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x000988D2 File Offset: 0x00096AD2
		private static UIComponent _ducknetUIGroup
		{
			get
			{
				return DuckNetwork._core.ducknetUIGroup;
			}
			set
			{
				DuckNetwork._core.ducknetUIGroup = value;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x000988DF File Offset: 0x00096ADF
		public static UIComponent duckNetUIGroup
		{
			get
			{
				return DuckNetwork._ducknetUIGroup;
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000988E6 File Offset: 0x00096AE6
		public static void Initialize()
		{
			DuckNetwork._chatFont = new FancyBitmapFont("smallFontChat");
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x000988F8 File Offset: 0x00096AF8
		public static void Kick(Profile p)
		{
			if (p.slotType == SlotType.Local)
			{
				DuckNetwork.SendToEveryone(new NMClientDisconnect(DG.localID.ToString(), p.networkIndex));
				DuckNetwork.ResetProfile(p);
				p.team = null;
				return;
			}
			if (Network.isServer && p != null && p.connection != null && p.connection != DuckNetwork.localConnection)
			{
				SFX.Play("little_punch", 1f, 0f, 0f, false);
				Send.Message(new NMKick(), p.connection);
				p.networkStatus = DuckNetStatus.Kicking;
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0009898C File Offset: 0x00096B8C
		public static void ChangeSlotSettings()
		{
			bool allFriends = true;
			bool allPrivate = true;
			DuckNetwork.numSlots = 0;
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.connection != DuckNetwork.localConnection)
				{
					if (p.slotType != SlotType.Friend)
					{
						allFriends = false;
					}
					if (p.slotType != SlotType.Invite)
					{
						allPrivate = false;
					}
					if (p.slotType != SlotType.Closed)
					{
						DuckNetwork.numSlots++;
					}
				}
				else if (p.slotType != SlotType.Closed)
				{
					DuckNetwork.numSlots++;
				}
			}
			if (Network.isServer)
			{
				if (Steam.lobby != null)
				{
					if (allFriends)
					{
						Steam.lobby.type = SteamLobbyType.FriendsOnly;
					}
					else if (allPrivate)
					{
						Steam.lobby.type = SteamLobbyType.Private;
					}
					else
					{
						Steam.lobby.type = SteamLobbyType.Public;
					}
					Steam.lobby.maxMembers = 32;
					Steam.lobby.SetLobbyData("numSlots", DuckNetwork.numSlots.ToString());
				}
				Send.Message(new NMChangeSlots((byte)DuckNetwork.profiles[0].slotType, (byte)DuckNetwork.profiles[1].slotType, (byte)DuckNetwork.profiles[2].slotType, (byte)DuckNetwork.profiles[3].slotType));
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00098AE0 File Offset: 0x00096CE0
		public static void KickedPlayer()
		{
			if (DuckNetwork.kickContext != null)
			{
				DuckNetwork.Kick(DuckNetwork.kickContext);
				DuckNetwork.kickContext = null;
			}
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00098AF9 File Offset: 0x00096CF9
		public static void ClosePauseMenu()
		{
			if (Network.isActive && MonoMain.pauseMenu != null)
			{
				MonoMain.pauseMenu.Close();
				MonoMain.pauseMenu = null;
				if (DuckNetwork._ducknetUIGroup != null)
				{
					Level.Remove(DuckNetwork._ducknetUIGroup);
					DuckNetwork._ducknetUIGroup = null;
				}
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00098B30 File Offset: 0x00096D30
		public static void OpenMatchSettingsInfo()
		{
			DuckNetwork._willOpenSettingsInfo = true;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00098B38 File Offset: 0x00096D38
		public static void OpenNoModsWindow(UIMenuActionCloseMenuCallFunction.Function acceptFunction)
		{
			float wide = 320f;
			float high = 180f;
			DuckNetwork._noModsUIGroup = new UIComponent(wide / 2f, high / 2f, 0f, 0f);
			DuckNetwork._noModsMenu = DuckNetwork.CreateNoModsOnlineWindow(acceptFunction);
			DuckNetwork._noModsUIGroup.Add(DuckNetwork._noModsMenu, false);
			DuckNetwork._noModsUIGroup.Close();
			DuckNetwork._noModsUIGroup.Close();
			Level.Add(DuckNetwork._noModsUIGroup);
			DuckNetwork._noModsUIGroup.Update();
			DuckNetwork._noModsUIGroup.Update();
			DuckNetwork._noModsUIGroup.Update();
			DuckNetwork._noModsUIGroup.Open();
			DuckNetwork._noModsMenu.Open();
			MonoMain.pauseMenu = DuckNetwork._noModsUIGroup;
			DuckNetwork._pauseOpen = true;
			SFX.Play("pause", 0.6f, 0f, 0f, false);
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00098C0C File Offset: 0x00096E0C
		private static UIMenu CreateNoModsOnlineWindow(UIMenuActionCloseMenuCallFunction.Function acceptFunction)
		{
			UIMenu matchSettingsInfo = new UIMenu("@LWING@YOU HAVE MODS ENABLED@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, -1f, "@QUACK@BACK", null, false);
			BitmapFont littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
			UIText t = new UIText("YOU WILL |DGRED|NOT|WHITE| BE ABLE TO PLAY", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("ONLINE WITH ANYONE WHO DOES ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("NOT HAVE THE |DGRED|SAME MODS|WHITE|.     ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("WOULD YOU LIKE TO |DGGREEN|DISABLE|WHITE|   ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("MODS AND RESTART THE GAME?  ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			matchSettingsInfo.Add(new UIMenuItem("|DGGREEN|DISABLE MODS AND RESTART", new UIMenuActionCloseMenuCallFunction(DuckNetwork._noModsUIGroup, new UIMenuActionCloseMenuCallFunction.Function(ModLoader.DisableModsAndRestart)), UIAlign.Center, Color.White, false), true);
			matchSettingsInfo.Add(new UIMenuItem("|DGYELLOW|I KNOW WHAT I'M DOING", new UIMenuActionCloseMenuCallFunction(DuckNetwork._noModsUIGroup, acceptFunction), UIAlign.Center, Color.White, false), true);
			matchSettingsInfo.Add(new UIMenuItem("BACK", new UIMenuActionCloseMenu(DuckNetwork._noModsUIGroup), UIAlign.Center, default(Color), true), true);
			matchSettingsInfo.Close();
			return matchSettingsInfo;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00098E1C File Offset: 0x0009701C
		public static UIComponent OpenModsRestartWindow(UIMenu openOnClose)
		{
			float wide = 320f;
			float high = 180f;
			DuckNetwork._restartModsUIGroup = new UIComponent(wide / 2f, high / 2f, 0f, 0f);
			DuckNetwork._restartModsMenu = DuckNetwork.CreateModsRestartWindow(openOnClose);
			DuckNetwork._restartModsUIGroup.Add(DuckNetwork._restartModsMenu, false);
			DuckNetwork._restartModsUIGroup.Close();
			DuckNetwork._restartModsUIGroup.Close();
			Level.Add(DuckNetwork._restartModsUIGroup);
			DuckNetwork._restartModsUIGroup.Update();
			DuckNetwork._restartModsUIGroup.Update();
			DuckNetwork._restartModsUIGroup.Update();
			DuckNetwork._restartModsUIGroup.Open();
			DuckNetwork._restartModsMenu.Open();
			MonoMain.pauseMenu = DuckNetwork._restartModsUIGroup;
			DuckNetwork._pauseOpen = true;
			SFX.Play("pause", 0.6f, 0f, 0f, false);
			return DuckNetwork._restartModsUIGroup;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00098EF4 File Offset: 0x000970F4
		private static UIMenu CreateModsRestartWindow(UIMenu openOnClose)
		{
			UIMenu matchSettingsInfo = new UIMenu("@LWING@MODS CHANGED@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, -1f, "@QUACK@BACK", null, false);
			BitmapFont littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
			UIText t = new UIText("YOU NEED TO RESTART THE GAME", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("FOR CHANGES TO TAKE EFFECT. ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText("DO YOU WANT TO |DGGREEN|RESTART|WHITE| NOW? ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			matchSettingsInfo.Add(new UIMenuItem("|DGGREEN|RESTART", new UIMenuActionCloseMenuCallFunction(DuckNetwork._restartModsUIGroup, new UIMenuActionCloseMenuCallFunction.Function(ModLoader.RestartGame)), UIAlign.Center, Color.White, false), true);
			matchSettingsInfo.Add(new UIMenuItem("|DGYELLOW|CONTINUE", new UIMenuActionOpenMenu(DuckNetwork._restartModsUIGroup, openOnClose), UIAlign.Center, Color.White, false), true);
			matchSettingsInfo.Close();
			return matchSettingsInfo;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00099048 File Offset: 0x00097248
		private static UIMenu CreateMatchSettingsInfoWindow(UIMenu openOnClose = null)
		{
			UIMenu matchSettingsInfo = null;
			if (openOnClose != null)
			{
				matchSettingsInfo = new UIMenu("@LWING@MATCH SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@QUACK@BACK", null, false);
			}
			else
			{
				matchSettingsInfo = new UIMenu("@LWING@NEW SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@QUACK@BACK", null, false);
			}
			BitmapFont littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
			UIText t = new UIText("THE HOST HAS CHANGED", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			if (openOnClose == null)
			{
				matchSettingsInfo.Add(t, true);
				t = new UIText("THE MATCH SETTINGS.", Color.White, UIAlign.Center, 0f, null);
				t.SetFont(littleFont);
				matchSettingsInfo.Add(t, true);
				t = new UIText(" ", Color.White, UIAlign.Center, 0f, null);
				t.SetFont(littleFont);
				matchSettingsInfo.Add(t, true);
			}
			MatchSetting i = TeamSelect2.GetMatchSetting("requiredwins");
			int part1Length = 16;
			int part2Length = 5;
			string textPart = i.name;
			string textPart2 = i.value.ToString();
			while (textPart.Length < part1Length)
			{
				textPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			string text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, Colors.DGGreen, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetMatchSetting("restsevery");
			textPart = i.name;
			textPart2 = i.value.ToString();
			while (textPart.Length < part1Length)
			{
				textPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, Colors.DGGreen, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetMatchSetting("normalmaps");
			textPart = i.name;
			textPart2 = i.value.ToString() + "%";
			if (i.minString != null && i.value is int && (int)i.value == i.min)
			{
				textPart2 = i.minString;
			}
			int endIdx = i.name.LastIndexOf('|');
			string realTextPart = i.name.Substring(endIdx, i.name.Count<char>() - endIdx);
			while (realTextPart.Length < part1Length)
			{
				textPart += " ";
				realTextPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, Colors.DGGreen, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetMatchSetting("custommaps");
			textPart = i.name;
			textPart2 = i.value.ToString() + "%";
			if (i.minString != null && i.value is int && (int)i.value == i.min)
			{
				textPart2 = i.minString;
			}
			endIdx = i.name.LastIndexOf('|');
			realTextPart = i.name.Substring(endIdx, i.name.Count<char>() - endIdx);
			while (realTextPart.Length < part1Length)
			{
				textPart += " ";
				realTextPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, Colors.DGGreen, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			textPart = "Num Custom: ";
			textPart2 = TeamSelect2.customLevels.ToString();
			while (textPart.Length < part1Length)
			{
				textPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (TeamSelect2.customLevels != TeamSelect2.prevCustomLevels)
			{
				t = new UIText(text, (TeamSelect2.customLevels > 0) ? Colors.DGGreen : Colors.DGRed, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			TeamSelect2.prevCustomLevels = TeamSelect2.customLevels;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetMatchSetting("randommaps");
			textPart = i.name;
			textPart2 = i.value.ToString() + "%";
			endIdx = i.name.LastIndexOf('|');
			realTextPart = i.name.Substring(endIdx, i.name.Count<char>() - endIdx);
			while (realTextPart.Length < part1Length)
			{
				textPart += " ";
				realTextPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, Colors.DGGreen, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetMatchSetting("workshopmaps");
			textPart = i.name;
			textPart2 = i.value.ToString() + "%";
			endIdx = i.name.LastIndexOf('|');
			realTextPart = i.name.Substring(endIdx, i.name.Count<char>() - endIdx);
			while (realTextPart.Length < part1Length)
			{
				textPart += " ";
				realTextPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, Colors.DGGreen, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetOnlineSetting("teams");
			textPart = i.name;
			textPart2 = i.value.ToString();
			while (textPart.Length < part1Length)
			{
				textPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, ((bool)i.value) ? Colors.DGGreen : Colors.DGRed, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText(" ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			i = TeamSelect2.GetMatchSetting("wallmode");
			textPart = i.name;
			textPart2 = i.value.ToString();
			while (textPart.Length < part1Length)
			{
				textPart += " ";
			}
			while (textPart2.Length < part2Length)
			{
				textPart2 = " " + textPart2;
			}
			text = textPart + " " + textPart2;
			if (i.value != i.prevValue)
			{
				t = new UIText(text, ((bool)i.value) ? Colors.DGGreen : Colors.DGRed, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			t = new UIText(" ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			foreach (UnlockData dat in Unlocks.GetUnlocks(UnlockType.Modifier))
			{
				if (dat.onlineEnabled)
				{
					text = dat.shortName;
					while (text.Length < 20)
					{
						text += " ";
					}
					if (dat.enabled != dat.prevEnabled || dat.enabled)
					{
						if (dat.enabled)
						{
							text = "@USERONLINE@" + text;
						}
						else
						{
							text = "@USEROFFLINE@" + text;
						}
						if (dat.enabled != dat.prevEnabled)
						{
							t = new UIText(text, dat.enabled ? Colors.DGGreen : Colors.DGRed, UIAlign.Center, 0f, null);
						}
						else
						{
							t = new UIText(text, dat.enabled ? Color.White : Colors.Silver, UIAlign.Center, 0f, null);
						}
						t.SetFont(littleFont);
						matchSettingsInfo.Add(t, true);
					}
					dat.prevEnabled = dat.enabled;
				}
			}
			t = new UIText(" ", Color.White, UIAlign.Center, 0f, null);
			t.SetFont(littleFont);
			matchSettingsInfo.Add(t, true);
			if (openOnClose != null)
			{
				matchSettingsInfo.Add(new UIMenuItem("OK", new UIMenuActionOpenMenu(matchSettingsInfo, openOnClose), UIAlign.Center, Color.White, true), true);
			}
			else
			{
				matchSettingsInfo.Add(new UIMenuItem("OK", new UIMenuActionCloseMenu(DuckNetwork._ducknetUIGroup), UIAlign.Center, Color.White, true), true);
			}
			matchSettingsInfo.Close();
			return matchSettingsInfo;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00099B84 File Offset: 0x00097D84
		private static void DoMatchSettingsInfoOpen()
		{
			float wide = 320f;
			float high = 180f;
			DuckNetwork._ducknetUIGroup = new UIComponent(wide / 2f, high / 2f, 0f, 0f);
			DuckNetwork._matchSettingMenu = DuckNetwork.CreateMatchSettingsInfoWindow(null);
			DuckNetwork._ducknetUIGroup.Add(DuckNetwork._matchSettingMenu, false);
			DuckNetwork._ducknetUIGroup.Close();
			DuckNetwork._ducknetUIGroup.Close();
			Level.Add(DuckNetwork._ducknetUIGroup);
			DuckNetwork._ducknetUIGroup.Update();
			DuckNetwork._ducknetUIGroup.Update();
			DuckNetwork._ducknetUIGroup.Update();
			DuckNetwork._ducknetUIGroup.Open();
			DuckNetwork._matchSettingMenu.Open();
			MonoMain.pauseMenu = DuckNetwork._ducknetUIGroup;
			DuckNetwork._pauseOpen = true;
			SFX.Play("pause", 0.6f, 0f, 0f, false);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00099C58 File Offset: 0x00097E58
		private static void OpenMenu(Profile whoOpen)
		{
			if (DuckNetwork._ducknetUIGroup != null)
			{
				Level.Remove(DuckNetwork._ducknetUIGroup);
			}
			bool canInvite = Level.current is TeamSelect2;
			DuckNetwork._menuOpenProfile = whoOpen;
			float wide = 320f;
			float high = 180f;
			DuckNetwork._ducknetUIGroup = new UIComponent(wide / 2f, high / 2f, 0f, 0f);
			DuckNetwork._ducknetMenu = new UIMenu("@LWING@MULTIPLAYER@RWING@", wide / 2f, high / 2f, 210f, -1f, "@DPAD@MOVE @SELECT@SELECT @QUACK@BACK", null, false);
			if (whoOpen.slotType == SlotType.Local)
			{
				DuckNetwork._confirmMenu = new UIMenu("REALLY BACK OUT?", wide / 2f, high / 2f, 160f, -1f, "@SELECT@SELECT", null, false);
			}
			else
			{
				DuckNetwork._confirmMenu = new UIMenu("REALLY QUIT?", wide / 2f, high / 2f, 160f, -1f, "@SELECT@SELECT", null, false);
			}
			DuckNetwork._confirmKick = new UIMenu("REALLY KICK?", wide / 2f, high / 2f, 160f, -1f, "@SELECT@SELECT", null, false);
			DuckNetwork._optionsMenu = new UIMenu("@WRENCH@OPTIONS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST @QUACK@BACK", null, false);
			DuckNetwork._settingsBeforeOpen = TeamSelect2.GetMatchSettingString();
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.connection != null)
				{
					DuckNetwork._ducknetMenu.Add(new UIConnectionInfo(p, DuckNetwork._ducknetMenu, DuckNetwork._confirmKick), true);
				}
			}
			if (whoOpen.slotType != SlotType.Local)
			{
				DuckNetwork._ducknetMenu.Add(new UIMenuItem("OPTIONS", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, DuckNetwork._optionsMenu), UIAlign.Left, default(Color), false), true);
			}
			if (whoOpen.slotType != SlotType.Local && canInvite && Network.isServer)
			{
				DuckNetwork._slotEditor = new UISlotEditor(DuckNetwork._ducknetMenu, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, -1f);
				DuckNetwork._slotEditor.Close();
				DuckNetwork._ducknetUIGroup.Add(DuckNetwork._slotEditor, false);
				DuckNetwork._ducknetMenu.Add(new UIMenuItem("|DGBLUE|EDIT SLOTS", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, DuckNetwork._slotEditor), UIAlign.Left, default(Color), false), true);
				DuckNetwork._matchSettingMenu = new UIMenu("@LWING@MATCH SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @QUACK@BACK", null, false);
				DuckNetwork._matchModifierMenu = new UIMenu("MODIFIERS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, -1f, "@DPAD@ADJUST  @QUACK@BACK", null, false);
				DuckNetwork._levelSelectMenu = new LevelSelectCompanionMenu(wide / 2f, high / 2f, DuckNetwork._matchSettingMenu);
				foreach (UnlockData dat in Unlocks.GetUnlocks(UnlockType.Modifier))
				{
					if (dat.onlineEnabled)
					{
						if (dat.unlocked)
						{
							DuckNetwork._matchModifierMenu.Add(new UIMenuItemToggle(dat.shortName, null, new FieldBinding(dat, "enabled", 0f, 1f, 0.1f), default(Color), null, null, false, false), true);
						}
						else
						{
							DuckNetwork._matchModifierMenu.Add(new UIMenuItem("@TINYLOCK@LOCKED", null, UIAlign.Center, Color.Red, false), true);
						}
					}
				}
				DuckNetwork._matchModifierMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
				DuckNetwork._matchModifierMenu.Add(new UIMenuItem("OK", new UIMenuActionOpenMenu(DuckNetwork._matchModifierMenu, DuckNetwork._matchSettingMenu), UIAlign.Center, default(Color), true), true);
				DuckNetwork._matchModifierMenu.Close();
				foreach (MatchSetting i in TeamSelect2.matchSettings)
				{
					if ((!(i.id == "workshopmaps") || Network.available) && i.id != "partymode")
					{
						DuckNetwork._matchSettingMenu.AddMatchSetting(i, false, true);
					}
				}
				DuckNetwork._matchSettingMenu.AddMatchSetting(TeamSelect2.GetOnlineSetting("teams"), false, true);
				DuckNetwork._matchSettingMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
				DuckNetwork._matchSettingMenu.Add(new UIModifierMenuItem(new UIMenuActionOpenMenu(DuckNetwork._matchSettingMenu, DuckNetwork._matchModifierMenu), UIAlign.Center, default(Color), false), true);
				DuckNetwork._matchSettingMenu.Add(new UICustomLevelMenu(new UIMenuActionOpenMenu(DuckNetwork._matchSettingMenu, DuckNetwork._levelSelectMenu), UIAlign.Center, default(Color), false), true);
				DuckNetwork._matchSettingMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
				DuckNetwork._matchSettingMenu.Add(new UIMenuItem("OK", new UIMenuActionOpenMenu(DuckNetwork._matchSettingMenu, DuckNetwork._ducknetMenu), UIAlign.Center, Color.White, true), true);
				DuckNetwork._matchSettingMenu.Close();
				DuckNetwork._ducknetUIGroup.Add(DuckNetwork._matchSettingMenu, false);
				DuckNetwork._ducknetUIGroup.Add(DuckNetwork._matchModifierMenu, false);
				DuckNetwork._ducknetUIGroup.Add(DuckNetwork._levelSelectMenu, false);
				DuckNetwork._ducknetUIGroup.Close();
				DuckNetwork._ducknetMenu.Add(new UIMenuItem("|DGBLUE|MATCH SETTINGS", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, DuckNetwork._matchSettingMenu), UIAlign.Left, default(Color), false), true);
			}
			if (Network.isClient && whoOpen.slotType != SlotType.Local)
			{
				UIMenu settingsInfoMenu = DuckNetwork.CreateMatchSettingsInfoWindow(DuckNetwork._ducknetMenu);
				DuckNetwork._ducknetUIGroup.Add(settingsInfoMenu, false);
				DuckNetwork._ducknetMenu.Add(new UIMenuItem("|DGBLUE|VIEW MATCH SETTINGS", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, settingsInfoMenu), UIAlign.Left, default(Color), false), true);
			}
			DuckNetwork._ducknetMenu.Add(new UIText("", Color.White, UIAlign.Center, 0f, null), true);
			if (canInvite && whoOpen.slotType != SlotType.Local)
			{
				DuckNetwork._inviteMenu = new UIInviteMenu("INVITE FRIENDS", null, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, -1f, "", null, false);
				((UIInviteMenu)DuckNetwork._inviteMenu).SetAction(new UIMenuActionOpenMenu(DuckNetwork._inviteMenu, DuckNetwork._ducknetMenu));
				DuckNetwork._inviteMenu.Close();
				DuckNetwork._ducknetUIGroup.Add(DuckNetwork._inviteMenu, false);
				DuckNetwork._ducknetMenu.Add(new UIMenuItem("|DGGREEN|INVITE FRIENDS", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, DuckNetwork._inviteMenu), UIAlign.Left, default(Color), false), true);
			}
			if (whoOpen.slotType != SlotType.Local || Level.current is TeamSelect2)
			{
				if (whoOpen.slotType == SlotType.Local)
				{
					DuckNetwork._ducknetMenu.Add(new UIMenuItem("|DGRED|BACK OUT", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, DuckNetwork._confirmMenu), UIAlign.Left, default(Color), false), true);
				}
				else
				{
					DuckNetwork._ducknetMenu.Add(new UIMenuItem("|DGRED|DISCONNECT", new UIMenuActionOpenMenu(DuckNetwork._ducknetMenu, DuckNetwork._confirmMenu), UIAlign.Left, default(Color), false), true);
				}
			}
			DuckNetwork._ducknetMenu.Add(new UIMenuItem("RESUME", new UIMenuActionCloseMenuSetBoolean(DuckNetwork._ducknetUIGroup, DuckNetwork._menuClosed), UIAlign.Left, default(Color), true), true);
			if (Level.current is GameLevel && Level.current.isCustomLevel)
			{
				DuckNetwork._ducknetMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
				if ((Level.current as GameLevel).data.metaData.workshopID != 0UL)
				{
					WorkshopItem item = WorkshopItem.GetItem((Level.current as GameLevel).data.metaData.workshopID);
					if ((item.stateFlags & WorkshopItemState.Subscribed) != WorkshopItemState.None)
					{
						DuckNetwork._ducknetMenu.Add(new UIMenuItem("@STEAMICON@|DGRED|UNSUBSCRIBE", new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(GameMode.Subscribe)), UIAlign.Left, default(Color), false), true);
					}
					else
					{
						DuckNetwork._ducknetMenu.Add(new UIMenuItem("@STEAMICON@|DGGREEN|SUBSCRIBE", new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(GameMode.Subscribe)), UIAlign.Left, default(Color), false), true);
					}
				}
				if (!(Level.current as GameLevel).matchOver && Network.isServer)
				{
					DuckNetwork._ducknetMenu.Add(new UIMenuItem("@SKIPSPIN@|DGRED|SKIP", new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(GameMode.Skip)), UIAlign.Left, default(Color), false), true);
				}
			}
			DuckNetwork._ducknetMenu.Close();
			DuckNetwork._ducknetMenu.SelectLastMenuItem();
			DuckNetwork._ducknetUIGroup.Add(DuckNetwork._ducknetMenu, false);
			DuckNetwork._ducknetUIGroup.Add(Options.optionsMenu, false);
			Options.openOnClose = DuckNetwork._ducknetMenu;
			DuckNetwork._confirmMenu.Add(new UIMenuItem("NO!", new UIMenuActionOpenMenu(DuckNetwork._confirmMenu, DuckNetwork._ducknetMenu), UIAlign.Center, default(Color), false), true);
			DuckNetwork._confirmMenu.Add(new UIMenuItem("YES!", new UIMenuActionCloseMenuSetBoolean(DuckNetwork._ducknetUIGroup, DuckNetwork._quit), UIAlign.Center, default(Color), false), true);
			DuckNetwork._confirmMenu.Close();
			DuckNetwork._ducknetUIGroup.Add(DuckNetwork._confirmMenu, false);
			DuckNetwork._confirmKick.Add(new UIMenuItem("NO!", new UIMenuActionOpenMenu(DuckNetwork._confirmKick, DuckNetwork._ducknetMenu), UIAlign.Center, default(Color), false), true);
			DuckNetwork._confirmKick.Add(new UIMenuItem("YES!", new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(DuckNetwork.KickedPlayer)), UIAlign.Center, default(Color), false), true);
			DuckNetwork._confirmKick.Close();
			DuckNetwork._ducknetUIGroup.Add(DuckNetwork._confirmKick, false);
			DuckNetwork._optionsMenu.Add(new UIMenuItemSlider("SFX VOLUME", null, new FieldBinding(Options.Data, "sfxVolume", 0f, 1f, 0.1f), 0.125f, default(Color)), true);
			DuckNetwork._optionsMenu.Add(new UIMenuItemSlider("MUSIC VOLUME", null, new FieldBinding(Options.Data, "musicVolume", 0f, 1f, 0.1f), 0.125f, default(Color)), true);
			DuckNetwork._optionsMenu.Add(new UIMenuItemToggle("SHENANIGANS", null, new FieldBinding(Options.Data, "shennanigans", 0f, 1f, 0.1f), default(Color), null, null, false, false), true);
			DuckNetwork._optionsMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
			DuckNetwork._optionsMenu.Add(new UIMenuItemToggle("FULLSCREEN", null, new FieldBinding(Options.Data, "fullscreen", 0f, 1f, 0.1f), default(Color), null, null, false, false), true);
			DuckNetwork._optionsMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
			DuckNetwork._optionsMenu.Add(new UIMenuItem("BACK", new UIMenuActionOpenMenu(DuckNetwork._optionsMenu, DuckNetwork._ducknetMenu), UIAlign.Center, default(Color), true), true);
			DuckNetwork._optionsMenu.Close();
			DuckNetwork._ducknetUIGroup.Add(DuckNetwork._optionsMenu, false);
			DuckNetwork._ducknetUIGroup.Close();
			Level.Add(DuckNetwork._ducknetUIGroup);
			DuckNetwork._ducknetUIGroup.Update();
			DuckNetwork._ducknetUIGroup.Update();
			DuckNetwork._ducknetUIGroup.Update();
			DuckNetwork._ducknetUIGroup.Open();
			DuckNetwork._ducknetMenu.Open();
			MonoMain.pauseMenu = DuckNetwork._ducknetUIGroup;
			HUD.AddCornerControl(HUDCorner.TopLeft, "CHAT@CHAT@", null);
			DuckNetwork._pauseOpen = true;
			SFX.Play("pause", 0.6f, 0f, 0f, false);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0009A918 File Offset: 0x00098B18
		public static void SendCustomLevelData()
		{
			int maxPacketSizeInBytes = 512;
			MemoryStream data = Editor.GetCompressedActiveLevelData();
			long dataSize = data.Length;
			int dataPosition = 0;
			Math.Ceiling((double)((float)dataSize / (float)maxPacketSizeInBytes));
			data.Position = 0L;
			Send.Message(new NMLevelDataHeader(DuckNetwork._core.levelTransferSession, (int)dataSize));
			while ((long)dataPosition != dataSize)
			{
				BitBuffer chunk = new BitBuffer(true);
				int writeBytes = (int)Math.Min(dataSize - (long)dataPosition, (long)maxPacketSizeInBytes);
				chunk.Write(data.GetBuffer(), dataPosition, writeBytes);
				dataPosition += writeBytes;
				Send.Message(new NMLevelDataChunk(DuckNetwork._core.levelTransferSession, chunk));
			}
			DuckNetworkCore core = DuckNetwork._core;
			core.levelTransferSession += 1;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0009A9C0 File Offset: 0x00098BC0
		public static void SendCurrentLevelData(ushort session, NetworkConnection c)
		{
			int maxPacketSizeInBytes = 512;
			MemoryStream data = DuckNetwork.compressedLevelData;
			long dataSize = data.Length;
			int dataPosition = 0;
			Math.Ceiling((double)((float)dataSize / (float)maxPacketSizeInBytes));
			data.Position = 0L;
			Send.Message(new NMLevelDataHeader(session, (int)dataSize), c);
			while ((long)dataPosition != dataSize)
			{
				BitBuffer chunk = new BitBuffer(true);
				int writeBytes = (int)Math.Min(dataSize - (long)dataPosition, (long)maxPacketSizeInBytes);
				chunk.Write(data.GetBuffer(), dataPosition, writeBytes);
				dataPosition += writeBytes;
				Send.Message(new NMLevelDataChunk(session, chunk), c);
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0009AA44 File Offset: 0x00098C44
		private static void OpenTeamSwitchDialogue(Profile p)
		{
			if (DuckNetwork._uhOhGroup != null && DuckNetwork._uhOhGroup.open)
			{
				return;
			}
			if (DuckNetwork._uhOhGroup != null)
			{
				Level.Remove(DuckNetwork._uhOhGroup);
			}
			DuckNetwork.ClearTeam(p);
			DuckNetwork._uhOhGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0f, 0f);
			DuckNetwork._uhOhMenu = new UIMenu("UH OH", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, -1f, "@SELECT@OK", null, false);
			float width = 190f;
			string text = "The host isn't allowing teams, and someone else is already wearing your hat :(";
			string curText = "";
			string nextWord = "";
			for (;;)
			{
				if (text.Length > 0 && text[0] != ' ')
				{
					nextWord += text[0];
				}
				else
				{
					if ((float)((curText.Length + nextWord.Length) * 8) > width)
					{
						DuckNetwork._uhOhMenu.Add(new UIText(curText, Color.White, UIAlign.Left, 0f, null), true);
						curText = "";
					}
					if (curText.Length > 0)
					{
						curText += " ";
					}
					curText += nextWord;
					nextWord = "";
				}
				if (text.Length == 0)
				{
					break;
				}
				text = text.Remove(0, 1);
			}
			if (nextWord.Length > 0)
			{
				if (curText.Length > 0)
				{
					curText += " ";
				}
				curText += nextWord;
			}
			if (curText.Length > 0)
			{
				DuckNetwork._uhOhMenu.Add(new UIText(curText, Color.White, UIAlign.Left, 0f, null), true);
			}
			DuckNetwork._uhOhMenu.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
			DuckNetwork._uhOhMenu.Add(new UIMenuItem("OH DEAR", new UIMenuActionCloseMenu(DuckNetwork._uhOhGroup), UIAlign.Center, Colors.MenuOption, true), true);
			DuckNetwork._uhOhMenu.Close();
			DuckNetwork._uhOhGroup.Add(DuckNetwork._uhOhMenu, false);
			DuckNetwork._uhOhGroup.Close();
			Level.Add(DuckNetwork._uhOhGroup);
			DuckNetwork._uhOhGroup.Open();
			DuckNetwork._uhOhMenu.Open();
			MonoMain.pauseMenu = DuckNetwork._uhOhGroup;
			SFX.Play("pause", 0.6f, 0f, 0f, false);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0009ACB4 File Offset: 0x00098EB4
		public static void ClearTeam(Profile p)
		{
			if (Level.current is TeamSelect2)
			{
				(Level.current as TeamSelect2).ClearTeam((int)p.networkIndex);
			}
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0009ACD8 File Offset: 0x00098ED8
		public static bool OnTeamSwitch(Profile p)
		{
			bool teamsAllowed = TeamSelect2.GetSettingBool("teams");
			if (teamsAllowed)
			{
				return true;
			}
			Team t = p.team;
			bool success = true;
			if (t != null)
			{
				foreach (Profile pro in DuckNetwork.profiles)
				{
					if (p.connection != null && p != pro && pro.team == p.team && Network.isServer)
					{
						if (p.connection == DuckNetwork.localConnection)
						{
							DuckNetwork.OpenTeamSwitchDialogue(p);
						}
						else
						{
							Send.Message(new NMTeamSetDenied(p.networkIndex, (byte)Teams.all.IndexOf(p.team)), p.connection);
						}
						success = false;
						return success;
					}
				}
				return success;
			}
			return success;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0009ADAC File Offset: 0x00098FAC
		private static void SendJoinMessage(NetworkConnection c)
		{
			bool invited = false;
			if (Steam.lobby != null && NCSteam.inviteLobbyID != 0UL && NCSteam.inviteLobbyID == Steam.lobby.id)
			{
				invited = true;
			}
			NCSteam.inviteLobbyID = 0UL;
			Send.Message(new NMRequestJoin(DG.version, DuckNetwork.randomID, Profile.CalculateLocalFlippers(), Network.activeNetwork.core.GetLocalName(), 1, Teams.core.extraTeams.Count > 0, invited), NetMessagePriority.ReliableOrdered, c);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0009AE24 File Offset: 0x00099024
		public static void Join(string id, string ip = "localhost")
		{
			if (DuckNetwork._core.status == DuckNetStatus.Disconnected)
			{
				DevConsole.Log(DCSection.DuckNet, "|LIME|Attempting to join " + id, -1);
				DuckNetwork.Reset();
				foreach (Profile p in Profiles.universalProfileList)
				{
					p.team = null;
				}
				for (int i = 0; i < 4; i++)
				{
					Teams.all[i].customData = null;
				}
				foreach (Profile p2 in DuckNetwork.profiles)
				{
					p2.slotType = SlotType.Open;
				}
				DuckNetwork._core.error = null;
				DuckNetwork._core.localDuckIndex = -1;
				TeamSelect2.DefaultSettings();
				Network.JoinServer(id, 1337, ip);
				DuckNetwork.localConnection.AttemptConnection();
				DuckNetwork._core.attemptTimeout = 15f;
				DuckNetwork._core.status = DuckNetStatus.EstablishingCommunication;
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0009AF40 File Offset: 0x00099140
		public static void Host(int maxPlayers, NetworkLobbyType lobbyType)
		{
			if (DuckNetwork._core.status == DuckNetStatus.Disconnected)
			{
				DevConsole.Log(DCSection.DuckNet, "|LIME|Hosting new server. ", -1);
				DuckNetwork.Reset();
				foreach (Profile p in Profiles.universalProfileList)
				{
					p.team = null;
				}
				DuckNetwork._core.error = null;
				TeamSelect2.DefaultSettings();
				Network.HostServer(lobbyType, maxPlayers, "duckGameServer", 1337);
				DuckNetwork.localConnection.AttemptConnection();
				foreach (Profile p2 in DuckNetwork.profiles)
				{
					if (lobbyType == NetworkLobbyType.Private)
					{
						p2.slotType = SlotType.Invite;
					}
					else if (lobbyType == NetworkLobbyType.FriendsOnly)
					{
						p2.slotType = SlotType.Friend;
					}
					else
					{
						p2.slotType = SlotType.Open;
					}
					if ((int)p2.networkIndex >= maxPlayers)
					{
						p2.slotType = SlotType.Closed;
					}
				}
				int numJoin = 1;
				DuckNetwork._core.localDuckIndex = -1;
				foreach (MatchmakingPlayer pro in UIMatchmakingBox.matchmakingProfiles)
				{
					string localName = Network.activeNetwork.core.GetLocalName();
					if (numJoin > 1)
					{
						localName = localName + "(" + numJoin.ToString() + ")";
					}
					if (DuckNetwork._core.localDuckIndex == -1)
					{
						DuckNetwork._core.localDuckIndex = (int)pro.duckIndex;
						DuckNetwork._core.hostDuckIndex = (int)pro.duckIndex;
					}
					Profile p3 = DuckNetwork.CreateProfile(DuckNetwork._core.localConnection, localName, (int)pro.duckIndex, pro.inputProfile, false, false, false);
					if (numJoin > 1)
					{
						p3.slotType = SlotType.Local;
					}
					DuckNetwork._core.localConnection.loadingStatus = 0;
					p3.networkStatus = DuckNetStatus.Connected;
					if (pro.team != null)
					{
						if (pro.team.customData != null)
						{
							p3.team = Teams.all[(int)pro.duckIndex];
							Team.MapFacade(p3.steamID, pro.team);
						}
						else
						{
							p3.team = pro.team;
						}
					}
					numJoin++;
				}
				DuckNetwork._core.localConnection.isHost = true;
				DuckNetwork._core.status = DuckNetStatus.Connecting;
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0009B1C8 File Offset: 0x000993C8
		public static Profile JoinLocalDuck(InputProfile input)
		{
			int index = 1;
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.connection == DuckNetwork.localConnection)
				{
					index++;
				}
			}
			string localName = Network.activeNetwork.core.GetLocalName();
			if (index > 1)
			{
				localName = localName + "(" + index.ToString() + ")";
			}
			Profile newProfile = DuckNetwork.CreateProfile(DuckNetwork.localConnection, localName, -1, input, false, false, true);
			if (newProfile == null)
			{
				return null;
			}
			if (Network.isClient)
			{
				newProfile.networkStatus = DuckNetStatus.Connecting;
			}
			else
			{
				newProfile.networkStatus = DuckNetStatus.Connected;
			}
			Level.current.OnNetworkConnecting(newProfile);
			DuckNetwork.SendNewProfile(newProfile, DuckNetwork.localConnection, false);
			return newProfile;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0009B330 File Offset: 0x00099530
		private static Profile CreateProfile(NetworkConnection c, string name = "", int index = -1, InputProfile input = null, bool hasCustomHats = false, bool invited = false, bool local = false)
		{
			bool friend = false;
			if (c.data is User && (c.data as User).id != 0UL)
			{
				if (DuckNetwork._invitedFriends.Contains((c.data as User).id))
				{
					invited = true;
				}
				if ((c.data as User).relationship == FriendRelationship.Friend)
				{
					friend = true;
				}
			}
			Profile p = null;
			if (index == -1)
			{
				p = DuckNetwork.profiles.FirstOrDefault((Profile x) => x.connection == null && x.reservedUser != null && c.data == x.reservedUser);
				if (p == null)
				{
					p = DuckNetwork.profiles.FirstOrDefault((Profile x) => x.connection == null && x.slotType == SlotType.Invite && invited);
					if (p == null)
					{
						p = DuckNetwork.profiles.FirstOrDefault((Profile x) => x.connection == null && x.slotType == SlotType.Friend && friend);
						if (p == null)
						{
							if (local)
							{
								p = DuckNetwork.profiles.FirstOrDefault((Profile x) => x.connection == null && x.slotType == SlotType.Local);
							}
							else
							{
								p = DuckNetwork.profiles.FirstOrDefault((Profile x) => x.connection == null && x.slotType == SlotType.Open);
							}
						}
					}
				}
			}
			else
			{
				p = DuckNetwork.profiles[index];
			}
			index = DuckNetwork.profiles.IndexOf(p);
			if (p == null)
			{
				return null;
			}
			if (c != null && p.connection != null && p.connection != c)
			{
				p.connection.Disconnect();
			}
			DuckNetwork.profiles[index].linkedProfile = null;
			if (Steam.user != null && c == DuckNetwork.localConnection)
			{
				foreach (Profile pro in Profiles.allCustomProfiles)
				{
					if (pro.steamID == Steam.user.id)
					{
						DuckNetwork.profiles[index].linkedProfile = pro;
						break;
					}
				}
			}
			p.hasCustomHats = hasCustomHats;
			p.name = name;
			c.profile = p;
			p.connection = c;
			if (p.reservedUser != null && p.reservedTeam != null)
			{
				p.team = p.reservedTeam;
			}
			else
			{
				p.team = Teams.all[(int)p.networkIndex];
			}
			p.reservedUser = null;
			p.reservedTeam = null;
			if (p.slotType == SlotType.Reserved)
			{
				p.slotType = SlotType.Invite;
			}
			p.persona = Persona.all.ElementAt((int)p.networkIndex);
			if (c == DuckNetwork._core.localConnection)
			{
				if (NetworkDebugger.enabled)
				{
					p.inputProfile = NetworkDebugger.inputProfiles[(int)p.networkIndex];
				}
				else if (input != null)
				{
					p.inputProfile = input;
				}
			}
			else
			{
				p.inputProfile = InputProfile.GetVirtualInput((int)p.networkIndex);
			}
			return p;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0009B65C File Offset: 0x0009985C
		public static void Reset()
		{
			foreach (Profile p in DuckNetwork.profiles)
			{
				p.connection = null;
				p.team = null;
				p.networkStatus = DuckNetStatus.Disconnected;
				p.hasCustomHats = false;
				p.reservedUser = null;
				p.reservedTeam = null;
				p.linkedProfile = null;
				p.acceptedMigration = false;
				p.flippers = 0;
				if (p.inputProfile != null)
				{
					p.inputProfile.lastActiveOverride = null;
				}
			}
			Level.core.gameInProgress = false;
			DuckNetwork._invitedFriends.Clear();
			Team.ClearFacades();
			Main.ResetGameStuff();
			Main.ResetMatchStuff();
			DuckNetwork._core.RecreateProfiles();
			DuckNetwork._core.hostDuckIndex = -1;
			DuckNetwork._core.localDuckIndex = -1;
			DuckNetwork._core.localConnection = new NetworkConnection(null, null);
			DuckNetwork._core.inGame = false;
			DuckNetwork._core.status = DuckNetStatus.Disconnected;
			DuckNetwork._core.migrationIndex = byte.MaxValue;
			DuckNetwork._core.levelIndex = 0;
			DuckNetwork._core.levelTransferSession = 0;
			DuckNetwork._core.levelTransferProgress = 0;
			DuckNetwork._core.levelTransferSize = 0;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0009B7A0 File Offset: 0x000999A0
		public static DuckNetStatus GeneralStatus()
		{
			DuckNetStatus status = DuckNetStatus.Connected;
			if (DuckNetwork.GetProfiles(DuckNetwork.localConnection).Count == 0)
			{
				return DuckNetStatus.Disconnected;
			}
			foreach (Profile p in DuckNetwork.GetProfiles(DuckNetwork.localConnection))
			{
				if (p.networkStatus != DuckNetStatus.Connected)
				{
					status = p.networkStatus;
					break;
				}
			}
			if (!(Level.current is TeamSelect2) && !(Level.current is JoinServer) && !(Level.current is DisconnectError) && !(Level.current is ConnectionError) && status != DuckNetStatus.Disconnected && status != DuckNetStatus.Disconnecting)
			{
				status = DuckNetStatus.Connected;
			}
			return status;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0009B854 File Offset: 0x00099A54
		public static void ResetProfile(Profile p)
		{
			p.networkStatus = DuckNetStatus.Disconnected;
			p.connection = null;
			p.team = null;
			p.linkedProfile = null;
			p.furniturePositionData.Clear();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0009B880 File Offset: 0x00099A80
		public static void OnDisconnect(NetworkConnection connection, string reason, bool kicked = false)
		{
			if (DuckNetwork._core.localDuckIndex == -1)
			{
				return;
			}
			List<Profile> profiles = DuckNetwork.GetProfiles(connection);
			bool hadProfile = false;
			foreach (Profile p in profiles)
			{
				hadProfile = true;
				if (Network.isServer && p.connection != DuckNetwork.localConnection)
				{
					p.networkStatus = DuckNetStatus.Disconnected;
					DuckNetwork.SendToEveryone(new NMClientDisconnect(p.connection.identifier, p.networkIndex));
					GhostManager.context.FreeOldGhosts(true);
					if (!(Level.current is TeamSelect2) && !kicked)
					{
						p.slotType = SlotType.Reserved;
						p.reservedUser = p.connection.data;
						p.reservedTeam = p.team;
					}
					Team.ClearFacade(p.steamID);
					p.connection = null;
					p.team = null;
					p.flippers = 0;
					p.linkedProfile = null;
					if (p.inputProfile != null)
					{
						p.inputProfile.lastActiveOverride = null;
					}
					DevConsole.Log(DCSection.DuckNet, "|RED|" + p.name + " Has left the DuckNet.", -1);
				}
				else
				{
					if (p.connection != DuckNetwork.localConnection)
					{
						GhostManager.context.FreeOldGhosts(true);
						if (p.reservedUser != null)
						{
							continue;
						}
						if (p.networkStatus == DuckNetStatus.Disconnecting || (reason == "CONTROLLED DISCONNECT" && p.connection.isHost))
						{
							DevConsole.Log(DCSection.DuckNet, "|RED|" + p.name + " disconnected.", -1);
							p.networkStatus = DuckNetStatus.Disconnected;
							continue;
						}
						DevConsole.Log(DCSection.DuckNet, "|RED|Trouble communicating with " + p.name + "...", -1);
						p.networkStatus = DuckNetStatus.RequiresNewConnection;
						using (List<Profile>.Enumerator enumerator2 = DuckNetwork.core.profiles.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Profile host = enumerator2.Current;
								if (host.connection != null && host.connection.isHost)
								{
									Send.Message(new NMRequiresNewConnection(p.connection.identifier), host.connection);
								}
							}
							continue;
						}
					}
					p.networkStatus = DuckNetStatus.Disconnected;
				}
			}
			if (hadProfile && DuckNetwork.status != DuckNetStatus.Disconnecting && DuckNetwork.status != DuckNetStatus.Disconnected)
			{
				if (connection.isHost)
				{
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.HostDisconnected,
						message = "|RED|Host disconnected!"
					});
				}
				else if (!(Level.current is TeamSelect2) && !(Level.current is TitleScreen) && Network.isServer)
				{
					Send.Message(new NMInGameDisconnect(connection.identifier));
					Level.current = new TeamSelect2();
				}
				if (connection == DuckNetwork.localConnection)
				{
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.ConnectionTimeout,
						message = "|RED|Connection failure!"
					});
				}
			}
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0009BB8C File Offset: 0x00099D8C
		public static void OnSessionEnded()
		{
			DuckNetwork.Reset();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0009BB94 File Offset: 0x00099D94
		public static void OnConnection(NetworkConnection c)
		{
			if (DuckNetwork.status == DuckNetStatus.EstablishingCommunication)
			{
				if (c.isHost)
				{
					DevConsole.Log(DCSection.DuckNet, "|LIME|Host contacted. Sending join request. ", -1);
					DuckNetwork._core.status = DuckNetStatus.Connecting;
					DuckNetwork.SendJoinMessage(c);
				}
				return;
			}
			DevConsole.Log(DCSection.DuckNet, "|RED|Host contacted. Join request not sent (" + DuckNetwork.status.ToString() + ")", -1);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0009BBF4 File Offset: 0x00099DF4
		public static void Update()
		{
			if (MonoMain.pauseMenu == null && DuckNetwork._pauseOpen)
			{
				HUD.CloseAllCorners();
				DuckNetwork._pauseOpen = false;
				if (Network.isServer)
				{
					TeamSelect2.UpdateModifierStatus();
				}
			}
			if (MonoMain.pauseMenu == null && DuckNetwork._willOpenSettingsInfo)
			{
				DuckNetwork.DoMatchSettingsInfoOpen();
				DuckNetwork._willOpenSettingsInfo = false;
			}
			if (DuckNetwork._core.status == DuckNetStatus.Disconnected || DuckNetwork._core.status == DuckNetStatus.Disconnecting)
			{
				DuckNetwork._quit.value = false;
				return;
			}
			if (DuckNetwork._quit.value)
			{
				if (DuckNetwork._menuOpenProfile != null && DuckNetwork._menuOpenProfile.slotType == SlotType.Local)
				{
					DuckNetwork.Kick(DuckNetwork._menuOpenProfile);
				}
				else
				{
					if (Steam.lobby != null)
					{
						UIMatchmakingBox.nonPreferredServers.Add(Steam.lobby.id);
					}
					Level.current = new DisconnectFromGame();
				}
				DuckNetwork._quit.value = false;
			}
			if (DuckNetwork._menuClosed.value && Network.isServer)
			{
				string newSettings = TeamSelect2.GetMatchSettingString();
				if (newSettings != DuckNetwork._settingsBeforeOpen)
				{
					TeamSelect2.SendMatchSettings(null, false);
					Send.Message(new NMMatchSettingsChanged());
				}
				(Network.activeNetwork.core as NCSteam).ApplyLobbyData();
				DuckNetwork._menuClosed.value = false;
			}
			if (Keyboard.Pressed(Keys.F1, false) && !Keyboard.Down(Keys.LeftShift) && !Keyboard.Down(Keys.RightShift))
			{
				ConnectionStatusUI.Show();
			}
			if (Keyboard.Released(Keys.F1))
			{
				ConnectionStatusUI.Hide();
			}
			if (Network.isClient)
			{
				if (DuckNetwork.status == DuckNetStatus.Connected)
				{
					DuckNetwork._core.attemptTimeout = 10f;
				}
				if (DuckNetwork._core.attemptTimeout > 0f)
				{
					DuckNetwork._core.attemptTimeout -= Maths.IncFrameTimer();
				}
				else if (DuckNetwork.status != DuckNetStatus.Connected)
				{
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.ConnectionTimeout,
						message = "|RED|Connection timeout."
					});
				}
			}
			if (MonoMain.pauseMenu != null)
			{
				DuckNetwork._core.enteringText = false;
				DuckNetwork._core.stopEnteringText = false;
			}
			bool skipPause = false;
			if (Network.isActive && MonoMain.pauseMenu == null)
			{
				List<ChatMessage> remove = new List<ChatMessage>();
				foreach (ChatMessage i in DuckNetwork._core.chatMessages)
				{
					i.timeout -= 0.016f;
					if (i.timeout < 0f)
					{
						i.alpha -= 0.01f;
					}
					if (i.alpha < 0f)
					{
						remove.Add(i);
					}
				}
				foreach (ChatMessage j in remove)
				{
					DuckNetwork._core.chatMessages.Remove(j);
				}
				if (DuckNetwork._core.stopEnteringText)
				{
					DuckNetwork._core.enteringText = false;
					DuckNetwork._core.stopEnteringText = false;
				}
				if (!DevConsole.core.open)
				{
					bool entering = DuckNetwork._core.enteringText;
					DuckNetwork._core.enteringText = false;
					bool pressed = Input.Pressed("CHAT", "Any");
					DuckNetwork._core.enteringText = entering;
					if (pressed)
					{
						if (!DuckNetwork._core.enteringText)
						{
							DuckNetwork._core.enteringText = true;
							DuckNetwork._core.currentEnterText = "";
							Keyboard.keyString = "";
						}
						else
						{
							if (DuckNetwork._core.currentEnterText != "")
							{
								NMChatMessage message = new NMChatMessage((byte)DuckNetwork._core.localDuckIndex, DuckNetwork._core.currentEnterText, DuckNetwork._core.chatIndex);
								DuckNetworkCore core = DuckNetwork._core;
								core.chatIndex += 1;
								DuckNetwork.SendToEveryone(message);
								DuckNetwork.ChatMessageReceived(message);
								DuckNetwork._core.currentEnterText = "";
							}
							DuckNetwork._core.stopEnteringText = true;
						}
					}
					else if (DuckNetwork._core.enteringText && Keyboard.Pressed(Keys.Escape, false))
					{
						DuckNetwork._core.stopEnteringText = true;
						skipPause = true;
					}
				}
				if (DuckNetwork._core.enteringText)
				{
					if (Keyboard.keyString.Length > 90)
					{
						Keyboard.keyString = Keyboard.keyString.Substring(0, 90);
					}
					DuckNetwork._core.currentEnterText = Keyboard.keyString;
				}
			}
			bool openedMenu = false;
			int numConnected = 0;
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.connection != null)
				{
					if (MonoMain.pauseMenu == null && (DuckNetwork._ducknetUIGroup == null || !DuckNetwork._ducknetUIGroup.open) && p.connection == DuckNetwork.localConnection && p.inputProfile.Pressed("START", false) && !skipPause && !openedMenu && !(Level.current is RockScoreboard) && (Level.current is TeamSelect2 || Level.current is GameLevel || Level.current is RockScoreboard))
					{
						DuckNetwork.OpenMenu(p);
						openedMenu = true;
					}
					numConnected++;
					if (p.connection.status == ConnectionStatus.Connected && p.networkStatus != DuckNetStatus.Disconnecting && p.networkStatus != DuckNetStatus.Disconnected && p.networkStatus != DuckNetStatus.Failure)
					{
						p.currentStatusTimeout -= Maths.IncFrameTimer();
						if (p.networkStatus == DuckNetStatus.NeedsNotificationWhenReadyForData)
						{
							if (p.currentStatusTimeout <= 0f)
							{
								p.currentStatusTimeout = 2f;
								p.currentStatusTries++;
							}
							if (p.currentStatusTries > 10)
							{
								if (p.isHost)
								{
									DuckNetwork.RaiseError(new DuckNetErrorInfo
									{
										error = DuckNetError.ConnectionTimeout,
										message = "|RED|Took too long to receive level data."
									});
								}
								p.connection.Disconnect();
							}
							if (Network.isServer || DuckNetwork.localConnection.loadingStatus == DuckNetwork.levelIndex)
							{
								if (p.connection != DuckNetwork.localConnection)
								{
									Send.Message(new NMLevelDataReady(DuckNetwork.levelIndex), p.connection);
								}
								p.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
							}
						}
						if (p.networkStatus == DuckNetStatus.WaitingForLoadingToBeFinished)
						{
							if (p.connection.loadingStatus == DuckNetwork.levelIndex)
							{
								p.networkStatus = DuckNetStatus.Connected;
							}
							else
							{
								if (p.currentStatusTimeout <= 0f)
								{
									if (p.connection != DuckNetwork.localConnection)
									{
										Send.Message(new NMAwaitingLevelReady(DuckNetwork.levelIndex), p.connection);
										DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Requesting level data from " + p.name, -1);
									}
									else
									{
										DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Still waiting for level data...", -1);
									}
									p.currentStatusTimeout = 7f;
									p.currentStatusTries++;
								}
								if (p.currentStatusTries > 4)
								{
									if (p.isHost)
									{
										DuckNetwork.RaiseError(new DuckNetErrorInfo
										{
											error = DuckNetError.ConnectionTimeout,
											message = "|RED|Took too long to connect with " + p.name + "."
										});
									}
									p.connection.Disconnect();
								}
							}
						}
						if (p.connection.wantsGhostData == (int)DuckNetwork.levelIndex && Level.current.initialized)
						{
							GhostManager.context.RefreshGhosts(null);
							Level.current.SendLevelData(p.connection);
							GhostManager.context.SendAllGhostData(false, NetMessagePriority.ReliableOrdered, p.connection, true);
							p.connection.sendPacketsNow = true;
							Send.Message(new NMEndOfGhostData(DuckNetwork.levelIndex), p.connection);
							Send.Message(new NMLevelDataReady(DuckNetwork.levelIndex), p.connection);
							DevConsole.Log(DCSection.DuckNet, string.Concat(new object[]
							{
								"|LIME|",
								p.connection.identifier,
								" LOADED LEVEL (",
								DuckNetwork.levelIndex,
								")"
							}), -1);
							p.connection.wantsGhostData = -1;
						}
					}
				}
			}
			if (DuckNetwork.error != null && DuckNetwork._core.status != DuckNetStatus.Disconnecting)
			{
				DuckNetwork._core.status = DuckNetStatus.Disconnecting;
				Network.Disconnect();
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0009C454 File Offset: 0x0009A654
		public static void ChatMessageReceived(NMChatMessage message)
		{
			if ((int)message.profileIndex >= DuckNetwork._core.profiles.Count)
			{
				return;
			}
			DuckNetwork._core.chatMessages.Add(new ChatMessage(DuckNetwork._core.profiles[(int)message.profileIndex], message.text, message.index));
			DuckNetwork._core.chatMessages = (from x in DuckNetwork._core.chatMessages
			orderby (int)(-(int)x.index)
			select x).ToList<ChatMessage>();
			SFX.Play("chatmessage", 0.8f, Rando.Float(-0.15f, 0.15f), 0f, false);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0009C510 File Offset: 0x0009A710
		public static List<Profile> GetProfiles(NetworkConnection connection)
		{
			List<Profile> pz = new List<Profile>();
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.connection == connection)
				{
					pz.Add(p);
				}
			}
			return pz;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0009C574 File Offset: 0x0009A774
		public static int IndexOf(Profile p)
		{
			return DuckNetwork.profiles.IndexOf(p);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0009C581 File Offset: 0x0009A781
		public static void Disconnect(Profile who)
		{
			GhostManager.context.FreeOldGhosts(true);
			who.networkStatus = DuckNetStatus.Disconnected;
			Team.ClearFacade(who.steamID);
			who.connection = null;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0009C5B2 File Offset: 0x0009A7B2
		private static Profile GetOpenProfile()
		{
			return DuckNetwork._core.profiles.FirstOrDefault((Profile x) => x.connection == null);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0009C5E0 File Offset: 0x0009A7E0
		public static void SendNewProfile(Profile joinProfile, NetworkConnection notifyCreation, bool notifyConnectionIsExisting = false)
		{
			if (notifyCreation != null && notifyCreation != DuckNetwork.localConnection)
			{
				Send.Message(new NMJoinDuckNetwork(joinProfile.networkIndex), notifyCreation);
				if (joinProfile.team != null)
				{
					Send.Message(new NMSetTeam(joinProfile.networkIndex, (byte)Teams.IndexOf(joinProfile.team)));
				}
				if (joinProfile.furniturePositions.Count > 0)
				{
					Send.Message(new NMRoomData(joinProfile.furniturePositionData, joinProfile.networkIndex));
				}
			}
			byte joinTeamSel = joinProfile.networkIndex;
			if (joinProfile.team != null)
			{
				if (Teams.IndexOf(joinProfile.team) >= Teams.allStock.Count)
				{
					joinProfile.team = Teams.all[(int)joinProfile.networkIndex];
				}
				joinTeamSel = (byte)Teams.IndexOf(joinProfile.team);
			}
			int count = 0;
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.networkStatus != DuckNetStatus.Disconnecting)
				{
					if (p != joinProfile && p.connection != null && p.connection != notifyCreation)
					{
						byte teamSel = p.networkIndex;
						if (p.team != null)
						{
							if (Teams.IndexOf(p.team) >= Teams.allStock.Count)
							{
								p.team = Teams.all[(int)p.networkIndex];
							}
							teamSel = (byte)Teams.IndexOf(p.team);
						}
						if (!notifyConnectionIsExisting && notifyCreation != null && notifyCreation != DuckNetwork.localConnection)
						{
							Send.Message(new NMRemoteJoinDuckNetwork((byte)count, (p.connection == DuckNetwork.localConnection) ? "SERVER" : p.connection.identifier, p.name, p.hasCustomHats, teamSel, p.flippers), notifyCreation);
							if (p.team != null)
							{
								if (p.team.customData != null)
								{
									Send.Message(new NMSpecialHat(p.team, p.steamID));
								}
								Send.Message(new NMSetTeam((byte)count, (byte)Teams.IndexOf(p.team)));
							}
							if (p.furniturePositions.Count > 0)
							{
								Send.Message(new NMRoomData(p.furniturePositionData, (byte)count));
							}
						}
						if (p.connection != DuckNetwork.localConnection)
						{
							Send.Message(new NMRemoteJoinDuckNetwork(joinProfile.networkIndex, (notifyCreation == DuckNetwork.localConnection) ? "SERVER" : notifyCreation.identifier, joinProfile.name, p.hasCustomHats, joinTeamSel, p.flippers), p.connection);
						}
					}
					count++;
				}
			}
			if (!notifyConnectionIsExisting && notifyCreation != null && notifyCreation != DuckNetwork.localConnection)
			{
				Send.Message(new NMEndOfDuckNetworkData(), notifyCreation);
			}
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0009C884 File Offset: 0x0009AA84
		public static NMVersionMismatch.Type CheckVersion(string id)
		{
			string[] split = id.Split(new char[]
			{
				'.'
			});
			NMVersionMismatch.Type type = NMVersionMismatch.Type.Match;
			if (split.Length == 4)
			{
				try
				{
					int verLow = Convert.ToInt32(split[3]);
					int verHigh = Convert.ToInt32(split[2]);
					if (verHigh < DG.versionHigh)
					{
						type = NMVersionMismatch.Type.Older;
					}
					else if (verHigh > DG.versionHigh)
					{
						type = NMVersionMismatch.Type.Newer;
					}
					else if (verLow < DG.versionLow)
					{
						type = NMVersionMismatch.Type.Older;
					}
					else if (verLow > DG.versionLow)
					{
						type = NMVersionMismatch.Type.Newer;
					}
				}
				catch
				{
					type = NMVersionMismatch.Type.Error;
				}
			}
			return type;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0009C908 File Offset: 0x0009AB08
		public static NetMessage OnMessageFromNewClient(NetMessage m)
		{
			if (Network.isServer)
			{
				if (m is NMRequestJoin)
				{
					if (DuckNetwork.inGame)
					{
						return new NMGameInProgress();
					}
					NMRequestJoin joinMessage = m as NMRequestJoin;
					DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join attempt from " + joinMessage.name, -1);
					NMVersionMismatch.Type type = DuckNetwork.CheckVersion(joinMessage.id);
					if (type != NMVersionMismatch.Type.Match)
					{
						DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + joinMessage.name + " had a version mismatch.", -1);
						return new NMVersionMismatch(type, DG.version);
					}
					Profile joinProfile = DuckNetwork.CreateProfile(m.connection, joinMessage.name, -1, null, joinMessage.hasCustomHats, joinMessage.wasInvited, false);
					if (joinProfile == null)
					{
						DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + joinMessage.name + " could not join, server is full.", -1);
						return new NMServerFull();
					}
					joinProfile.flippers = joinMessage.flippers;
					joinProfile.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
					DuckNetwork._core.status = DuckNetStatus.Connected;
					Level.current.OnNetworkConnecting(joinProfile);
					DuckNetwork.SendNewProfile(joinProfile, m.connection, false);
					Send.Message(new NMChangeSlots((byte)DuckNetwork.profiles[0].slotType, (byte)DuckNetwork.profiles[1].slotType, (byte)DuckNetwork.profiles[2].slotType, (byte)DuckNetwork.profiles[3].slotType), m.connection);
					TeamSelect2.SendMatchSettings(m.connection, true);
					return null;
				}
				else if (m is NMMessageIgnored)
				{
					return null;
				}
			}
			else
			{
				if (m is NMRequestJoin)
				{
					DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Another computer has requested a matchmaking connection.", -1);
					return new NMGameInProgress();
				}
				if (m is NMMessageIgnored)
				{
					return null;
				}
			}
			return new NMMessageIgnored();
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0009CAA0 File Offset: 0x0009ACA0
		public static bool HandleCoreConnectionMessages(NetMessage m)
		{
			if (m is NMClientNeedsLevelData)
			{
				NMClientNeedsLevelData needsData = m as NMClientNeedsLevelData;
				if (needsData.levelIndex == DuckNetwork.levelIndex)
				{
					m.connection.dataTransferProgress = 0;
					m.connection.dataTransferSize = 0;
					DuckNetwork.SendCurrentLevelData(needsData.transferSession, m.connection);
				}
				return true;
			}
			if (m is NMLevelDataHeader)
			{
				if (Network.isClient)
				{
					NMLevelDataHeader header = m as NMLevelDataHeader;
					if (DuckNetwork._core.levelTransferSession == header.transferSession)
					{
						DuckNetwork._core.levelTransferProgress = 0;
						DuckNetwork._core.levelTransferSize = header.length;
					}
				}
				return true;
			}
			if (m is NMLevelDataChunk)
			{
				if (Network.isClient)
				{
					NMLevelDataChunk chunk = m as NMLevelDataChunk;
					if (DuckNetwork._core.levelTransferSession == chunk.transferSession)
					{
						DuckNetwork._core.levelTransferProgress += chunk.GetBuffer().lengthInBytes;
						if (DuckNetwork._core.compressedLevelData == null)
						{
							DuckNetwork._core.compressedLevelData = new MemoryStream();
						}
						DuckNetwork._core.compressedLevelData.Write(chunk.GetBuffer().buffer, 0, chunk.GetBuffer().lengthInBytes);
						if (DuckNetwork._core.levelTransferProgress == DuckNetwork._core.levelTransferSize)
						{
							(Level.core.currentLevel as XMLLevel).ApplyLevelData(Editor.ReadCompressedLevelData(DuckNetwork._core.compressedLevelData));
						}
					}
				}
				return true;
			}
			if (m is NMChatMessage)
			{
				NMChatMessage chat = m as NMChatMessage;
				chat.index = DuckNetwork._core.chatIndex;
				DuckNetworkCore core = DuckNetwork._core;
				core.chatIndex += 1;
				DuckNetwork.ChatMessageReceived(chat);
				return true;
			}
			if (m is NMLevelDataReady)
			{
				if ((m as NMLevelDataReady).levelIndex == DuckNetwork.levelIndex)
				{
					DevConsole.Log(DCSection.DuckNet, string.Concat(new object[]
					{
						"|DGYELLOW|Level ready message from ",
						m.connection.name,
						"(",
						(m as NMLevelDataReady).levelIndex,
						")"
					}), -1);
					foreach (Profile p in DuckNetwork.GetProfiles(m.connection))
					{
						p.connection.loadingStatus = (m as NMLevelDataReady).levelIndex;
					}
					if (Network.isServer)
					{
						bool ready = true;
						foreach (Profile pro in DuckNetwork._core.profiles)
						{
							if (pro.connection != DuckNetwork.localConnection && pro.connection != null && pro.connection.loadingStatus != DuckNetwork.levelIndex)
							{
								ready = false;
								break;
							}
						}
						if (ready)
						{
							DuckNetwork.SendToEveryone(new NMAllClientsReady());
						}
					}
				}
				else
				{
					DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Level ready message from " + m.connection.name + "(BAD LEVEL)", -1);
				}
				return true;
			}
			if (m is NMAwaitingLevelReady)
			{
				if ((m as NMAwaitingLevelReady).levelIndex == DuckNetwork.levelIndex)
				{
					if (DuckNetwork.levelIndex == DuckNetwork.localConnection.loadingStatus)
					{
						Send.Message(new NMLevelDataReady(DuckNetwork.levelIndex), m.connection);
					}
					else
					{
						foreach (Profile p2 in DuckNetwork.GetProfiles(m.connection))
						{
							if (p2.networkStatus == DuckNetStatus.Connected || p2.networkStatus == DuckNetStatus.WaitingForLoadingToBeFinished)
							{
								p2.networkStatus = DuckNetStatus.NeedsNotificationWhenReadyForData;
							}
						}
					}
					DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + m.connection.name + " requested level loaded confirmation.", -1);
				}
				else
				{
					DevConsole.Log(DCSection.DuckNet, string.Concat(new object[]
					{
						"|DGYELLOW|",
						m.connection.name,
						" tried to request an invalid level (",
						(m as NMAwaitingLevelReady).levelIndex,
						" VS ",
						DuckNetwork.levelIndex,
						")"
					}), -1);
					Send.Message(new NMInvalidLevel(), m.connection);
				}
				return true;
			}
			if (Network.isServer)
			{
				if (m is NMRequiresNewConnection)
				{
					List<Profile> mProfiles = DuckNetwork.GetProfiles(m.connection);
					if (mProfiles.Count > 0)
					{
						NMRequiresNewConnection newConnection = m as NMRequiresNewConnection;
						bool foundConnection = false;
						foreach (Profile p3 in DuckNetwork.profiles)
						{
							if (p3.connection != null && p3.connection.identifier == newConnection.toWhom && p3.connection != DuckNetwork.localConnection)
							{
								byte teamSel = p3.networkIndex;
								if (p3.team != null)
								{
									if (Teams.IndexOf(p3.team) >= Teams.allStock.Count)
									{
										p3.team = Teams.all[(int)p3.networkIndex];
									}
									teamSel = (byte)Teams.IndexOf(p3.team);
								}
								Send.Message(new NMRemoteJoinDuckNetwork(p3.networkIndex, p3.connection.identifier, p3.name, p3.hasCustomHats, teamSel, p3.flippers), m.connection);
								if (p3.team != null)
								{
									if (p3.team.customData != null)
									{
										Send.Message(new NMSpecialHat(p3.team, p3.steamID), m.connection);
									}
									Send.Message(new NMSetTeam(p3.networkIndex, (byte)Teams.IndexOf(p3.team)), m.connection);
								}
								if (p3.furniturePositions.Count > 0)
								{
									Send.Message(new NMRoomData(p3.furniturePositionData, p3.networkIndex), m.connection);
								}
								foreach (Profile mPro in mProfiles)
								{
									teamSel = mPro.networkIndex;
									if (mPro.team != null)
									{
										if (Teams.IndexOf(mPro.team) >= Teams.allStock.Count)
										{
											mPro.team = Teams.all[(int)mPro.networkIndex];
										}
										teamSel = (byte)Teams.IndexOf(mPro.team);
									}
									Send.Message(new NMRemoteJoinDuckNetwork(mPro.networkIndex, mPro.connection.identifier, mPro.name, mPro.hasCustomHats, teamSel, p3.flippers), p3.connection);
									if (mPro.team != null)
									{
										if (mPro.team.customData != null)
										{
											Send.Message(new NMSpecialHat(mPro.team, mPro.steamID), p3.connection);
										}
										Send.Message(new NMSetTeam(mPro.networkIndex, (byte)Teams.IndexOf(mPro.team)), p3.connection);
									}
									if (mPro.furniturePositions.Count > 0)
									{
										Send.Message(new NMRoomData(mPro.furniturePositionData, mPro.networkIndex), p3.connection);
									}
								}
								DevConsole.Log(DCSection.DuckNet, string.Concat(new string[]
								{
									"|DGYELLOW|",
									m.connection.name,
									" needs a connection to ",
									p3.connection.name,
									"..."
								}), -1);
								foundConnection = true;
							}
						}
						if (!foundConnection)
						{
							Send.Message(new NMNoConnectionExists(newConnection.toWhom), m.connection);
							DevConsole.Log(DCSection.DuckNet, string.Concat(new string[]
							{
								"|RED|",
								m.connection.name,
								" requested a bad connection (",
								newConnection.toWhom,
								")"
							}), -1);
						}
					}
					else
					{
						Send.Message(new NMInvalidUser(), m.connection);
						DevConsole.Log(DCSection.DuckNet, "|RED|Invalid user requested a connection (" + m.connection.name + ")", -1);
					}
					return true;
				}
			}
			else
			{
				if (m is NMKick)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					if (Steam.lobby != null)
					{
						UIMatchmakingBox.nonPreferredServers.Add(Steam.lobby.id);
					}
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.Kicked,
						message = "|RED|Oh no! The host kicked you :("
					});
					return true;
				}
				if (m is NMClientDisconnect)
				{
					NMClientDisconnect disconnect = m as NMClientDisconnect;
					if (DuckNetwork.profiles[(int)disconnect.duckIndex].connection != null && DuckNetwork.profiles[(int)disconnect.duckIndex].connection.identifier == disconnect.whom)
					{
						if (DuckNetwork.profiles[(int)disconnect.duckIndex].connection == Network.host && (int)disconnect.duckIndex != DuckNetwork.hostDuckIndex)
						{
							DuckNetwork.profiles[(int)disconnect.duckIndex].networkStatus = DuckNetStatus.Disconnected;
							DuckNetwork.profiles[(int)disconnect.duckIndex].connection = null;
							DuckNetwork.profiles[(int)disconnect.duckIndex].team = null;
							DevConsole.Log(DCSection.DuckNet, "|RED|Host local disconnect (" + m.connection.name + ")", -1);
						}
						else
						{
							if (DuckNetwork.profiles[(int)disconnect.duckIndex].connection.status == ConnectionStatus.Disconnected)
							{
								DuckNetwork.profiles[(int)disconnect.duckIndex].networkStatus = DuckNetStatus.Disconnected;
							}
							else
							{
								DuckNetwork.profiles[(int)disconnect.duckIndex].networkStatus = DuckNetStatus.Disconnecting;
								DuckNetwork.profiles[(int)disconnect.duckIndex].connection.Disconnect();
							}
							Team.ClearFacade(DuckNetwork.profiles[(int)disconnect.duckIndex].steamID);
							DevConsole.Log(DCSection.DuckNet, "|RED|Client disconnect (" + m.connection.name + ")", -1);
						}
					}
					return true;
				}
				if (m is NMGameInProgress)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.GameInProgress,
						message = "|RED|Game was already in progress."
					});
					return true;
				}
				if (m is NMServerFull)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.FullServer,
						message = "|RED|The game was full!"
					});
					return true;
				}
				if (m is NMInvalidLevel)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.InvalidLevel,
						message = "|RED|Level request was invalid!"
					});
					return true;
				}
				if (m is NMInvalidUser)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.InvalidUser,
						message = "|RED|The host did not reconize you!"
					});
					return true;
				}
				if (m is NMInvalidCustomHat)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					DuckNetwork.RaiseError(new DuckNetErrorInfo
					{
						error = DuckNetError.InvalidCustomHat,
						message = "|RED|Your custom hat was invalid!"
					});
					return true;
				}
				if (m is NMVersionMismatch)
				{
					DuckNetwork._core.status = DuckNetStatus.Failure;
					NMVersionMismatch ver = m as NMVersionMismatch;
					DuckNetwork.FailWithVersionMismatch(ver.serverVersion, ver.GetCode());
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0009D684 File Offset: 0x0009B884
		public static void FailWithVersionMismatch(string theirVersion, NMVersionMismatch.Type type)
		{
			DuckNetwork._core.status = DuckNetStatus.Failure;
			string message = "";
			bool tooNew = false;
			if (type == NMVersionMismatch.Type.Error)
			{
				message = "|RED|Your game version caused an error.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
			}
			else if (type == NMVersionMismatch.Type.Newer)
			{
				message = "|RED|Your version is too old.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
			}
			else if (type == NMVersionMismatch.Type.Older)
			{
				message = "|RED|Your version is too new.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
				tooNew = true;
			}
			DuckNetwork.RaiseError(new DuckNetErrorInfo
			{
				error = DuckNetError.VersionMismatch,
				message = message,
				tooNew = tooNew
			});
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0009D71C File Offset: 0x0009B91C
		public static DuckNetErrorInfo AssembleMismatchError(string theirVersion)
		{
			string message = "";
			NMVersionMismatch.Type type = DuckNetwork.CheckVersion(theirVersion);
			bool tooNew = false;
			if (type == NMVersionMismatch.Type.Error)
			{
				message = "|RED|Your game version caused an error.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
			}
			else if (type == NMVersionMismatch.Type.Newer)
			{
				message = "|RED|Your version is too old.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
			}
			else if (type == NMVersionMismatch.Type.Older)
			{
				message = "|RED|Your version is too new.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
				tooNew = true;
			}
			return new DuckNetErrorInfo
			{
				error = DuckNetError.VersionMismatch,
				message = message,
				tooNew = tooNew
			};
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0009D7A8 File Offset: 0x0009B9A8
		public static void OnMessage(NetMessage m)
		{
			if (m is NMJoinDuckNetwork)
			{
				DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join message", -1);
			}
			if (DuckNetwork.status == DuckNetStatus.Disconnected)
			{
				return;
			}
			if (m is NMDuckNetworkEvent)
			{
				(m as NMDuckNetworkEvent).Activate();
				return;
			}
			if (m == null)
			{
				Main.codeNumber = 13371;
			}
			UIMatchmakingBox.pulseNetwork = true;
			if (DuckNetwork.GetProfiles(m.connection).Count == 0 && m.connection != Network.host)
			{
				Main.codeNumber = 13372;
				NetMessage result = DuckNetwork.OnMessageFromNewClient(m);
				if (result != null)
				{
					Send.Message(result, m.connection);
				}
				return;
			}
			if (DuckNetwork.HandleCoreConnectionMessages(m))
			{
				return;
			}
			if (DuckNetwork.status == DuckNetStatus.Disconnecting)
			{
				return;
			}
			Main.codeNumber = 13373;
			foreach (Profile p in DuckNetwork.GetProfiles(m.connection))
			{
				if (p.networkStatus == DuckNetStatus.Disconnecting || p.networkStatus == DuckNetStatus.Disconnected || p.networkStatus == DuckNetStatus.Failure)
				{
					return;
				}
			}
			Main.codeNumber = (int)m.typeIndex;
			if (Network.isServer)
			{
				if (m is NMLateJoinDuckNetwork)
				{
					if (!(Level.current is TeamSelect2))
					{
						Send.Message(new NMGameInProgress(), NetMessagePriority.ReliableOrdered, m.connection);
						return;
					}
					NMLateJoinDuckNetwork joinMessage = m as NMLateJoinDuckNetwork;
					DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Late join attempt from " + joinMessage.name, -1);
					Profile joinProfile = DuckNetwork.CreateProfile(m.connection, joinMessage.name, (int)joinMessage.duckIndex, null, false, false, false);
					if (joinProfile != null)
					{
						joinProfile.networkStatus = DuckNetStatus.Connected;
						Level.current.OnNetworkConnecting(joinProfile);
						DuckNetwork.SendNewProfile(joinProfile, m.connection, true);
						return;
					}
					Send.Message(new NMServerFull(), NetMessagePriority.ReliableOrdered, m.connection);
					return;
				}
				else
				{
					if (m is NMJoinedDuckNetwork)
					{
						foreach (Profile profile in DuckNetwork.GetProfiles(m.connection))
						{
							DevConsole.Log(DCSection.DuckNet, "|LIME|" + profile.name + " Has joined the DuckNet", -1);
						}
						Send.Message(new NMSwitchLevel("@TEAMSELECT", DuckNetwork.levelIndex, (ushort)GhostManager.context.currentGhostIndex, false, 0u), m.connection);
						return;
					}
					if (m is NMClientLoadedLevel)
					{
						if ((m as NMClientLoadedLevel).levelIndex == DuckNetwork.levelIndex)
						{
							m.connection.wantsGhostData = (int)(m as NMClientLoadedLevel).levelIndex;
							return;
						}
						DevConsole.Log(DCSection.DuckNet, string.Concat(new object[]
						{
							"|DGRED|",
							m.connection.identifier,
							" LOADED WRONG LEVEL! (",
							DuckNetwork.levelIndex,
							" VS ",
							(m as NMClientLoadedLevel).levelIndex,
							")"
						}), -1);
						return;
					}
					else if (m is NMSetTeam)
					{
						NMSetTeam t = m as NMSetTeam;
						if (t.duck < 0 || t.duck >= 4)
						{
							return;
						}
						Profile p2 = DuckNetwork.profiles[(int)t.duck];
						if (p2.connection == null || p2.team == null)
						{
							return;
						}
						p2.team = Teams.all[(int)t.team];
						if (DuckNetwork.OnTeamSwitch(p2))
						{
							Send.MessageToAllBut(new NMSetTeam(t.duck, t.team), NetMessagePriority.ReliableOrdered, m.connection);
							return;
						}
						return;
					}
					else if (m is NMRoomData)
					{
						NMRoomData t2 = m as NMRoomData;
						if (t2.duck < 0 || t2.duck >= 4)
						{
							return;
						}
						Profile p3 = DuckNetwork.profiles[(int)t2.duck];
						if (p3.connection != null && p3.connection != DuckNetwork.localConnection)
						{
							p3.furniturePositionData = t2.data;
							Send.MessageToAllBut(new NMRoomData(t2.data, t2.duck), NetMessagePriority.ReliableOrdered, m.connection);
							return;
						}
						return;
					}
					else
					{
						if (!(m is NMSpecialHat))
						{
							return;
						}
						NMSpecialHat specialHat = m as NMSpecialHat;
						Team t3 = Team.Deserialize(specialHat.GetData());
						using (List<Profile>.Enumerator enumerator3 = DuckNetwork.profiles.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Profile p4 = enumerator3.Current;
								if (p4.steamID == specialHat.link)
								{
									if (t3 != null)
									{
										Team.MapFacade(p4.steamID, t3);
									}
									else
									{
										Team.ClearFacade(p4.steamID);
									}
									Send.MessageToAllBut(new NMSpecialHat(t3, p4.steamID), NetMessagePriority.ReliableOrdered, m.connection);
								}
							}
							return;
						}
					}
				}
			}
			if (m is NMSpecialHat)
			{
				NMSpecialHat specialHat2 = m as NMSpecialHat;
				Team t4 = Team.Deserialize(specialHat2.GetData());
				using (List<Profile>.Enumerator enumerator4 = DuckNetwork.profiles.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Profile p5 = enumerator4.Current;
						if (p5.steamID == specialHat2.link)
						{
							if (t4 != null)
							{
								Team.MapFacade(p5.steamID, t4);
							}
							else
							{
								Team.ClearFacade(p5.steamID);
							}
						}
					}
					return;
				}
			}
			if (m is NMJoinDuckNetwork)
			{
				NMRemoteJoinDuckNetwork remote = m as NMRemoteJoinDuckNetwork;
				if (remote == null)
				{
					DevConsole.Log(DCSection.DuckNet, "|LIME|Connection with host was established!", -1);
					NMJoinDuckNetwork join = m as NMJoinDuckNetwork;
					DuckNetwork._core.status = DuckNetStatus.Connected;
					if (DuckNetwork.profiles[(int)join.duckIndex].connection == DuckNetwork.localConnection)
					{
						DuckNetwork.profiles[(int)join.duckIndex].networkStatus = DuckNetStatus.Connected;
						return;
					}
					Profile profile2 = DuckNetwork.CreateProfile(DuckNetwork.localConnection, Network.activeNetwork.core.GetLocalName(), (int)join.duckIndex, (UIMatchmakingBox.matchmakingProfiles.Count > 0) ? UIMatchmakingBox.matchmakingProfiles[0].inputProfile : InputProfile.DefaultPlayer1, Teams.core.extraTeams.Count > 0, false, false);
					DuckNetwork._core.localDuckIndex = (int)join.duckIndex;
					profile2.flippers = Profile.CalculateLocalFlippers();
					profile2.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
					return;
				}
				else
				{
					NetworkConnection joinConnection = remote.connection;
					Main.codeNumber = 133701;
					if (!(remote.identifier == "SERVER"))
					{
						Main.codeNumber = 133705;
						bool foundStatus = false;
						DuckNetStatus foundStatusValue = DuckNetStatus.NeedsNotificationWhenReadyForData;
						foreach (Profile p6 in DuckNetwork.GetProfiles(joinConnection))
						{
							if (p6.connection.identifier == remote.identifier)
							{
								joinConnection = p6.connection;
								foundStatus = true;
								foundStatusValue = p6.networkStatus;
								break;
							}
						}
						Main.codeNumber = 133706;
						if (!foundStatus)
						{
							joinConnection = Network.activeNetwork.core.AttemptConnection(remote.identifier);
							if (joinConnection == null)
							{
								DuckNetwork.RaiseError(new DuckNetErrorInfo
								{
									error = DuckNetError.InvalidConnectionInformation,
									message = "Invalid connection information (" + remote.identifier + ")"
								});
								return;
							}
						}
						Main.codeNumber = 133707;
						Profile profile3 = DuckNetwork.CreateProfile(joinConnection, remote.name, (int)remote.duckIndex, null, remote.hasCustomHats, false, false);
						profile3.team = Teams.all[(int)remote.team];
						profile3.networkStatus = foundStatusValue;
						profile3.flippers = remote.flippers;
						return;
					}
					Main.codeNumber = 133702;
					Profile profile4 = DuckNetwork.CreateProfile(joinConnection, remote.name, (int)remote.duckIndex, null, remote.hasCustomHats, false, false);
					profile4.flippers = remote.flippers;
					profile4.team = Teams.all[(int)remote.team];
					if (DuckNetwork._core.hostDuckIndex == -1)
					{
						DuckNetwork._core.hostDuckIndex = (int)remote.duckIndex;
					}
					Main.codeNumber = 133703;
					bool foundStatus2 = false;
					foreach (Profile p7 in DuckNetwork.GetProfiles(joinConnection))
					{
						if (p7 != profile4)
						{
							profile4.networkStatus = p7.networkStatus;
							foundStatus2 = true;
							break;
						}
					}
					Main.codeNumber = 133704;
					if (!foundStatus2)
					{
						profile4.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
						return;
					}
				}
			}
			else
			{
				if (m is NMEndOfDuckNetworkData)
				{
					Send.Message(new NMJoinedDuckNetwork(), m.connection);
					using (List<Profile>.Enumerator enumerator7 = DuckNetwork.profiles.GetEnumerator())
					{
						while (enumerator7.MoveNext())
						{
							Profile p8 = enumerator7.Current;
							if (p8.connection == DuckNetwork.localConnection)
							{
								Send.Message(new NMProfileInfo(p8.networkIndex, p8.stats.unloyalFans, p8.stats.loyalFans));
								if (p8.linkedProfile == Profiles.experienceProfile && Profiles.experienceProfile != null && Profiles.experienceProfile.furniturePositions.Count > 0)
								{
									Send.Message(new NMRoomData(Profiles.experienceProfile.furniturePositionData, p8.networkIndex), m.connection);
								}
							}
						}
						return;
					}
				}
				if (m is NMEndOfGhostData)
				{
					if ((m as NMEndOfGhostData).levelIndex == DuckNetwork.levelIndex)
					{
						DevConsole.Log(DCSection.DuckNet, "|DGGREEN|Received Host Level Information (" + (m as NMEndOfGhostData).levelIndex + ").", -1);
						Level.current.TransferComplete(m.connection);
						DuckNetwork.SendToEveryone(new NMLevelDataReady(DuckNetwork.levelIndex));
						using (List<Profile>.Enumerator enumerator8 = DuckNetwork.GetProfiles(DuckNetwork.localConnection).GetEnumerator())
						{
							while (enumerator8.MoveNext())
							{
								Profile p9 = enumerator8.Current;
								p9.connection.loadingStatus = (m as NMEndOfGhostData).levelIndex;
							}
							return;
						}
					}
					DevConsole.Log(DCSection.DuckNet, "|DGRED|Recieved data for wrong level.", -1);
					return;
				}
				if (m is NMSetTeam)
				{
					NMSetTeam t5 = m as NMSetTeam;
					if (t5.duck >= 0 && t5.duck < 4)
					{
						Profile p10 = DuckNetwork.profiles[(int)t5.duck];
						if (p10.connection != null && p10.team != null)
						{
							p10.team = Teams.all[(int)t5.team];
							return;
						}
					}
				}
				else if (m is NMRoomData)
				{
					NMRoomData t6 = m as NMRoomData;
					if (t6.duck >= 0 && t6.duck < 4)
					{
						Profile p11 = DuckNetwork.profiles[(int)t6.duck];
						if (p11.connection != null && p11.connection != DuckNetwork.localConnection)
						{
							p11.furniturePositionData = t6.data;
							return;
						}
					}
				}
				else if (m is NMTeamSetDenied)
				{
					NMTeamSetDenied t7 = m as NMTeamSetDenied;
					if (t7.duck >= 0 && t7.duck < 4)
					{
						Profile p12 = DuckNetwork.profiles[(int)t7.duck];
						if (p12.connection == DuckNetwork.localConnection && p12.team != null && Teams.all.IndexOf(p12.team) == (int)t7.team)
						{
							DuckNetwork.OpenTeamSwitchDialogue(p12);
						}
					}
				}
			}
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0009E328 File Offset: 0x0009C528
		public static void SendToEveryone(NetMessage m)
		{
			foreach (Profile p in DuckNetwork.profiles)
			{
				if (p.connection != null && p.connection != DuckNetwork.localConnection && (!p.isHost || (int)p.networkIndex == DuckNetwork.hostDuckIndex))
				{
					NetMessage msg = Activator.CreateInstance(m.GetType(), null) as NetMessage;
					Editor.CopyClass(m, msg);
					msg.ClearSerializedData();
					Send.Message(msg, NetMessagePriority.ReliableOrdered, p.connection);
				}
			}
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0009E3CC File Offset: 0x0009C5CC
		public static void Draw()
		{
			if (DuckNetwork._core.localDuckIndex == -1)
			{
				return;
			}
			Vec2 size = new Vec2(Layer.Console.width, Layer.Console.height);
			float yDraw = 0f;
			int numDraw = 8;
			DuckNetwork._chatFont.scale = new Vec2(2f);
			if (DuckNetwork._core.enteringText && !DuckNetwork._core.stopEnteringText)
			{
				DuckNetwork._core.cursorFlash++;
				if (DuckNetwork._core.cursorFlash > 30)
				{
					DuckNetwork._core.cursorFlash = 0;
				}
				bool flash = DuckNetwork._core.cursorFlash >= 15;
				Profile p = DuckNetwork.profiles[DuckNetwork._core.localDuckIndex];
				string fullString = p.name + ": " + DuckNetwork._core.currentEnterText;
				if (flash)
				{
					fullString += "_";
				}
				Vec2 messagePos = new Vec2(14f, yDraw + (size.y - 32f));
				Graphics.DrawRect(messagePos + new Vec2(-1f, -1f), messagePos + new Vec2(DuckNetwork._chatFont.GetWidth(fullString, false) + 4f, 20f) + new Vec2(1f, 1f), Color.Black * 0.6f, 0.7f, false, 1f);
				Color boxColor = Color.White;
				Color textColor = Color.Black;
				if (p.persona != null)
				{
					boxColor = p.persona.colorUsable;
				}
				Graphics.DrawRect(messagePos, messagePos + new Vec2(DuckNetwork._chatFont.GetWidth(fullString, false) + 4f, 20f), boxColor * 0.6f, 0.8f, true, 1f);
				DuckNetwork._chatFont.Draw(fullString, messagePos + new Vec2(2f, 2f), textColor, 1f, false);
				yDraw -= 22f;
			}
			float hatDepth = 1f;
			foreach (ChatMessage message in DuckNetwork._core.chatMessages)
			{
				string fullString2 = "|WHITE|" + message.who.name + ": |BLACK|" + message.text;
				float leftOffset = 20f;
				DuckNetwork._chatFont.scale = new Vec2(2f * message.scale);
				float stringWidth = DuckNetwork._chatFont.GetWidth(fullString2, false) + leftOffset;
				Vec2 messagePos2 = new Vec2(-((15f + stringWidth) * (1f - message.slide)) + 14f, yDraw + (size.y - 32f));
				Graphics.DrawRect(messagePos2 + new Vec2(-1f, -1f), messagePos2 + new Vec2(stringWidth + 4f, 20f) + new Vec2(1f, 1f), Color.Black * 0.6f * message.alpha, 0.7f, false, 1f);
				float extraScale = 0.3f + (float)message.text.Length * 0.007f;
				if (extraScale > 0.5f)
				{
					extraScale = 0.5f;
				}
				if (message.slide > 0.8f)
				{
					message.scale = Lerp.FloatSmooth(message.scale, 1f, 0.1f, 1.1f);
				}
				else if (message.slide > 0.5f)
				{
					message.scale = Lerp.FloatSmooth(message.scale, 1f + extraScale, 0.1f, 1.1f);
				}
				message.slide = Lerp.FloatSmooth(message.slide, 1f, 0.1f, 1.1f);
				Color boxColor2 = Color.White;
				Color textColor2 = Color.Black;
				if (message.who.persona != null)
				{
					boxColor2 = message.who.persona.colorUsable;
					SpriteMap hat = message.who.persona.defaultHead;
					Vec2 zero = Vec2.Zero;
					if (message.who.team != null && message.who.team.hasHat && (message.who.connection != DuckNetwork.localConnection || !message.who.team.locked))
					{
						Vec2 hatOffset = message.who.team.hatOffset;
						hat = message.who.team.hat;
					}
					hat.CenterOrigin();
					hat.depth = hatDepth;
					hat.alpha = message.alpha;
					hat.scale = new Vec2(2f, 2f);
					Graphics.Draw(hat, messagePos2.x, messagePos2.y);
					hat.scale = new Vec2(1f, 1f);
					hat.alpha = 1f;
					boxColor2 *= 0.85f;
					boxColor2.a = byte.MaxValue;
				}
				Graphics.DrawRect(messagePos2, messagePos2 + new Vec2(stringWidth + 4f, 20f), boxColor2 * 0.75f * message.alpha, 0.6f, true, 1f);
				DuckNetwork._chatFont.Draw(fullString2, messagePos2 + new Vec2(2f + leftOffset, 2f), textColor2 * message.alpha, 0.9f, false);
				yDraw -= 26f;
				hatDepth -= 0.01f;
				if (numDraw == 0)
				{
					break;
				}
				numDraw--;
			}
		}

		// Token: 0x04000EBE RID: 3774
		private static List<OnlineLevel> _levels = new List<OnlineLevel>
		{
			new OnlineLevel
			{
				num = 1,
				xpRequired = 0
			},
			new OnlineLevel
			{
				num = 2,
				xpRequired = 175
			},
			new OnlineLevel
			{
				num = 3,
				xpRequired = 400
			},
			new OnlineLevel
			{
				num = 4,
				xpRequired = 1200
			},
			new OnlineLevel
			{
				num = 5,
				xpRequired = 3500
			},
			new OnlineLevel
			{
				num = 6,
				xpRequired = 6500
			},
			new OnlineLevel
			{
				num = 7,
				xpRequired = 10000
			},
			new OnlineLevel
			{
				num = 8,
				xpRequired = 13000
			},
			new OnlineLevel
			{
				num = 9,
				xpRequired = 16000
			},
			new OnlineLevel
			{
				num = 10,
				xpRequired = 19000
			},
			new OnlineLevel
			{
				num = 11,
				xpRequired = 23000
			},
			new OnlineLevel
			{
				num = 12,
				xpRequired = 28000
			},
			new OnlineLevel
			{
				num = 13,
				xpRequired = 34000
			},
			new OnlineLevel
			{
				num = 14,
				xpRequired = 40000
			},
			new OnlineLevel
			{
				num = 15,
				xpRequired = 45000
			},
			new OnlineLevel
			{
				num = 16,
				xpRequired = 50000
			},
			new OnlineLevel
			{
				num = 17,
				xpRequired = 56000
			},
			new OnlineLevel
			{
				num = 18,
				xpRequired = 62000
			},
			new OnlineLevel
			{
				num = 19,
				xpRequired = 75000
			},
			new OnlineLevel
			{
				num = 20,
				xpRequired = 100000
			}
		};

		// Token: 0x04000EBF RID: 3775
		public static int kills;

		// Token: 0x04000EC0 RID: 3776
		public static int deaths;

		// Token: 0x04000EC1 RID: 3777
		private static DuckNetworkCore _core = new DuckNetworkCore();

		// Token: 0x04000EC2 RID: 3778
		private static FancyBitmapFont _chatFont;

		// Token: 0x04000EC3 RID: 3779
		private static UIMenu _ducknetMenu;

		// Token: 0x04000EC4 RID: 3780
		private static UIMenu _optionsMenu;

		// Token: 0x04000EC5 RID: 3781
		private static UIMenu _confirmMenu;

		// Token: 0x04000EC6 RID: 3782
		private static UIMenu _confirmKick;

		// Token: 0x04000EC7 RID: 3783
		private static MenuBoolean _quit = new MenuBoolean();

		// Token: 0x04000EC8 RID: 3784
		private static MenuBoolean _menuClosed = new MenuBoolean();

		// Token: 0x04000EC9 RID: 3785
		public static int numSlots = 4;

		// Token: 0x04000ECA RID: 3786
		public static Profile kickContext;

		// Token: 0x04000ECB RID: 3787
		public static List<ulong> _invitedFriends = new List<ulong>();

		// Token: 0x04000ECC RID: 3788
		private static MenuBoolean _inviteFriends = new MenuBoolean();

		// Token: 0x04000ECD RID: 3789
		private static UIMenu _inviteMenu;

		// Token: 0x04000ECE RID: 3790
		private static UIMenu _slotEditor;

		// Token: 0x04000ECF RID: 3791
		private static UIMenu _matchSettingMenu;

		// Token: 0x04000ED0 RID: 3792
		private static UIMenu _matchModifierMenu;

		// Token: 0x04000ED1 RID: 3793
		private static UIComponent _noModsUIGroup;

		// Token: 0x04000ED2 RID: 3794
		private static UIMenu _noModsMenu;

		// Token: 0x04000ED3 RID: 3795
		private static UIComponent _restartModsUIGroup;

		// Token: 0x04000ED4 RID: 3796
		private static UIMenu _restartModsMenu;

		// Token: 0x04000ED5 RID: 3797
		private static bool _pauseOpen = false;

		// Token: 0x04000ED6 RID: 3798
		private static string _settingsBeforeOpen = "";

		// Token: 0x04000ED7 RID: 3799
		private static bool _willOpenSettingsInfo = false;

		// Token: 0x04000ED8 RID: 3800
		private static UIMenu _levelSelectMenu;

		// Token: 0x04000ED9 RID: 3801
		private static Profile _menuOpenProfile;

		// Token: 0x04000EDA RID: 3802
		private static UIComponent _uhOhGroup;

		// Token: 0x04000EDB RID: 3803
		private static UIMenu _uhOhMenu;
	}
}
