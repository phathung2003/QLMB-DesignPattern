using QLMB.Design_Pattern.Prototype.Interface;
using QLMB.Models;
namespace QLMB.Design_Pattern.Prototype.ConcretePrototype
{
    public class ConcreteClonePost : IEventSalePost
    {
        public SuKienUuDai info {  get; set; }
        public IEventSalePost Clone()
        {
            ConcreteClonePost cloneInfo = (ConcreteClonePost)this.MemberwiseClone();    
            cloneInfo.info = this.info;
            return cloneInfo;
        }
    }
}