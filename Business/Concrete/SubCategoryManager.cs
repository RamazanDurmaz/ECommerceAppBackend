using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class SubCategoryManager : ISubCategoryService
    {
        private ISubCategoryDal _subCategoryDal;
        
        public SubCategoryManager(ISubCategoryDal subCategoryDal)
        {
            _subCategoryDal = subCategoryDal;
        }

        public IDataResult<List<SubCategory>> GetAll()
        {
            return new SuccessDataResult<List<SubCategory>>(_subCategoryDal.GetAll().ToList());
        }

        public IDataResult<SubCategory> GetById(int subCategoryId)
        {
            return new SuccessDataResult<SubCategory>(_subCategoryDal.Get(c => c.SubCategoryId == subCategoryId));
        }
    }
}
