using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> SaveUserKYC(UserKycDetails userKycDetails);
        Task<KycStatusDetails> GetKycDetails(string mobile);
        Task<int> SaveInvestments(Investments investments);
        Task<List<RateOfIntrest>> GetROI();
        Task<List<Investments>> GetInvestments(string mobile);
        Task<List<Transactions>> GetTransactions(string mobile);
        Task<List<Investments>> GetDashboard(string mobile);
        Task<List<UserKycDetails>> GetSelfie(string mobile);
        Task<List<Investments>> GetWithDraws(string mobile);
        Task<List<RecurringInvestments>> GetRecurringInvestments(string mobile);
    }
}
