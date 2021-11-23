using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Events;

namespace Backend.Level {
	[Serializable]
	public class BWConsoleConfig {
		
		//public BWConsole_Action[] actions;
		
		public Dictionary<string, BWConsole_Action> actionDictionary;

		private bool initialised;
		private BWConsole console;
		
		public void Initialise(BWConsole console) {
			if (initialised) {
				return;
			}
			
			this.console = console;
			actionDictionary = new Dictionary<string, BWConsole_Action>();
			
			// Generate a dictionary from the supplied actions
			/*foreach (BWConsole_Action action in actions) {
				actionDictionary.Add(action.command, action.action);
			}*/
			
			GenerateBasicCommands();
			
			initialised = true;
		}

		private void GenerateBasicCommands() {
			// Generate clear command
			actionDictionary.Add("clear", new BWConsole_Action(console.Clear));
			
			// Generate print command
			actionDictionary.Add("print", new BWConsole_Action(console.Print));
			
			// Generate test int int command
			actionDictionary.Add("multiply", new BWConsole_Action(TestIntInt));
		}

		public void TestIntInt(int test1, int test2) {
			console.Print(""+test1*test2);
		}

	}

	[Serializable]
	public class BWConsole_Action {

		public UnityAction action_void;
		public UnityAction<string> action_string;
		public UnityAction<int> action_int;
		public UnityAction<int, int> action_int_int;
		public UnityAction<bool> action_bool;
		public UnityAction<float> action_float;

		public BWConsole_ActionType actionType;
		
		public BWConsole_Action(UnityAction action) {
			this.actionType = BWConsole_ActionType.Void;
			this.action_void = action;
		}
		
		public BWConsole_Action(UnityAction<string> action) {
			this.actionType = BWConsole_ActionType.String;
			this.action_string = action;
		}
		
		public BWConsole_Action(UnityAction<int> action) {
			this.actionType = BWConsole_ActionType.Int;
			this.action_int = action;
		}
		
		public BWConsole_Action(UnityAction<int, int> action) {
			this.actionType = BWConsole_ActionType.Int_Int;
			this.action_int_int = action;
		}
		
		public BWConsole_Action(UnityAction<bool> action) {
			this.actionType = BWConsole_ActionType.Bool;
			this.action_bool = action;
		}
		
		public BWConsole_Action(UnityAction<float> action) {
			this.actionType = BWConsole_ActionType.Float;
			this.action_float = action;
		}
	}

	public enum BWConsole_ActionType {
		Void, String, Bool, Int, Int_Int, Float
	}
	
}