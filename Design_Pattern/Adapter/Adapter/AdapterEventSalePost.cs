using QLMB.Design_Pattern.Adapter.Adaptee;
using QLMB.Design_Pattern.Adapter.Interface;
using QLMB.Design_Pattern.Prototype.ConcretePrototype;
using QLMB.Models;
namespace QLMB.Design_Pattern.Adapter.Adapter
{
    public class AdapterEventSalePost : IConvertPost

    {
        private AdapteeChangeFormat adaptee;
        public AdapterEventSalePost(AdapteeChangeFormat adaptee)
        {
            this.adaptee = adaptee;
        }
        public SuKienUuDai ConvertToSKUD(ConcreteClonePost concreteClonePost)
        {
            return adaptee.ChangeToSKUD(concreteClonePost);
        }
    }
}