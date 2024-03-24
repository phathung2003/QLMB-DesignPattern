using QLMB.Design_Pattern.Proxy.Interface;
using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Proxy.Proxy
{
    // Assuming IRegister interface is public
    public class RegisterProxy : IRegister
    {
        private IRegister registerService;

        public RegisterProxy(IRegister registerService)
        {
            this.registerService = registerService;
        }

        public bool UserRegister(ThongTinND info, string username, string password, string rePassword, ModelStateDictionary modelState)
        {
            if(checkInfo(info, username, password, rePassword, modelState))
            {
                return registerService.UserRegister(info, username, password, rePassword, modelState);
            }
            return false;
        }

        //Kiểm tra thông tin -- | [Strategy Pattern] | --
        private bool checkInfo(ThongTinND info, string username, string password, string rePassword, ModelStateDictionary modelState)
        {
            ContextStrategy checkResult;

            //CMND
            checkResult = new ContextStrategy(new ConcreteCMND(modelState, "CMND", info.CMND));
            checkResult.GetResult();

            //Ngày cấp
            checkResult.SetStrategy(new ConcreteIssuanceDate(modelState, "NgayCapCMND", info.NgayCap));
            checkResult.GetResult();

            //Họ tên
            checkResult.SetStrategy(new ConcreteName(modelState, "HoTen", info.HoTen));
            checkResult.GetResult();

            //Giới tính
            checkResult.SetStrategy(new ConcreteGender(modelState, "GioiTinh", info.GioiTinh));
            checkResult.GetResult();

            //Ngày sinh
            checkResult.SetStrategy(new ConcreteBirthday(modelState, "NgaySinhND", info.NgaySinh, true));
            checkResult.GetResult();

            //Địa chỉ
            checkResult.SetStrategy(new ConcreteAddress(modelState, "DiaChi", info.DiaChi));
            checkResult.GetResult();

            //Tên đăng nhập
            checkResult.SetStrategy(new ConcreteUsername(modelState, "TenDangNhap", username, true));
            checkResult.GetResult();

            //Mật khẩu
            checkResult.SetStrategy(new ConcretePassword(modelState, "MatKhau", password));
            checkResult.GetResult();

            //Nhập lại mật khẩu
            checkResult.SetStrategy(new ConcreteRePassword(modelState, "MatKhauLai", password, rePassword));
            checkResult.GetResult();

            return checkResult.noError;
        }
    }
}
