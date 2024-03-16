using QLMB.Design_Pattern.Strategy.Interface;
namespace QLMB.Design_Pattern.Strategy.Context
{
    public class ContextStrategy
    {
        public bool noError {  get; private set; } = true;

        public IValidation strategy {  get; set; }

        public ContextStrategy(IValidation strategy)
        {
            this.strategy = strategy;
        }

        public void GetResult()
        {
            if (!strategy.Result())
            {
                noError = false;
            }
        }
    }
}