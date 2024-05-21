﻿
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess
{
    public static partial class SqlQueries
    {
        public const string USer_Kyc_Registration = @"INSERT INTO CustomerKYCDetails(AadharName,AadharNo,Address,DOB,AadharImage,IsAadharVerified,phone)
                                                        VALUES(@AadharName,@AadharNo,@Address,@DOB,@AadharImage,@IsAadharVerified,@phone)";

        public const string USer_Kyc_Selfie_Updataion = @"Update CustomerKYCDetails set selfieImage = @selfieImage, isselfieverified = @IsSelfieVerified, Email = @Email where phone =@phone";

        public const string USer_Kyc_Bank_Updataion = @"Update CustomerKYCDetails set AcNumber = @AcNumber, AcHolderName = @AcHolderName,TypeOfAc=@TypeOfAccount,IfscCode=@IfscCode ,
                    IsBankVerified = @IsBankVerified where phone =@phone  ";
        public const string Get_Kyc_Details = @"Select IsAadharVerified,IsSelfieVerified,IsBankVerified from CustomerKYCDetails where phone = @mobile ";

        public const string Save_Investments = @"INSERT INTO Investments(Amount, ROIMonthly, ROIYearly, MonthlyTenure,	YearlyTenure, StartDate, MaturityDate, MaturityValue, IsActive,	Phone)
                                                values (@Amount, @ROIMonthly, @ROIYearly, @MonthlyTenure, @YearlyTenure, @StartDate, @MaturityDate, @MaturityValue, @IsActive,	@Phone)";
    }
}




