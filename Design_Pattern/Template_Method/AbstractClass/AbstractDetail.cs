using System.Web.Mvc;
public abstract class AbstractDetail : Controller
{
    public abstract bool CheckAllowAccess();
    public abstract ActionResult DetailPage(string data);
    
    //Template Method
    public ActionResult DetailTemplateMethod(string data)
    {
        try
        {
            if (CheckAllowAccess())
            {
                return DetailPage(data);
            }
            //Không thoả --> Về trang xử lý chuyển trang
            return RedirectToAction("Manager", "Account");
        }
        //Lỗi xử lý --> Skill Issue :))
        catch { return RedirectToAction("Index", "SkillIssue"); }
    }
}   