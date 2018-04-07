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
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

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
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
            }
        }

        public CoreProvinceView FindById(int Id)
        {
            var query = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == Id);

            return ConvertModelToView(query);
        }

        #region Helpers
        private List<CoreProvinceView> ConvertListToView(List<DataAccess.CoreProvince> model)
        {
            return model.Select(t => new CoreProvinceView
            {
                IsActive = t.IsActive,
                IsAssigned = t.IsAssigned,
                ProvinceId = t.ProvinceId,
                ProvinceName = t.ProvinceName

            }).ToList();
        }

        private CoreProvinceView ConvertModelToView(DataAccess.CoreProvince v)
        {
            CoreProvinceView model = new CoreProvinceView()
            {
                IsActive = v.IsActive,
                IsAssigned = v.IsAssigned,
                ProvinceId = v.ProvinceId,
                ProvinceName = v.ProvinceName
            };
            return model;
        }

        private DataAccess.CoreProvince ConvertToView(CoreProvinceView v)
        {
            DataAccess.CoreProvince model = new DataAccess.CoreProvince()
            {
                IsActive = v.IsActive,
                IsAssigned = v.IsAssigned,
                ProvinceId = v.ProvinceId,
                ProvinceName = v.ProvinceName
            };
            return model;
        }
        #endregion
    }
}
