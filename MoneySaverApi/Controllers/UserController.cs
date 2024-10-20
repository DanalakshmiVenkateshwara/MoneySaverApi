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
using Razorpay.Api;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;  // Make sure to import Razorpay API namespace
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
                return Ok(new CreationResults { Id = result, Success = true, Message = "" });
            else
                return Ok(new CreationResults { Id = result, Success = false, Message = "" });
        }
        [HttpGet]
        [Route("GetWithDraws")]
        public async Task<List<Investments>> GetWithDraws(string mobile)
        {
            return await _userManager.GetWithDraws(mobile);
        }
        [HttpGet]
        [Route("GetRecurringInvestments")]
        public async Task<List<RecurringInvestments>> GetRecurringInvestments(string mobile)
        {
            return await _userManager.GetRecurringInvestments(mobile);
        }
        [HttpGet]
        [Route("GetInvestments")]
        public async Task<List<Investments>> GetInvestments(string mobile)
        {
            return await _userManager.GetInvestments(mobile);
        }

        [HttpGet]
        [Route("GetSelfie")]
        public async Task<List<UserKycDetails>> GetSelfie(string mobile)
        {
            return await _userManager.GetSelfie(mobile);
        }
        [HttpGet]
        [Route("GetTransactions")]
        public async Task<List<Transactions>> GetTransactions(string mobile)
        {
            return await _userManager.GetTransactions(mobile);
        }
        [HttpGet]
        [Route("GetDashboard")]
        public async Task<Dashboard> GetDashboard(string mobile)
        {
            var investments = await _userManager.GetDashboard(mobile);
              // Calculate the sum of MaturityValue and Amount
        int totalMaturityValue = investments.Sum(investment => investment.MaturityValue);
        int totalAmount = investments.Sum(investment => investment.Amount);
         double growth = totalMaturityValue - totalAmount;
        double percentage = (growth/totalAmount)*100;
        double Roundedpercentage = Math.Round(percentage,2);
            var result = new  Dashboard
        {  
            MaturityValue = totalMaturityValue, 
            Amount = totalAmount, 
            InterestAmount = growth,
            percentage = Roundedpercentage,
            
        };
        return result;
        }

        [HttpPost]
        [Route("CreateRazorpayOrder")]
        public IActionResult CreateRazorpayOrder([FromBody] RazorpayOrderRequest orderRequest)
        {
            try
            {
                // Initialize Razorpay Client
                RazorpayClient client = new RazorpayClient("rzp_test_Oab7yc2dBS3Mbl", "KcbjpVlXQLHtBsjafyzlNQOU");

                // Create a dictionary for the order request
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", orderRequest.Amount },
                    { "currency", orderRequest.Currency },
                    { "receipt", orderRequest.Receipt }
                };

                Dictionary<string, object> notes = new Dictionary<string, object>
                {
                    { "notes_key_1", "Tea, Earl Grey, Hot" }
                };
                options.Add("notes", notes);

                // Create the order
                Order order = client.Order.Create(options);
                return Ok(order.Attributes.ToString());
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
         [HttpPost]
        [Route("VerifyPaymentSignature")]
        public IActionResult VerifyPaymentSignature([FromBody] RazorpayPaymentVerificationRequest verificationRequest)
        {
            try
            {
                  // Initialize Razorpay Client
                RazorpayClient client = new RazorpayClient("rzp_test_Oab7yc2dBS3Mbl", "KcbjpVlXQLHtBsjafyzlNQOU");
                // Create a dictionary to hold the options for signature verification
                Dictionary<string, string> options = new Dictionary<string, string>
                {
                    { "razorpay_order_id", verificationRequest.OrderId },
                    { "razorpay_payment_id", verificationRequest.PaymentId },
                    { "razorpay_signature", verificationRequest.Signature }
                };
                // Verify the payment signature
                Utils.verifyPaymentSignature(options);

                // If verification passes, return success response
                return Ok(new { Success = true, Message = "Payment signature verified successfully." });
            }
            catch (Exception ex)
            {
                // Handle signature verification failure
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        // Create a model for Razorpay Payment Verification Request
        public class RazorpayPaymentVerificationRequest
        {
            public string OrderId { get; set; }
            public string PaymentId { get; set; }
            public string Signature { get; set; }
        }
        // Create a model for Razorpay Order Request
        public class RazorpayOrderRequest
        {
            public int Amount { get; set; }  // Amount in paise (e.g., 50000 paise = 500 INR)
            public string Currency { get; set; }
            public string Receipt { get; set; }
            public Dictionary<string, string> Notes { get; set; }
        }
    }
}
