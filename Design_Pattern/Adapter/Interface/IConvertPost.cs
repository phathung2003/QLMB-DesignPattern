using QLMB.Design_Pattern.Prototype.ConcretePrototype;
using QLMB.Models;
namespace QLMB.Design_Pattern.Adapter.Interface
{
    public interface IConvertPost
    {
        SuKienUuDai ConvertToSKUD(ConcreteClonePost concreteClonePost);
    }
}
