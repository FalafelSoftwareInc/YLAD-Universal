using System;

namespace YLADRT.Core.Commands
{
	/// <summary>
	/// A convenient class of a relay command that takes no parameters.
	/// </summary>
	public sealed class RelayCommand : RelayCommand<object>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="commandAction">The action to relay the command to.</param>
		public RelayCommand(Action commandAction) : base(o => commandAction())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="commandAction">The action to relay the command to.</param>
		/// <param name="canExecuteFunc">The function to execute to determine whether the command can be executed or not.</param>
		public RelayCommand(Action commandAction, Func<bool> canExecuteFunc) : base(o => commandAction(), o => canExecuteFunc())
		{
		}
	}
}