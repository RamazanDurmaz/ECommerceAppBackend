using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    //İsimlendirme productService de olabilir
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        [CacheAspect]
        public IDataResult<Product> GetByID(int productId)
        {
            //Burda entityframework ilerde değiştirilmek istendiğinde problem çıkarır.Bu yüzden dependency injection altyapısı kullanılmalıdır
            //EfProductDal productDal = new EfProductDal();
            //return productDal.Get(p => p.ProductId == productId);
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));

        }

        //[CacheAspect]
        //[SecuredOperation("Product.List")]
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll().ToList());
        }

        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.SubCategoryId == categoryId).ToList());
        }

        //Claim 
        //admin claim ine veya product.add claimine sahip olması gerekiyor
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {

            //Kategori sayısı 15 ten büyük ise ürün eklenemez
            IResult result = BusinessRules.Run(CheckIfProductCountCategoryCorrect(product.SubCategoryId), CheckIfProductNameExists(product.ProductName), CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);



        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        private IResult CheckIfProductCountCategoryCorrect(int categoryId)
        {
            //Bir kategoride en fazla 50 ürün olabilir 
            var result = _productDal.GetAll(p => p.SubCategoryId == categoryId).Count;
            if (result >= 50)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            //Aynı isimde ürün eklenmez
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
