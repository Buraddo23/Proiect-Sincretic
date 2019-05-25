﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PLCWebControl.Models;
using PLCWebControl.Services;

namespace PLCWebControl.Controllers
{
    public class HomeController : Controller
    {
        private ITcpService _service;

        public HomeController(ITcpService service)
        {
            _service = service;
        }

        //This is the default action
        public IActionResult Index()
        {
            var lastData = _service.GetLastData();
            return View(lastData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
