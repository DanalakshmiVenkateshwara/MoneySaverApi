﻿using BusinessManagers.Interfaces;
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
using System.Reflection;

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
        //[EnableCors]
        [HttpPost]
        [Route("SaveUserKYC")]
        public async Task<IActionResult> SaveUserKYC([FromBody] UserKycDetails userKycDetails)
        {
            //string path = Path.Combine(_hostingEnvironment.ContentRootPath, _staticFileSettings.Value.Documents);

            //List<string> imageFileNames = new();

            //var extension = Path.GetExtension(userKycDetails.Image);

            //if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg")// need to add constants
            //{
            //    if (userKycDetails.ImageFile != null)
            //    {
            //        imageFileNames = SaveFiles(path, userKycDetails.ImageFile, string.Empty);
            //        userKycDetails.Image = imageFileNames[0];
            //    }
            //}

            // Example Base64 string and corresponding file name
            string base64String = $"{(!userKycDetails.IsAadharVerified ? userKycDetails.AadharImage : userKycDetails.selfieImage)}"; ;
            //string base64String = "iVBORw0KGgoAAAANSUhEUgAAAkAAAAKECAYAAADv44WUAAAAAXNSR0IArs4c6QAAAARzQklUCAgICHwIZIgAAAAJcEhZcwAADsQAAA7EAZUrDhsAAANgaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49J++7vycgaWQ9J1c1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCc/Pgo8eDp4bXBtZXRhIHhtbG5zOng9J2Fkb2JlOm5zOm1ldGEvJz4KPHJkZjpSREYgeG1sbnM6cmRmPSdodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjJz4KCiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0nJwogIHhtbG5zOkF0dHJpYj0naHR0cDovL25zLmF0dHJpYnV0aW9uLmNvbS9hZHMvMS4wLyc+CiAgPEF0dHJpYjpBZHM+CiAgIDxyZGY6U2VxPgogICAgPHJkZjpsaSByZGY6cGFyc2VUeXBlPSdSZXNvdXJjZSc+CiAgICAgPEF0dHJpYjpDcmVhdGVkPjIwMjQtMDQtMjk8L0F0dHJpYjpDcmVhdGVkPgogICAgIDxBdHRyaWI6RXh0SWQ+ZDk0N2IyYmItNGY3Yi00ZTZjLWE3YzUtNjEwNTkxNjEzNzBkPC9BdHRyaWI6RXh0SWQ+CiAgICAgPEF0dHJpYjpGYklkPjUyNTI2NTkxNDE3OTU4MDwvQXR0cmliOkZiSWQ+CiAgICAgPEF0dHJpYjpUb3VjaFR5cGU+MjwvQXR0cmliOlRvdWNoVHlwZT4KICAgIDwvcmRmOmxpPgogICA8L3JkZjpTZXE+CiAgPC9BdHRyaWI6QWRzPgogPC9yZGY6RGVzY3JpcHRpb24+CgogPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9JycKICB4bWxuczpkYz0naHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8nPgogIDxkYzp0aXRsZT4KICAgPHJkZjpBbHQ+CiAgICA8cmRmOmxpIHhtbDpsYW5nPSd4LWRlZmF1bHQnPlVudGl0bGVkIGRlc2lnbiAtIDE8L3JkZjpsaT4KICAgPC9yZGY6QWx0PgogIDwvZGM6dGl0bGU+CiA8L3JkZjpE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";
            string fileName = $"{(!userKycDetails.IsAadharVerified ? "aadhar.png" : "selfie.png")}";

            // User-specific folder name
            string userFolderName = $"Files/{userKycDetails.Phone}";
            string savedFilePath = FileUtilities.SaveBase64FileToFolder(userFolderName, base64String, fileName);

            if (!userKycDetails.IsAadharVerified && savedFilePath != null)
            {
                userKycDetails.AadharImage = $"{userFolderName}/{fileName}";
            }
            else if (userKycDetails.IsAadharVerified && !userKycDetails.IsSelfieVerified && savedFilePath != null)
            {
                userKycDetails.selfieImage = $"{userFolderName}/{fileName}";
            }
            Console.Write(savedFilePath);




            int result = await _userManager.SaveUserKYC(userKycDetails);
            if (result > 0)
                return Ok(await GetKycDetails(userKycDetails.Phone));
            //return Ok(new KycResults { Success = true,Id = result, Message = "", ImagePath = "" });
            else
                return Ok(new KycResults { Success = false, Message = "", ImagePath = "" });
        }
        //[EnableCors]
        [HttpGet]
        [Route("GetKYCDetails")]
        public async Task<KycStatusDetails> GetKycDetails(string mobile)
        {
            return await _userManager.GetKycDetails(mobile);
        }
        //[EnableCors]
        [HttpGet]
        [Route("GetROI")]
        public async Task<List<RateOfIntrest>> GetROI()
        {
            return await _userManager.GetROI();
        }

        //[EnableCors]
        [HttpPost]
        [Route("SaveInvestments")]
        public async Task<IActionResult> SaveInvestments([FromBody] Investments investments)
        {
            int result = await _userManager.SaveInvestments(investments);
            if (result > 0)
                return Ok(new CreationResults {Id = result,  Success = true, Message = "" });
            else
                return Ok(new CreationResults { Id = result, Success = false, Message = "" });
        }
        [HttpGet]
        [Route("GetWithDraws")]
        public async Task<List<Investments>> GetWithDraws(string mobile)
        {
            return  await _userManager.GetWithDraws(mobile);
        }
        [HttpGet]
        [Route("GetInvestments")]
        public async Task<List<Investments>> GetInvestments(string mobile)
        {
            return await _userManager.GetInvestments(mobile);
        }
    }
}
