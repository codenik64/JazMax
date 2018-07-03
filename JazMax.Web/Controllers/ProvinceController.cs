using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Web.Models;
using JazMax.Core.SystemHelpers;
using JazMax.BusinessLogic.Cache;
using System.Runtime.Caching;

namespace JazMax.Web.Controllers
{
    public class ProvinceController : Controller
    {
        private static CoreProvinceService obj = new CoreProvinceService();

        #region Get All 
        [JazMaxIdentity(UserGroup = "TeamLeader,CEO")]
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }
        #endregion

        #region Create Province
        public ActionResult Create()
        {
            return View();
        }

        public JsonResult CreateValue(string txtProvince)
        {
            try
            {
                JazMax.Web.ViewModel.UserAccountView.CoreProvinceView model = new ViewModel.UserAccountView.CoreProvinceView()
                {
                    ProvinceName = txtProvince,
                    IsActive = true,
                    IsAssigned = false
                };

                if (obj.Create(model) == false)
                {
                    return Json(new JazMaxJsonHelper
                    {
                        Result = Common.Models.JsonResult.Success,
                        Message = Common.Models.JsonMessage.Saved,
                    });
                }
                else
                {
                    return Json(new JazMaxJsonHelper
                    {
                        Result = Common.Models.JsonResult.Exists,
                        Message = "Error, Province Already exists",
                    });
                }
            }
            catch
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                    Message = Common.Models.JsonMessage.Error,
                });
            }
        }
        #endregion

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(CoreProvinceService.GetProvinceDetailsNew((int)id));
        }


        public JsonResult UpdateProvince(string ProvinceName, string ProvinceId)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreProvinceService.Update(ProvinceName, JazMaxIdentityHelper.GetCoreUserId(), Convert.ToInt32(ProvinceId));
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Activate(string ProvinceId)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreProvinceService.DeactiveCoreProvince(Convert.ToInt32(ProvinceId), JazMaxIdentityHelper.GetCoreUserId(), false);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Deactivate(string ProvinceId)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreProvinceService.DeactiveCoreProvince(Convert.ToInt32(ProvinceId), JazMaxIdentityHelper.GetCoreUserId(), true);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckCount()
        {
            if (obj.CheckCount() == false)
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Success,
                });
            }
            else
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                });
            }
        }

        public JsonResult ProvinceCount()
        {
            return Json(new JazMaxMultipleResultHelper
            {
                Result = Common.Models.JsonResult.Success,
                Message1 = "Total Active Provinces " +  obj.Count()[0].ToString(),
                Message2 = "Total Disabled Provines "+ obj.Count()[1].ToString()
            });

        }

        public string test()
        {
            

            var cache = MemoryCache.Default;

            var key = "myKey2";
            var value = obj.Count();
            var policy = new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 0, 30) };
            cache.Add(key, value, policy);

            int[] st = (int[])cache["myKey2"];
            return "Total Active Provinces " + st[0].ToString() + "<br/> " + "Total Disabled Provines " + st[1].ToString();
        }

    }
}