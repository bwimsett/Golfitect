namespace Game_Assets.Scripts.Backend.Server {
	public class DBObject {
		public string _id;
		public DBUser user;
		public string name;
		public int par;
		public int creationTime;

		public override bool Equals(object obj) {
			if (obj is DBObject dbObject) {
				return dbObject._id.Equals(_id);
			}

			return false;
		}
	}
}