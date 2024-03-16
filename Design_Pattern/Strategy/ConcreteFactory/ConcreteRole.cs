using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteFactory
{
    public class ConcreteRole : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string role;
        private string editRole;
        public ConcreteRole(ModelStateDictionary modelState, string key, string role)
        {
            this.modelState = modelState;
            this.key = key;
            this.role = role;
        }
        public ConcreteRole(ModelStateDictionary modelState, string key, string currentRole, string editRole)
        {
            this.modelState = modelState;
            this.key = key;
            this.role = currentRole;
            this.editRole = editRole;
        }

        public bool Result ()
        {
            if(string.IsNullOrEmpty(editRole))
            {
                return RoleCheck(role);
            }
            else
            {
                if(role == editRole)
                {
                    return true;
                }

                return RoleCheck(editRole);
            }
            
        }

        private bool RoleCheck(string role)
        {
            if (string.IsNullOrEmpty(role) || role.Trim() == "Default")
            {
                modelState.AddModelError(key, "* Xin hãy chọn chức vụ");
                return false;
            }

            return true;
        }
    }
}