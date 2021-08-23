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
        public HouseController(CreateHouseDBService createHoueDBService, HouseDBService houseDBService)
        {
            _createHouseDBService = createHoueDBService;
            _houseDBService = houseDBService;
        }
        public IActionResult Index()
        {
            List<HouseModel> houses = _houseDBService.GetData();
            return View(houses);
        }
        public IActionResult DisplayCreate()
        {
            HouseCreateModel newHouse = _createHouseDBService.NewHouse();
            return View("Create", newHouse);
        }
        public IActionResult Create(HouseModel house)
        {
            _houseDBService.SaveToDatabase(house);
            return View();
        }
    }
}
