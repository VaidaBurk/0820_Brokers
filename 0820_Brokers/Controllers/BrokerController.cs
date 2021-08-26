using _0820_Brokers.Models;
using _0820_Brokers.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Controllers
{
    public class BrokerController : Controller
    {
        private BrokerDBService _brokerDBService;
        private CompanyDBService _companyDBService;
        public BrokerController(BrokerDBService brokerDBService, CompanyDBService companyDBService)
        {
            _brokerDBService = brokerDBService;
            _companyDBService = companyDBService;
        }
        public IActionResult Index()
        {
            List<BrokerModel> brokers = _brokerDBService.GetData();
            return View(brokers);
        }

        public IActionResult DisplayCreate()
        {
            return View("Create");
        }

        public IActionResult Create(BrokerModel broker)
        {
            _brokerDBService.SaveToDatabase(broker);
            return RedirectToAction("Index");
        }

        public IActionResult DisplayPossibleAppartments(int brokerId)
        {
            List<HouseModel> houses = _brokerDBService.HousesBrokerCanChoose(brokerId);
            BrokerHousesModel possibleHouses = new(houses, brokerId);
            return View("DisplayPossibleAppartments", possibleHouses);
        }
        public IActionResult AssignAppartmentToBroker(int houseId, int brokerId)
        {
            _brokerDBService.AssignAppartmentToBroker(houseId, brokerId);
            return RedirectToAction("ListBrokerAppartments", "House", brokerId);
        }
    }
}
