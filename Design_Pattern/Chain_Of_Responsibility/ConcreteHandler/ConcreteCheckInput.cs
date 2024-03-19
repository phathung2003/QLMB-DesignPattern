using QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler;
using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler
{
    public class ConcreteCheckInput : BaseHandlerEmployeeRegister
    {
        private ContextStrategy checkResult;
        
        //Kiểm tra thông tin đăng ký - [Strategy Pattern]
        public override (bool, string, PhraseType) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
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

            //Chức vụ
            checkResult.SetStrategy(new ConcreteRole(modelState, "ChonChucVu", role.MaChucVu));
            checkResult.GetResult();

            if (checkResult.noError)
            {
                return base.HandleRequest(info, role, modelState);
            }
            return (false, role.MaChucVu, PhraseType.CheckInput);
        }
    }
}