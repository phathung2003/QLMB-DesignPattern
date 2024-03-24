using QLMB.Design_Pattern.Facade;
using System;
using System.Web.Mvc;

public abstract class ControllerTemplate : Controller
{
    protected abstract bool checkRole();
    protected abstract void ExecuteAction();
    protected abstract ActionResult HandleUnauthorizedAccess();
    protected abstract ActionResult HandleException(Exception ex);
 


    //Template Method
    public ActionResult TemplateMethod()
    {
        try
        {
            if (checkRole())
            {
                  ExecuteAction();
                return null;
            }
            else
            {
                return HandleUnauthorizedAccess();
            }
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}   