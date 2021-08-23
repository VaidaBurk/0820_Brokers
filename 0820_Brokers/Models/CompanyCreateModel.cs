using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Models
{
    public class CompanyCreateModel
    {
        public CompanyModel Company { get; set; }
        public List<int> BrokerIds { get; set; }
        public List<BrokerModel> BrokerModels { get; set; }

        public CompanyCreateModel()
        {

        }
    }
}
