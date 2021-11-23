using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Backend.Level;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BWConsole : MonoBehaviour {

	[Header("Console Config")]
	[SerializeField] private BWConsoleConfig config;
	
	[Header("GUI Components")]
	[SerializeField] private Transform outputContainer;
	[SerializeField] private GameObject outputLinePrefab;
	private Queue<BWConsole_OutputLine> outputLines;
	[SerializeField] private int maxLines;
	
	[SerializeField] private TMP_InputField inputField;


	public void ProcessInput() {
		config.Initialise(this);
		
		string inputText = inputField.text;
		
		Print("> "+inputText);

		// Get the method name
		string regex = ".+(?=\\()";
		string methodName = Regex.Match(inputText, regex).Value;

		if (methodName.Equals(String.Empty)) {
			PrintError($"Error with input: \"{inputText}\", please use format: methodName(parameter1,parameter2,...)");
			return;
		}
		
		// Get the method parameters
		regex = "(?<=\\().+(?=,)|(?<=\\().+(?=\\))|(?<=\\s).+(?=,)|(?<=\\s).+(?=\\))";
		MatchCollection matches = Regex.Matches(inputText, regex);

		List<string> parameters = new List<string>();
		
		foreach (Match match in matches) {
			parameters.Add(match.Value);
		}
		
		CallMethod(methodName, parameters.ToArray());
	}

	private void CallMethod(string methodName, string[] parameters) {
		// First find the method in the console config
		BWConsole_Action bwaction = null;

		config.actionDictionary.TryGetValue(methodName, out bwaction);

		if (bwaction == null) {
			PrintError($"No action with name: {methodName} available");
			return;
		}

		MethodInfo action = GetMethodInfoFromBWAction(bwaction);

		ParameterInfo[] parameterInfo = action.GetParameters();
		
		string parameterTypeString = "";
		// Check the types of the parameters
		foreach (ParameterInfo info in parameterInfo) {
			parameterTypeString += info.ParameterType +",";
		}

		// Check the length of the parameters supplied and create a string for errors
		if (parameterTypeString.Length > 0) {
			parameterTypeString = parameterTypeString.Substring(0, parameterTypeString.Length - 1);
		}

		if (parameterInfo.Length != parameters.Length) {
			PrintError($"Not enough parameters supplied for: {methodName}({parameterTypeString})");
			return;
		}
		
		// Now cast the given parameters
		object[] parameterCasts = new object[parameters.Length];
		for (int i = 0; i < parameters.Length; i++) {
			object cast = CastParameter(parameters[i], parameterInfo[i]);
			if (cast == null) {
				PrintError($"Invalid parameter provided for: {methodName}{parameterTypeString}");
				return;
			}
			parameterCasts[i] = cast;
		}
		
		// Invoke the method
		action.Invoke(GetTargetFromBWAction(bwaction), parameterCasts);
	}

	private MethodInfo GetMethodInfoFromBWAction(BWConsole_Action action) {
		switch (action.actionType) {
			case BWConsole_ActionType.Int: return action.action_int.Method;
			case BWConsole_ActionType.Int_Int: return action.action_int_int.Method;
			case BWConsole_ActionType.String: return action.action_string.Method;
			case BWConsole_ActionType.Void: return action.action_void.Method;
			case BWConsole_ActionType.Bool: return action.action_bool.Method;
			case BWConsole_ActionType.Float: return action.action_float.Method;
		}

		return null;
	}

	private object GetTargetFromBWAction(BWConsole_Action action) {
		switch (action.actionType) {
			case BWConsole_ActionType.Int: return action.action_int.Target;
			case BWConsole_ActionType.Int_Int: return action.action_int_int.Target;
			case BWConsole_ActionType.String: return action.action_string.Target;
			case BWConsole_ActionType.Void: return action.action_void.Target;
			case BWConsole_ActionType.Bool: return action.action_bool.Target;
			case BWConsole_ActionType.Float: return action.action_float.Target;
		}

		return null;
	}

	private object CastParameter(string parameter, ParameterInfo parameterInfo) {
		object output = null;

		try {
			switch (Type.GetTypeCode(parameterInfo.ParameterType)) {
				case TypeCode.Boolean: output = Boolean.Parse(parameter); break;
				case TypeCode.String: output = parameter; break;
				case TypeCode.Int32: output = Int32.Parse(parameter); break;
				case TypeCode.Decimal: output = (float)Decimal.Parse(parameter); break;
			}	
		} catch {
			return null;
		}

		return output;
	}

	private BWConsole_OutputLine PrintAndOutputLine(string output) {
		BWConsole_OutputLine line = GenerateTextLine();
		line.SetText(output);
		return line;
	}

	public void Print(string output) {
		PrintAndOutputLine(output);
	}

	public BWConsole_OutputLine PrintError(string output) {
		BWConsole_OutputLine line = PrintAndOutputLine(output);
		line.SetTextColor(Color.red);
		return line;
	}

	private BWConsole_OutputLine GenerateTextLine() {
		if (outputLines == null) {
			outputLines = new Queue<BWConsole_OutputLine>();
		}
		
		if (outputLines.Count >= maxLines) {
			BWConsole_OutputLine line = outputLines.Dequeue();
			line.Reset();
			line.transform.SetAsFirstSibling();
			outputLines.Enqueue(line);
			return line;
		}
		
		// Instantiate a new line
		BWConsole_OutputLine output = Instantiate(outputLinePrefab, outputContainer).GetComponent<BWConsole_OutputLine>();
		outputLines.Enqueue(output);

		output.Reset();

		return output;
	}
	

	public void Clear() {
		while (outputLines.Count > 0) {
			Destroy(outputLines.Dequeue().gameObject);
		}
	}

}
