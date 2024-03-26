using System.Web.Mvc;
namespace QLMB.Design_Pattern.Command
{
    public class CommandInvoker
    {
        private ICommand command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public ActionResult ExecuteCommand()
        {
            return command.Execute();
        }
    }
}