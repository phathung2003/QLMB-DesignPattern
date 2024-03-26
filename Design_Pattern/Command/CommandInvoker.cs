using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.Command
{
    public class CommandInvoker
    {
        private ICommand _command;

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void ExecuteCommand()
        {
            _command.Execute();
        }
    }
}