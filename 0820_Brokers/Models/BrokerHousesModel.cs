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
        public string BrokerName { get; set; }
        public BrokerHousesModel(List<HouseModel> houses, int brokerId, string brokerName)
        {
            Houses = houses;
            BrokerId = brokerId;
            BrokerName = brokerName;
        }
    }
}
