using _0820_Brokers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Services
{
    public class CreateCompanyDBService
    {
        public readonly CompanyDBService CompanyDBService;
        public readonly BrokerDBService BrokerDBService;

        public CreateCompanyDBService(CompanyDBService companyDBService, BrokerDBService brokerDBService)
        {
            CompanyDBService = companyDBService;
            BrokerDBService = brokerDBService;
        }

        public CompanyCreateModel NewCompany()
        {
            CompanyModel newCompany = new();

            return new CompanyCreateModel()
            {
                Company = newCompany,
                BrokerModels = BrokerDBService.GetData()
            };
        }

        public CompanyCreateModel SelectCompanyWithPossibleBrokers(int companyId)
        {
            return new CompanyCreateModel
            {
                Company = CompanyDBService.GetCompanyById(companyId).FirstOrDefault(),
                BrokerModels = BrokerDBService.GetNotCompanyBrokers(companyId)
            };
        }
    }
}
