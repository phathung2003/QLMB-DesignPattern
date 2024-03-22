using QLMB.Design_Pattern.factory;
using QLMB.Design_Pattern.Factory;
namespace QLMB.Design_Pattern.Factory_Method.ConcreteCreator
{
    public class ConcreteCreatorRentail : CreatorLoginChecker
    {
        public override ILoginChecker GetLoginChecker()
        {
            return new ConcreteRentalLoginChecker();
        }
    }
}