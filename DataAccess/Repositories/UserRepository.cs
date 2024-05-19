﻿using DataAccess.Repositories.Interfaces;
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
            if (!userKycDetails.IsAadharVerified)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Registration, new
                {
                    AadharName = userKycDetails.AadharName,
                    AadharNo = userKycDetails.AadharNo,
                    Address = userKycDetails.Address,
                    DOB = userKycDetails.DOB,
                    AadharImage = userKycDetails.AadharImage,
                    IsAadharVerified = true,
                    phone = userKycDetails.Phone
                });
            }
            else if (!userKycDetails.IsSelfieVerified)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Selfie_Updataion, new
                {
                    selfieImage = userKycDetails.selfieImage,
                    IsSelfieVerified = true,
                    Email = userKycDetails.Email,
                    phone = userKycDetails.Phone
                });
            }
            else if(!userKycDetails.IsBankVerified)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.USer_Kyc_Bank_Updataion, new
                {
                    AcNumber = userKycDetails.AcNumber,
                    AcHolderName = userKycDetails.AcHolderName,
                    TypeOfAccount = userKycDetails.TypeOfAccount,
                    IfscCode = userKycDetails.IfscCode,
                    IsBankVerified = true,
                    //selfieImage = userKycDetails.Image,
                    //IsSelfieVerified = userKycDetails.IsSelfieVerified,
                    phone = userKycDetails.Phone
                });
            }
            return 0;
            
        }
        public async Task<KycStatusDetails> GetKycDetails(string mobile)
        {
            var test = await this.Find<KycStatusDetails>(SqlQueries.Get_Kyc_Details, new { mobile });

            // Check if test is null (no records found)
            if (test == null)
            {
                // If no records are found, return a new KycStatusDetails object with all properties set to false
                return new KycStatusDetails
                {
                    IsAadharVerified = false,
                    IsSelfieVerified = false,
                    IsBankVerified = false
                };
            }

            return test;
        }
    }
}
