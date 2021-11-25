using System;
using Steamworks;

namespace Backend {
	public class BWSteamUtility {
		
		public const int APP_ID = 1828350;
		
		public static ulong GetCurrentUserID() {
			return SteamUser.GetSteamID().m_SteamID;
		}
	}
}