namespace QLMB.Design_Pattern.Factory
{
    public interface ILoginChecker
    {
        bool CheckLogin(string username, string password);
    }
}
