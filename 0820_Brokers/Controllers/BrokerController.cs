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

    }
}