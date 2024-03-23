using System;
using System.Web.Mvc;

public abstract class ControllerTemplate : Controller
{
    protected abstract bool checkRole();
    protected abstract ActionResult HandleException(Exception ex);
   
    //Template Method
    public ActionResult ExecuteAction(Func<ActionResult> action)
    {
        try
        {
            if (checkRole())
            {
                return action();
            }
            else
            {
                return RedirectToAction("Manager", "Account");
            }
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}