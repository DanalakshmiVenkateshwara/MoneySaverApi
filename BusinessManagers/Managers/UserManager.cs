using BusinessManagers.Interfaces;
using BusinessObjects;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagers.Managers
{
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<int> SaveUserKYC(UserKycDetails userKycDetails)
        {
            return await _userRepository.SaveUserKYC(userKycDetails);
        }
        public async Task<KycStatusDetails> GetKycDetails(string mobile)
        {
            return await _userRepository.GetKycDetails(mobile);
        }
        public async Task<int> SaveInvestments(Investments investments)
        {
            return await _userRepository.SaveInvestments(investments);
        }
    }
}
