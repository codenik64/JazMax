using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class CoreProvinceService
    {
        private static JazMax.AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();

        public List<CoreProvinceView> GetAll()
        {
            var query = from t in db.CoreProvinces
                        where t.IsActive == true
                        select t;

            return ConvertListToView(query.ToList());
        }

        public void Create(CoreProvinceView view)
        {
            try
            {
                db.CoreProvinces.Add(ConvertToView(view));
                db.SaveChanges();
            }
            catch
            {
               
            }
        }

        public CoreProvinceView FindById(int Id)
        {
            var query = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == Id);

            return ConvertModelToView(query);
        }

        #region Helpers
        private List<CoreProvinceView> ConvertListToView(List<AzureDataAccess.CoreProvince> model)
        {
            return model.Select(t => new CoreProvinceView
            {
                IsActive = t.IsActive,
                IsAssigned = t.IsAssigned,
                ProvinceId = t.ProvinceId,
                ProvinceName = t.ProvinceName

            }).ToList();
        }

        private CoreProvinceView ConvertModelToView(AzureDataAccess.CoreProvince v)
        {
            CoreProvinceView model = new CoreProvinceView();
            model.IsActive = v.IsActive;
            model.IsAssigned = v.IsAssigned;
            model.ProvinceId = v.ProvinceId;
            model.ProvinceName = v.ProvinceName;
            return model;
        }

        private AzureDataAccess.CoreProvince ConvertToView(CoreProvinceView v)
        {
            AzureDataAccess.CoreProvince model = new AzureDataAccess.CoreProvince();
            model.IsActive = v.IsActive;
            model.IsAssigned = v.IsAssigned;
            model.ProvinceId = v.ProvinceId;
            model.ProvinceName = v.ProvinceName;
            return model;
        }
        #endregion
    }
}
