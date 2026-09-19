using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoMVC.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILogger<LessonController> _logger;

        public LessonController(ILogger<LessonController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}

// Bước 1. Cài đặt các package cần thiết cho project
// Bước 2. Thiết lập kết nối CSDL trong appsettings.json
// Bước 3. Tạo file ApplicationDbContext.
// Bước 4. Cấu hình kết nối CSDL trong Program.cs
// Bước 5. Tạo class trong Model
// Bước 6.Sử dung Migrations để quản lý phiên bản CSDL
// Bước 7. Cập nhật thay đổi CSDL