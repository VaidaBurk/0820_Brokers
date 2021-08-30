using _0820_Brokers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Services
{
    public class HouseFilterDBService
    {
        public BrokerDBService BrokerDBService;
        public CompanyDBService CompanyDBService;
        public HouseDBService HouseDBService;
        public HouseFilterDBService(BrokerDBService brokerDBService, CompanyDBService companyDBservice, HouseDBService houseDBService)
        {
            BrokerDBService = brokerDBService;
            CompanyDBService = companyDBservice;
            HouseDBService = houseDBService;
        }
        public HouseFilterModel GetData()
        {
            return new HouseFilterModel()
            {
                Companies = CompanyDBService.GetData(),
                Brokers = BrokerDBService.GetData(),
                Houses = HouseDBService.GetData(),
                Cities = HouseDBService.GetAllCities()
            };
        }
        public HouseFilterModel GetFilteredByCityData(string cityName)
        {
            return new HouseFilterModel()
            {
                Houses = HouseDBService.GetFilteredByCity(cityName),
                Companies = CompanyDBService.GetData(),
                Brokers = BrokerDBService.GetData(),
                Cities = HouseDBService.GetAllCities()
            };
        }
        public HouseFilterModel GetFilteredByCompanyData(int companyId)
        {
            return new HouseFilterModel()
            {
                Houses = HouseDBService.GetFilteredByCompany(companyId),
                Companies = CompanyDBService.GetData(),
                Brokers = BrokerDBService.GetData(),
                Cities = HouseDBService.GetAllCities()
            };
        }
        public HouseFilterModel GetFilteredByBrokerData(int brokerId)
        {
            return new HouseFilterModel()
            {
                Houses = HouseDBService.GetFilteredByBroker(brokerId),
                Companies = CompanyDBService.GetData(),
                Brokers = BrokerDBService.GetData(),
                Cities = HouseDBService.GetAllCities()
            };
        }
    }
}
