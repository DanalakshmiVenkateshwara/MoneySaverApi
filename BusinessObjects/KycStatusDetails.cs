using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class KycStatusDetails
    {
        public bool IsAadharVerified { get; set; }
        public bool IsSelfieVerified { get; set; }
        public bool IsBankVerified { get; set; }

    }
}
