using System;
using System.Windows.Input;

namespace Chroma.FuelCell.GatewayConnector
{
    public class CommandHandler : ICommand
    {
        Action execute;
        Action<object> exepara;
        Func<bool> isCanexecute;

        public CommandHandler(Action exe) : this(exe, null) { }

        public CommandHandler(Action exe, Func<bool> isCanexe)
        {
            if (exe == null)
                throw new ArgumentNullException("Command Action Error!");
            execute = exe;
            isCanexecute = isCanexe;
        }

        public CommandHandler(Action<object> exe) : this(exe, null) { }


        public CommandHandler(Action<object> exe, Func<bool> isCanexe)
        {
            if (exe == null)
                throw new ArgumentNullException("Command Action Error!");
            exepara = exe;
            isCanexecute = isCanexe;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (execute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (execute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return isCanexecute == null ? true : isCanexecute();
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
                execute();
            else
                exepara(parameter);
        }
    }
}
