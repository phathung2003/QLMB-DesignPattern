using QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler;
using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler
{
    public class ConcreteCheckInput : BaseEmployeeRegisterHandler
    {
        public override (bool, string) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            ContextStrategy checkResult;

            //CMND
            checkResult = new ContextStrategy(new ConcreteCMND(modelState, "CMND", info.CMND));
            checkResult.GetResult();

            //Ngày cấp
            checkResult.strategy = new ConcreteIssuanceDate(modelState, "NgayCapCMND", info.NgayCap);
            checkResult.GetResult();

            //Họ tên
            checkResult.strategy = new ConcreteName(modelState, "HoTen", info.HoTen);
            checkResult.GetResult();

            //Giới tính
            checkResult.strategy = new ConcreteGender(modelState, "GioiTinh", info.GioiTinh);
            checkResult.GetResult();

            //Ngày sinh
            checkResult.strategy = new ConcreteBirthday(modelState, "NgaySinhND", info.NgaySinh, true);
            checkResult.GetResult();

            //Địa chỉ
            checkResult.strategy = new ConcreteAddress(modelState, "DiaChi", info.DiaChi);
            checkResult.GetResult();

            //Chức vụ
            checkResult.strategy = new ConcreteRole(modelState, "ChonChucVu", role.MaChucVu);
            checkResult.GetResult();

            if (checkResult.noError)
            {
                return base.HandleRequest(info, role, modelState);
            }

            return (false, role.MaChucVu);
        }
    }
}