using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Models
{
    public class HouseCreateModel
    {
        public HouseModel House { get; set; }
        public List<CompanyModel> CompanyModels { get; set; }
        public List<BrokerModel> BrokerModels { get; set; }
    }
}
