using QLMB.Design_Pattern.Prototype.ConcretePrototype;
using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.Adapter.Adaptee
{
    public class AdapteeChangeFormat
    {
        public SuKienUuDai ChangeToSKUD(ConcreteClonePost concreteClonePost)
        {
            SuKienUuDai suKienUuDai = concreteClonePost.info;
            return suKienUuDai;
        }
    }
}