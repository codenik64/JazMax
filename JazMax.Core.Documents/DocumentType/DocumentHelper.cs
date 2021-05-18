using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Documents;
using JazMax.Core.SystemHelpers;
using System.Data.Entity;

namespace JazMax.Core.Documents.DocumentType
{
    public class DocumentHelper
    {
        #region Get Document Types
        public IQueryable<DataAccess.CoreDocumentType>GetTypes()
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var types = db.CoreDocumentTypes.ToList();
                return types.AsQueryable();
            }
        }
        #endregion

        #region Get All Document Types
        public IQueryable<DocumentTypesView> GetAllTypes()
        {
            var types = GetTypes().Select(x => new DocumentTypesView
            {
                CoreFileCategoryId = x.CoreFileCategoryId,
                CategoryName = x.CategoryName,
                IsActive = x.IsActive
            }).ToList();

            return types.AsQueryable();
        }
        #endregion

        #region Find DocumentType By Id
        public DocumentTypesView FindById(int id)
        {
            return GetAllTypes().FirstOrDefault(x => x.CoreFileCategoryId == id);
        }
        #endregion

        #region Create Document Type

        public int CreateDocument(DocumentTypesView doctype)
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                DataAccess.CoreDocumentType type = new DataAccess.CoreDocumentType()
                {
                    CoreFileCategoryId = doctype.CoreFileCategoryId,
                    CategoryName = doctype.CategoryName,
                    IsActive = true,
                };
                db.CoreDocumentTypes.Add(type);
                db.SaveChanges();
                return doctype.CoreFileCategoryId;
            }
        }
        #endregion

       
     
    }
}
