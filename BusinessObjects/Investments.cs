using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Investments
    {
        public int Amount { get; set; }
        public string? ROIDaily { get; set; }
        public string? ROIYearly { get; set; }
        public string? YearlyTenure { get; set; }
        public string? DailyTenure  { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public int MaturityValue { get; set; }
        public bool IsActive { get; set; }
        public string? Phone { get; set; }
        public string? TransactionId { get; set; }
        public string? PolicyNumber { get; set; }
        public bool? recurring { get; set; }
    }
}
