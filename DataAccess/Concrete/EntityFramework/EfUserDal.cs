using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, ECommerceContext>, IUserDal
    {
        ///OperationClaim Dto yapılsa daha iyi olur
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new ECommerceContext())
            {
                var result = from operationClaim in context.OperationClaim
                             join userOperationClaim in context.UserOperationClaim
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

                return result.ToList();
            }
        }
    }
}
