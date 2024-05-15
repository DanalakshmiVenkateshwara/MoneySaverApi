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
        public const string USer_Kyc_Registration = @"INSERT INTO userkycdetails(AadharName,AadharNo,Address,DOB,AadharImage,IsAadharVerified,phone)
                                                        VALUES(@AadharName,@AadharNo,@Address,@DOB,@AadharImage,@IsAadharVerified,@phone)";

        public const string USer_Kyc_Selfie_Updataion = @"Update userkycdetails set selfieImage = @Image, issefieverified = @issefieverified, Email =@Email where phone =@phone";

        public const string USer_Kyc_Bank_Updataion = @"Update userkycdetails set AcNumber = @AcNumber, AcHolderName = @AcHolderName,TypeOfAccount=@TypeOfAccount,IfscCode=@IfscCode ,
                    IsBankVerified = @IsBankVerified,IsSelfieVerified = IsSelfieVerified where phone =@phone  ";

    }
}



