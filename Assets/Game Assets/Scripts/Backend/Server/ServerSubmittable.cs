using System;
using System.IO;
using Backend.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Submittable {
	public class ServerSubmittable {
		
		public IServerSerializable obj;
		public string savePath { get; private set; }

		public ServerSubmittable(IServerSerializable obj) {
			this.obj = obj;
		}

		public string GetJson() {
			obj.Save();
			string json = JsonConvert.SerializeObject(obj);
			return json;
		}

	}  
}