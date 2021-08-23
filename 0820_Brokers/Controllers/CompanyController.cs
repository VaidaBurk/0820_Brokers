using _0820_Brokers.Models;
using _0820_Brokers.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            CompanyCreateModel selectedCompany = _createCompanyDBService.SelectedCompany(companyId);
            return View("AddBroker", selectedCompany);
        }
    }
}
