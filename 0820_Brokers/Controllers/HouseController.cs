using _0820_Brokers.Models;
using _0820_Brokers.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Controllers
{
    public class HouseController : Controller
    {
        private readonly CreateHouseDBService _createHouseDBService;
        private readonly HouseDBService _houseDBService;
        private readonly BrokerDBService _brokerDBService;
        private readonly HouseFilterDBService _houseFilterDBService;
        public HouseController(CreateHouseDBService createHoueDBService, HouseDBService houseDBService, BrokerDBService brokerDBService, HouseFilterDBService houseFilterDBService)
        {
            _createHouseDBService = createHoueDBService;
            _houseDBService = houseDBService;
            _brokerDBService = brokerDBService;
            _houseFilterDBService = houseFilterDBService;
        }
        public IActionResult Index()
        {
            HouseFilterModel housesFromDb = _houseFilterDBService.GetData();
            return View(housesFromDb);
        }
        public IActionResult ListFiltered(HouseFilterModel model)
        {
            HouseFilterModel filteredHouses = _houseFilterDBService.GetData();
            if(model.FilterByCityName != null)
            {
                filteredHouses = _houseFilterDBService.GetFilteredByCityData(model.FilterByCityName);
                return View("Index", filteredHouses);
            }
            if (model.FilterByCompanyId != 0)
            {
                filteredHouses = _houseFilterDBService.GetFilteredByCompanyData(model.FilterByCompanyId);
                return View("Index", filteredHouses);
            }
            if (model.FilterByBrokerId != 0)
            {
                filteredHouses = _houseFilterDBService.GetFilteredByBrokerData(model.FilterByBrokerId);
                return View("Index", filteredHouses);
            }
            return RedirectToAction("Index");
        }
        public IActionResult DisplayCreate()
        {
            HouseCreateModel newHouse = _createHouseDBService.NewHouse();
            return View("Create", newHouse);
        }
        public IActionResult Create(HouseModel house)
        {
            _houseDBService.SaveToDatabase(house);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveAppartmentFromBroker(int houseId, int brokerId)
        {
            _houseDBService.RemoveAppartmentFromBroker(houseId);
            return RedirectToAction("ListBrokerAppartments", new { brokerId });
        }
        public IActionResult DeleteAppartment(int houseId)
        {
            _houseDBService.DeleteAppartment(houseId);
            return RedirectToAction("Index");
        }
        public IActionResult DetailAppartment(int houseId)
        {
            HouseModel house = _houseDBService.GetHouseData(houseId).FirstOrDefault();
            return View("Details", house);
        }
        public IActionResult DisplayEditAppartment(int houseId)
        {
            HouseModel house = _houseDBService.GetHouseData(houseId).FirstOrDefault();
            return View("Edit", house);
        }
        public IActionResult EditAppartment(HouseModel house)
        {
            _houseDBService.EditInDatabase(house);
            return RedirectToAction("DetailAppartment", new { houseId = house.FlatId });
        }
    }
}
