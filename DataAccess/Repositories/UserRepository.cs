using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Net;
using System.Numerics;
using System.Reflection;
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
            else if (!userKycDetails.IsBankVerified)
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
                return new KycStatusDetails();
                
            }

            return test;
        }
        public async Task<List<RateOfIntrest>> GetROI()
        {
           return await this.All<RateOfIntrest>(SqlQueries.Get_ROI);
        }
        public async Task<int> SaveInvestments(Investments investments)
        {
            if (investments != null)
            {
                var transaction = await this.AddOrUpdateDynamic(SqlQueries.Save_Transaction, new
                {
                    Date = DateTime.Now,
                    Amount = investments.Amount,
                    Type = "Deposit",
                    Status = true,
                    phone = investments.Phone,
                    Order_Id = investments.Order_Id,
                    Payment_Id = investments.Payment_Id,
                    Signature = investments.Signature
                });
                if (transaction > 0 && investments.PolicyNumber != string.Empty)
                {

                    //result = await this.AddOrUpdateDynamic(SqlQueries.Save_SubInvestments, new
                    //{
                    //    Amount = investments.Amount,
                    //    RoI = investments.ROIMonthly,
                    //    Tenure = investments.MonthlyTenure,
                    //    StartDate = investments.StartDate,
                    //    MaturityDate = investments.MaturityDate,
                    //    MaturityValue = investments.MaturityValue,
                    //    IsActive = investments.IsActive,
                    //    Phone = investments.Phone,
                    //    TransactionId = investments.TransactionId,
                    //    PolicyNumber = investments.PolicyNumber
                    //});
                    int result = 0;
                    result = await savesubinvestments(investments);
                    if (result > 0)
                    {
                        return await this.AddOrUpdateDynamic(SqlQueries.Update_Investments, new
                        {
                            Amount = investments.Amount,
                            MaturityValue = investments.MaturityValue,
                            PolicyNumber = investments.PolicyNumber
                        });
                    }
                    return result;
                }
                else
                {
                    //int result = 0;
                    ///*result = await savesubinvestments(investments);*/
                    //if(result > 0)
                    //{
                        return await this.AddOrUpdateDynamic(SqlQueries.Save_Investments, new
                        {
                            Amount = investments.Amount,
                            RoIDaily = investments.ROIDaily,
                            RoIYearly = investments.ROIYearly,
                            DailyTenure = investments.DailyTenure,
                            yearlyTenure = investments.YearlyTenure,
                            StartDate = DateTime.Now.ToString(),
                            MaturityDate = investments.MaturityDate,
                            MaturityValue = investments.MaturityValue,
                            IsActive = investments.IsActive,
                            Phone = investments.Phone,
                            Order_Id = investments.Order_Id,
                            Recurring = investments.recurring
                        });
                    
                    //}
                    //return result;

                }
            }
            else
             return 0;

        }

        private async Task<int> savesubinvestments(Investments investments)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.Save_SubInvestments, new
            {
                Amount = investments.Amount,
                RoI = investments.ROIDaily,
                Tenure = investments.DailyTenure,
                StartDate = investments.StartDate,
                paymentDate = DateTime.Now.ToString(),
                MaturityDate = investments.MaturityDate,
                MaturityValue = investments.MaturityValue,
                IsActive = investments.IsActive,
                Phone = investments.Phone,
                Order_Id = investments.Order_Id,
                Payment_Id = investments.Payment_Id,
                Signature = investments.Signature,
                PolicyNumber = investments.PolicyNumber
            });
        }
        public async Task<List<Investments>> GetInvestments(string mobile)
        {
            return await this.All<Investments>(SqlQueries.Get_Investments, new { mobile});
        }
        public async Task<List<Transactions>> GetTransactions(string mobile)
        {
            return await this.All<Transactions>(SqlQueries.Get_Transactions, new { mobile});
        }
        public async Task<List<UserKycDetails>> GetSelfie(string mobile)
        {
            return await this.All<UserKycDetails>(SqlQueries.Get_Kyc_User_details, new { mobile});
        }
        public async Task<List<Investments>> GetWithDraws(string mobile)
        {
            return await this.All<Investments>(SqlQueries.Get_WithDraws);
        }
        public async Task<List<RecurringInvestments>> GetRecurringInvestments(string mobile)
        {
            return await this.All<RecurringInvestments>(SqlQueries.Get_Recurring_Investments, new { mobile });
        }

    }
}
