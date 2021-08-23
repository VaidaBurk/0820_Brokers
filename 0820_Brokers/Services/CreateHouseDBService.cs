using _0820_Brokers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Services
{
    public class CreateHouseDBService
    {
        public readonly CompanyDBService CompanyDBService;
        public readonly BrokerDBService BrokerDBService;
        public CreateHouseDBService(CompanyDBService companyDBService, BrokerDBService brokerDBService)
        {
            CompanyDBService = companyDBService;
            BrokerDBService = brokerDBService;
        }
        public HouseCreateModel NewHouse()
        {
            HouseModel house = new();

            return new HouseCreateModel()
            {
                House = house,
                BrokerModels = BrokerDBService.GetData(),
                CompanyModels = CompanyDBService.GetData()
            };
        }
    }
}
