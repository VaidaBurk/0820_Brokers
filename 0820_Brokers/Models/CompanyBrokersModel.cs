using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Models
{
    public class CompanyBrokersModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<BrokerModel> Brokers { get; set; }
        public CompanyBrokersModel(int companyId, string companyName, List<BrokerModel> brokers)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            Brokers = brokers;
        }
    }
}
