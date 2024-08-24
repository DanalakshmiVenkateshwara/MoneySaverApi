using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class RecurringInvestments
    {
        public DateTime MaturityDate { get; set; }
        public int Amount { get; set; }
        public int MaturityValue { get; set; }
        public string? Phone { get; set; }
        public string? ROIMonthly { get; set; }
        public string? ROIYearly { get; set; }
        public string? PolicyNumber { get; set; }
        public bool? recurring { get; set; }
    }
}
