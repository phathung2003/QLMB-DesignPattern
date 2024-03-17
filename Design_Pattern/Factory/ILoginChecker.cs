using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLMB.Design_Pattern.Factory
{
    public interface ILoginChecker
    {
        bool CheckLogin(string username, string password);
    }
}
