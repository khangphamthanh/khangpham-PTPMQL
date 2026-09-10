using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DemoMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Data form Index action method of StudentController";
            return View();
        }

        [HttpPost]
        public IActionResult Index(Student std)
        {
            //var fName = fullName;
            //fName = "Xin chào " + fName.ToUpper() + "!";
            //ViewData["FullName"] = fName;
            //var data = $"Họ và tên: ";
            ViewData["TheStudent"] = std;
            ViewBag["TheStudent"] = std;
            TempData["TheStudent"] = JsonSerializer.Serialize(std);


            return View(std);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}