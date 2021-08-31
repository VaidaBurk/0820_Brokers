using _0820_Brokers.Models;
using _0820_Brokers.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace _0820_Brokers.Controllers
{
    public class HouseController : Controller
    {
        private readonly CreateHouseDBService _createHouseDBService;
        private readonly HouseDBService _houseDBService;
        private readonly HouseFilterDBService _houseFilterDBService;
        public HouseController(CreateHouseDBService createHoueDBService, HouseDBService houseDBService, HouseFilterDBService houseFilterDBService)
        {
            _createHouseDBService = createHoueDBService;
            _houseDBService = houseDBService;
            _houseFilterDBService = houseFilterDBService;
        }
        public IActionResult Index()
        {
            HouseFilterModel houses = _houseFilterDBService.GetData();
            return View(houses);
        }
        public IActionResult ListFiltered(HouseFilterModel model)
        {
            if (model.FilterByCityName != null || model.FilterByCompanyId != 0 || model.FilterByBrokerId != 0)
            {
                HouseFilterModel filteredData = _houseFilterDBService.GetFilteredByThreeParameters(model);
                return View("Index", filteredData);
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
