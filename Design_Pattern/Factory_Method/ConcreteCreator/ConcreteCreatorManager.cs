using QLMB.Design_Pattern.factory;
using QLMB.Design_Pattern.Factory;
namespace QLMB.Design_Pattern.Factory_Method.ConcreteCreator
{
    public class ConcreteCreatorManager : CreatorLoginChecker
    {
        public override ILoginChecker GetLoginChecker()
        {
            return new ConcreteManagerLoginChecker();
        }
    }
}