using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Models
{
    public class CompanyHousesModel
    {
        public List<HouseModel> Houses { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public CompanyHousesModel(List<HouseModel> houses, int companyId, string companyName)
        {
            Houses = houses;
            CompanyId = companyId;
            CompanyName = companyName;
        }
    }
}
