using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows.Input;

namespace YLADRT.Core.Commands
{
	/// <summary>
	/// A simple relay command implementation of <c>ICommand</c>.
	/// </summary>
	/// <typeparam name="T">The type of the command parameter.</typeparam>
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _commandAction;
		private readonly Func<T, bool> _canExecuteFunc;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
		/// </summary>
		/// <param name="commandAction">The action to relay the command to.</param>
		public RelayCommand(Action<T> commandAction)
		{
			_commandAction = commandAction;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
		/// </summary>
		/// <param name="commandAction">The action to relay the command to.</param>
		/// <param name="canExecuteFunc">The function to execute to determine whether the command can be executed or not.</param>
		public RelayCommand(Action<T> commandAction, Func<T, bool> canExecuteFunc)
		{
			_commandAction = commandAction;
			_canExecuteFunc = canExecuteFunc;
		}

		/// <summary>
		/// Can be used to force a re-evaluation of the <c>CanExecute</c> method.
		/// </summary>
		[SuppressMessage("Microsoft.Design",
			"CA1030:UseEventsWhereAppropriate",
			Justification = "This intentionally is created that way, to enable raising the event from the outside.")]
		public void RaiseCanExecuteChanged()
		{
			var handlers = CanExecuteChanged;
			if (handlers != null)
			{
				handlers(this, EventArgs.Empty);
			}
		}

		#region ICommand implementation

		/// <summary>
		/// Gets a value indicating whether the command can be executed or not.
		/// </summary>
		/// <param name="parameter">The data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		public bool CanExecute(object parameter)
		{
			if (_canExecuteFunc == null)
			{
				return true;
			}

			var typedParameter = GetParameter(parameter);
			return _canExecuteFunc(typedParameter);
		}

		/// <summary>
		/// Occurs when the can execute state of the command has changed.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Executes the command with the given parameter.
		/// </summary>
		/// <param name="parameter">The data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			if (_commandAction == null)
			{
				return;
			}

			// we try to do some simple parsing and conversion
			var typedParameter = GetParameter(parameter);
			_commandAction(typedParameter);
		}

		#endregion

		private static T GetParameter(object parameter)
		{
			if (parameter == null)
			{
				return default(T);
			}

			// get the type of the parameter
			var type = typeof(T);

			// if we have identical types, simply cast the parameter
			if (parameter.GetType() == type)
			{
				return (T)parameter;
			}

			// if the type is an enum, try to parse the enum value
			if (type.GetType().GetTypeInfo().IsEnum)
			{
				return (T)Enum.Parse(type, parameter.ToString(), true);
			}

			// ok, we cannot use the input parameter
			throw new ArgumentException("Input type not supported.", "parameter");
		}
	}
}