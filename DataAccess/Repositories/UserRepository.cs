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
            if (userKycDetails.IsAadharVerified)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Registration, new
                {
                    AadharName = userKycDetails.AadharName,
                    AadharNo = userKycDetails.AadharNo,
                    Address = userKycDetails.Address,
                    DOB = userKycDetails.DOB,
                    AadharImage = userKycDetails.Image,
                    IsAadharVerified = userKycDetails.IsAadharVerified,
                    phone = userKycDetails.Phone
                });
            }
            else if (userKycDetails.IsSelfieVerified)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Selfie_Updataion, new
                {
                    selfieImage = userKycDetails.Image,
                    IsSelfieVerified = userKycDetails.IsSelfieVerified,
                    phone = userKycDetails.Phone
                });
            }
            else if(userKycDetails.IsBankVerified)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Bank_Updataion, new
                {
                    AcNumber = userKycDetails.AcNumber,
                    AcHolderName = userKycDetails.AcHolderName,
                    TypeOfAccount = userKycDetails.TypeOfAccount,
                     IfscCode = userKycDetails.IfscCode,
                    IsBankVerified = userKycDetails.IsBankVerified,
                    selfieImage = userKycDetails.Image,
                    IsSelfieVerified = userKycDetails.IsSelfieVerified,
                    phone = userKycDetails.Phone
                });
            }
            return 0;
            
        }
    }
}
