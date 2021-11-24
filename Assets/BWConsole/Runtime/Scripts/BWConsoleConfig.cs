using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Level {
	[Serializable]
	public class BWConsoleConfig {
		
		//public BWConsole_Action[] actions;
		[Header("Custom Commands")]
		public UnityEvent InitialiseCustomCommandsMethod;
		public Dictionary<string, BWConsole_Action> actionDictionary;

		private bool initialised;
		private BWConsole console;
		
		public void Initialise(BWConsole console) {
			if (initialised) {
				return;
			}
			
			this.console = console;
			actionDictionary = new Dictionary<string, BWConsole_Action>();

			// Initialise the basic commands class
			BWConsole_BasicCommands basicCommands = new BWConsole_BasicCommands(console);

			// Check if the player has supplied custom commands
			if (InitialiseCustomCommandsMethod.GetPersistentEventCount() <= 0) { 
				console.PrintWarning(
					"No method supplied for initialising custom commands. Ensure this is supplied under \"Initialise Custom Commands Method\" on console GameObject.");
			} else {
				InitialiseCustomCommandsMethod.Invoke();
			}
			
			initialised = true;
		}

		public void AddAction(string command, BWConsole_Action action) {
			command = command.ToLower();
			actionDictionary.Add(command, action);
		}
	}

	[Serializable]
	public class BWConsole_Action {

		public UnityAction action_void;
		public UnityAction<string> action_string;
		public UnityAction<int> action_int;
		public UnityAction<int, int> action_int_int;
		public UnityAction<int, int, int> action_int_int_int;
		public UnityAction<bool> action_bool;
		public UnityAction<float> action_float;

		public BWConsole_ActionType actionType;
		
		public BWConsole_Action(UnityAction action) {
			actionType = BWConsole_ActionType.Void;
			action_void = action;
		}
		
		public BWConsole_Action(UnityAction<string> action) {
			actionType = BWConsole_ActionType.String;
			action_string = action;
		}
		
		public BWConsole_Action(UnityAction<int> action) {
			actionType = BWConsole_ActionType.Int;
			action_int = action;
		}
		
		public BWConsole_Action(UnityAction<int, int> action) {
			actionType = BWConsole_ActionType.Int_Int;
			action_int_int = action;
		}
		
		public BWConsole_Action(UnityAction<int, int, int> action) {
			actionType = BWConsole_ActionType.Int_Int_Int;
			action_int_int_int = action;
		}
	}

	public enum BWConsole_ActionType {
		Void, String, Bool, Int, Int_Int, Int_Int_Int, Float
	}
	
}