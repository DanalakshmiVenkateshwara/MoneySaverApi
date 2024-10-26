
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

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

        public const string Save_Investments = @"INSERT INTO Investments(Amount, ROIDaily, ROIYearly, DailyTenure,	YearlyTenure, StartDate, MaturityDate, MaturityValue, IsActive,	Phone, Order_Id,Recurring )
                                                values (@Amount, @ROIDaily, @ROIYearly, @DailyTenure, @YearlyTenure, @StartDate, @MaturityDate, @MaturityValue, @IsActive,	@Phone, @Order_Id, @Recurring)";
        public const string Save_Transaction = @"INSERT INTO transactions(Date, Amount, Type, Status, phone,Order_Id,Payment_Id,Signature) 
                                                 values(@Date, @Amount, @Type, @Status, @phone,@Order_Id,@Payment_Id,@Signature)";

        public const string Get_ROI = "Select [Plan],ROI,RPayKey, RPayPassword from rateofintrest";
        public const string Get_RazorPayKeys = "Select RPayKey, RPayPassword from rateofintrest";

        public const string Get_Investments = "Select * from Investments where phone = @mobile";
        public const string Get_Transactions = "Select [Date], Amount, Phone, Payment_Id, Order_Id from Transactions where phone = @mobile";
        public const string Get_Kyc_User_details = "Select * from CustomerKYCDetails where phone = @mobile";
        public const string Get_WithDraws = "Select * from Investments where phone = @mobile";
        public const string Get_Recurring_Investments = "select amount,MaturityDate,MaturityValue, ROIMonthly, ROIYearly, PolicyNumber from Investments where Phone = @mobile and Recurring = 1 ";

        public const string Save_SubInvestments = @"INSERT INTO SubInvestments(Amount, ROI,paymentDate Tenure, StartDate, MaturityDate, MaturityValue, IsActive,	Phone, TransactionId,PolicyNumber)
                                                values (@Amount, @ROI,@paymentDate, @Tenure, @StartDate, @MaturityDate, @MaturityValue, @IsActive, @Phone, @TransactionId,@PolicyNumber)";
        
        public const string Update_Investments = @"update Investments set amount =@Amount , MaturityValue =@MaturityValue  where PolicyNumber = @PolicyNumber)"; 


    }
}




