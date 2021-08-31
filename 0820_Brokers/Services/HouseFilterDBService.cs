using _0820_Brokers.Models;

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
        public HouseFilterModel GetFilteredByThreeParameters(HouseFilterModel model)
        {
            string sqlCommand = GenerateWhereClause(model);
            return new HouseFilterModel()
            {
                Houses = HouseDBService.GetFilteredByThreeParameters(sqlCommand),
                Companies = CompanyDBService.GetData(),
                Brokers = BrokerDBService.GetData(),
                Cities = HouseDBService.GetAllCities()
            };
        }
        public string GenerateWhereClause(HouseFilterModel model)
        {
            string sqlCommand = "";
            sqlCommand += " WHERE 1=1 ";
            if (model.FilterByCityName != null)
            {
                sqlCommand += $" AND h.City = '{model.FilterByCityName}'";
            }
            if (model.FilterByCompanyId != 0)
            {
                sqlCommand += $" AND h.CompanyId = {model.FilterByCompanyId}";
            }
            if (model.FilterByBrokerId != 0)
            {
                sqlCommand += $" AND h.BrokerId = {model.FilterByBrokerId}";
            }
            return sqlCommand;
        }
    }
}
