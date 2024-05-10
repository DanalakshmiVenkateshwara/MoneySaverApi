using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Net;

namespace DataAccess.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<int> SaveUserKYC(UserKycDetails userKycDetails)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Registration, new
            {
                AadharName = userKycDetails.AadharName,
                AadharNo = userKycDetails.AadharNo,
                Address = userKycDetails.Address,
                DOB = userKycDetails.DOB,
                AadharImage = userKycDetails.AadharImage,
                IsAadharVerified = userKycDetails.IsAadharVerified,
                phone = userKycDetails.Phone
            });
        }
    }
}
