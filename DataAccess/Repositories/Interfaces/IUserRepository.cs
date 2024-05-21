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
    }
}
