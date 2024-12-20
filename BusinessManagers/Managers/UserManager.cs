﻿using BusinessManagers.Interfaces;
using BusinessObjects;
using DataAccess;
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
        public async Task<List<RateOfIntrest>> GetROI()
        {
            return await _userRepository.GetROI();
        }
        public async Task<RateOfIntrest> Get_RazorPayKeys()
        {
            return await _userRepository.Get_RazorPayKeys();
        }
        public async Task<int> SaveUserDetails(UserDetails userDetails)
        {
            return await _userRepository.SaveUserDetails(userDetails);
        }
        public async Task<UserDetails> GetUserDetails(string phone, string password)
        {
            return await _userRepository.GetUserDetails(phone, password);
        }

        public async Task<int> SaveInvestments(Investments investments)
        {
            return await _userRepository.SaveInvestments(investments);
        }
        public async Task<List<Investments>> GetInvestments(string mobile)
        {
            return await _userRepository.GetInvestments(mobile);
        }
        public async Task<List<Transactions>> GetTransactions(string mobile)
        {
            return await _userRepository.GetTransactions(mobile);
        }
        public async Task<List<Investments>> GetDashboard(string mobile)
        {
            return await _userRepository.GetDashboard(mobile);
        }
        public async Task<List<UserKycDetails>> GetSelfie(string mobile)
        {
            return await _userRepository.GetSelfie(mobile);
        }
        public async Task<List<Investments>> GetWithDraws(string mobile)
        {
            return await _userRepository.GetWithDraws(mobile);
        }
        public async Task<List<RecurringInvestments>> GetRecurringInvestments(string mobile)
        {
            return await _userRepository.GetRecurringInvestments(mobile);
        }
    }
}
