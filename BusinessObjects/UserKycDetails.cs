using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserKycDetails
    {
        public string? AadharName { get; set; }
        public string? AadharNo { get; set; }
        public string? Address { get; set; }
        public string? DOB { get; set; }
        public string? AadharImage { get; set; }
        public bool IsAadharVerified { get; set; }
        public string? selfieImage { get; set; }
        public string? Image { get; set; }
        public string? Email { get; set; }
        public bool IsSelfieVerified { get; set; }
        public string? AcNumber { get; set; }
        public string? AcHolderName { get; set; }
        public string? TypeOfAccount { get; set; }
        public string? IfscCode { get; set;}
        public bool IsBankVerified { get;set; }
        public string? Phone { get; set; }
        public IFormFileCollection ImageFile { get; set; }
    }
}
