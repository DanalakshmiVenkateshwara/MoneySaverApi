using BusinessManagers.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneySaverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MoneySaverCors")]
    public class UserController : Controller
    {
        private IUserManager _userManager { get; set; }
        
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [EnableCors]
        [HttpPost]
        [Route("SaveUserKYC")]
        public async Task<int> SaveUserKYC(UserKycDetails userKycDetails)
        {

            return await _userManager.SaveUserKYC(userKycDetails);
        }
    }
}
