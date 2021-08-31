using _0820_Brokers.Models;
using _0820_Brokers.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace _0820_Brokers.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyDBService _companyDBService;
        private readonly CreateCompanyDBService _createCompanyDBService;
        public CompanyController(CompanyDBService companyDBService, CreateCompanyDBService createCompanyDBService)
        {
            _companyDBService = companyDBService;
            _createCompanyDBService = createCompanyDBService;
        }
        public IActionResult Index()
        {
            List<CompanyModel> companies = _companyDBService.GetData();
            return View(companies);
        }
        public IActionResult DisplayCreate()
        {
            CompanyCreateModel newCompany = _createCompanyDBService.NewCompany();
            return View("Create", newCompany);
        }
        public IActionResult Create(CompanyCreateModel newCompany)
        {
            _companyDBService.SaveToDatabase(newCompany);
            return RedirectToAction("Index");
        }
        public IActionResult DisplayAddBroker(int companyId)
        {
            CompanyCreateModel selectedCompany = _createCompanyDBService.SelectCompanyWithPossibleBrokers(companyId);
            selectedCompany.Company.CompanyId = companyId;
            return View("AddBroker", selectedCompany);
        }
        public IActionResult AddBroker(CompanyCreateModel selectedCompany)
        {
            _companyDBService.SaveBrokerToCompany(selectedCompany);
            return RedirectToAction("Index");
        }
        public IActionResult ListCompanyBrokers(int companyId)
        {
            List<BrokerModel> brokers = _companyDBService.GetCompanyBrokers(companyId);
            string companyName = _companyDBService.GetCompanyName(companyId);
            CompanyBrokersModel companyWithBrokers = new(companyId, companyName, brokers);
            return View(companyWithBrokers);
        }
        public IActionResult RemoveBrokerFromCompany(int brokerId, int companyId)
        {
            _companyDBService.RemoveBrokerFromCompany(brokerId, companyId);
            return RedirectToAction("ListCompanyBrokers", new { companyId });
        }
        public IActionResult ListCompanyAppartments(int companyId)
        {
            List<HouseModel> houses = _companyDBService.GetCompanyAppartmentsFromDB(companyId);
            string companyName = _companyDBService.GetCompanyName(companyId);
            CompanyHousesModel companyHouses = new(houses, companyId, companyName);
            return View(companyHouses);
        }
    }
}
