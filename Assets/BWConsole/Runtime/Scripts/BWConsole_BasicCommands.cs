using System.Runtime.CompilerServices;

namespace Backend.Level {
	public class BWConsole_BasicCommands {
		private BWConsole console;
		private BWConsoleConfig consoleConfig;
		
		public BWConsole_BasicCommands(BWConsole console) {
			this.console = console;
			consoleConfig = console.config;
			InitialiseCommands();
		}
		
		public void InitialiseCommands() {
			consoleConfig.AddAction("print", new BWConsole_Action(console.Print));
			consoleConfig.AddAction("multiply", new BWConsole_Action(Multiply));
			consoleConfig.AddAction("clear", new BWConsole_Action(console.Clear));
			consoleConfig.AddAction("close", new BWConsole_Action(console.Close));
		}

		private void Multiply(int value1, int value2) {
			console.Print(""+value1*value2);
		}
	}
}