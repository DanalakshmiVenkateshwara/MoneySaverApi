using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Transactions
    {
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Phone { get; set; }
        public string? Order_Id { get; set; }
        public string? Payment_Id { get; set; }
    }
}
