using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Models
{
    public class HouseFilterModel
    {
        public HouseModel House { get; set; }
        public List<HouseModel> Houses { get; set; }
        public List<string> Cities { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public List<BrokerModel> Brokers { get; set; }
        public string FilterByCityName { get; set; }
        public int FilterByCompanyId { get; set; }
        public int FilterByBrokerId { get; set; }
        public HouseFilterModel(List<HouseModel> houses, List<string> cities)
        {
            Houses = houses;
            Cities = cities;
        }
        public HouseFilterModel()
        {

        }
    }
}
