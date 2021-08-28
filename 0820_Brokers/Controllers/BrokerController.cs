﻿using _0820_Brokers.Models;
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
        public BrokerController(BrokerDBService brokerDBService)
        {
            _brokerDBService = brokerDBService;
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
            string brokerName = _brokerDBService.GetBrokerName(brokerId);
            BrokerHousesModel possibleHouses = new(houses, brokerId, brokerName);
            return View("DisplayPossibleAppartments", possibleHouses);
        }
        public IActionResult AssignAppartmentToBroker(int houseId, int brokerId)
        {
            _brokerDBService.AssignAppartmentToBroker(houseId, brokerId);
            return RedirectToAction("ListBrokerAppartments", "House", new { brokerId = brokerId } );
        }
        public IActionResult DisplayEditBroker(int brokerId)
        {
            BrokerModel broker = _brokerDBService.GetBrokerData(brokerId);
            return View("Edit", broker);
        }
        public IActionResult EditBroker(BrokerModel broker)
        {
            _brokerDBService.EditBrokerInDatabase(broker);
            return RedirectToAction("Index");
        } 
    }
}
