using System.Web.Mvc;
namespace QLMB.Design_Pattern.Command
{
    public interface ICommand
    {
        ActionResult Execute();
    }
}
