using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagers.Interfaces
{
    public interface IUserManager
    {
        Task<int> SaveUserKYC(UserKycDetails userKycDetails);
        Task<KycStatusDetails> GetKycDetails(string mobile);
        Task<int> SaveInvestments(Investments investments);
        Task<List<RateOfIntrest>> GetROI();
        Task<List<Investments>> GetInvestments(string mobile);
        Task<List<Investments>> GetWithDraws(string mobile);
    }
}
