using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Documents;
using JazMax.Core.SystemHelpers;

namespace JazMax.Core.Documents.FileType
{
    public class FileTypeHelper
    {
        #region Get FileTypes
        public IQueryable<DataAccess.CoreFileType>GetFileType()
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var filetype = db.CoreFileTypes.ToList();
                return filetype.AsQueryable();
            }
        }
        #endregion

        #region GetAll FileTypes
        public IQueryable<CoreFileTypesView>GetAllFileTypes()
        {
            var fileTypes = GetFileType().Select(x => new CoreFileTypesView
            {
                CoreFileTypeId = x.CoreFileTypeId,
                TypeName = x.TypeName,
                IsActive = x.IsActive
            }).ToList();

            return fileTypes.AsQueryable();
        }
        #endregion

        #region FindById
        public CoreFileTypesView FindById(int id)
        {
            return GetAllFileTypes().FirstOrDefault(x => x.CoreFileTypeId == id);
        }
        #endregion

        #region Create FileType
        public int CreateFileType(CoreFileTypesView filetypes)
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                DataAccess.CoreFileType filetype = new DataAccess.CoreFileType()
                {
                    CoreFileTypeId = filetypes.CoreFileTypeId,
                    TypeName = filetypes.TypeName,
                    IsActive = true,
                };
                db.CoreFileTypes.Add(filetype);
                db.SaveChanges();
                return filetypes.CoreFileTypeId;
            }
        }
        #endregion
    }
}
