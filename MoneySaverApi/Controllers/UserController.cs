using BusinessManagers.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneySaver.Utilities.Config;
using Microsoft.Extensions.Hosting.Internal;

namespace MoneySaverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MoneySaverCors")]
    public class UserController : BaseController
    {
        private IUserManager _userManager { get; set; }
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IOptions<StaticFileSettings> _staticFileSettings;

        public UserController(IUserManager userManager, IWebHostEnvironment hostingEnvironment, IOptions<StaticFileSettings> staticFileSettings)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _staticFileSettings = staticFileSettings;
        }
        [EnableCors]
        [HttpPost]
        [Route("SaveUserKYC")]
        public async Task<IActionResult> SaveUserKYC([FromForm] UserKycDetails userKycDetails)
        {
            //string path = Path.Combine(_hostingEnvironment.ContentRootPath, _staticFileSettings.Value.Documents);

            List<string> imageFileNames = new();

            //var extension = Path.GetExtension(userKycDetails.Image);

            //if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg")// need to add constants
            //{
            //    if (userKycDetails.ImageFile != null)
            //    {
            //        imageFileNames = SaveFiles(path, userKycDetails.ImageFile, string.Empty);
            //        userKycDetails.Image = imageFileNames[0];
            //    }
            //}
             int result = await _userManager.SaveUserKYC(userKycDetails);
            if(result > 0)
                return Ok(new KycResults { Success = true,Id = result, Message = "", ImagePath = "" });
            else
                return Ok(new KycResults { Success = false, Message = "" , ImagePath =""});
        }
        [EnableCors]
        [HttpGet]
        [Route("GetKYCDetails")]
        public async Task<KycStatusDetails> GetKycDetails(string mobile)
        {
            return await _userManager.GetKycDetails(mobile);
        }
    }
}
