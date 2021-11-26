using System.IO;
using Backend.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Submittable {
	public class SteamSubmittable {

		public string title = "";
		public string description = "";
		public ISteamSerializable obj;
		public string savePath = "";

		public SteamSubmittable([NotNull]string title, [NotNull]string description, ISteamSerializable obj) {
			this.title = title;
			this.description = description;
			this.obj = obj;
		}

		public void SaveLocal() {
			obj.Save();
			string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
			savePath = GetSaveFolderPath();
			Directory.CreateDirectory(savePath);
			File.WriteAllText(savePath+"/"+title+"."+obj.fileExtension, json);	
		}

		public string GetSaveFolderPath() {
			return Application.persistentDataPath + "/" + obj.saveFolderName;
		}

	}  
}