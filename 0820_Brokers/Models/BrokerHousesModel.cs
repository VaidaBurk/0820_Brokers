using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Models
{
    public class BrokerHousesModel
    {
        public List<HouseModel> Houses { get; set; }
        public int BrokerId { get; set; }
        public BrokerHousesModel(List<HouseModel> houses, int brokerId)
        {
            Houses = houses;
            BrokerId = brokerId;
        }
    }
}
