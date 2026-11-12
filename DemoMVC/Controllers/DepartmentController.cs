using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString = "", int page = 1, int pageSize = 5)
        {
            return View(await GetDepartmentsPageAsync(searchString, page, pageSize));
        }

        public async Task<IActionResult> Table(string searchString = "", int page = 1, int pageSize = 5)
        {
            return PartialView("_DepartmentTable",
                await GetDepartmentsPageAsync(searchString, page, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new Department());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (await DepartmentNameExistsAsync(department.DepartmentName))
            {
                ModelState.AddModelError(nameof(Department.DepartmentName),
                    "Tên khoa đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", department);
            }

            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm khoa thành công." });
            }
            catch (DbUpdateException)
            {
                _context.Entry(department).State = EntityState.Detached;
                ModelState.AddModelError(nameof(Department.DepartmentName),
                    "Không thể thêm khoa. Tên khoa có thể vừa được tạo bởi người dùng khác.");
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", department);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var department = await _context.Departments.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            return department == null
                ? NotFound()
                : PartialView("_Edit", department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (await DepartmentNameExistsAsync(department.DepartmentName, id))
            {
                ModelState.AddModelError(nameof(Department.DepartmentName),
                    "Tên khoa đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Edit", department);
            }

            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            existingDepartment.DepartmentName = department.DepartmentName;

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật khoa thành công." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DepartmentExistsAsync(id))
                {
                    return NotFound();
                }

                return Conflict(new
                {
                    message = "Dữ liệu khoa đã được thay đổi bởi người dùng khác. Vui lòng tải lại và thử lại."
                });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể cập nhật khoa. Vui lòng kiểm tra dữ liệu và thử lại."
                });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            return department == null ? NotFound() : View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var department = await _context.Departments.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            return department == null
                ? NotFound()
                : PartialView("_Delete", department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            try
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Xóa khoa thành công." });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể xóa vì đang có dữ liệu liên quan."
                });
            }
        }

        private async Task<List<Department>> GetDepartmentsPageAsync(
            string searchString,
            int page,
            int pageSize)
        {
            pageSize = pageSize < 1 ? 5 : pageSize;
            page = page < 1 ? 1 : page;
            searchString = searchString?.Trim() ?? string.Empty;

            var query = _context.Departments
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(d => d.DepartmentName.Contains(searchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalRecords / (double)pageSize));
            page = Math.Min(page, totalPages);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchString = searchString;

            return await query
                .OrderByDescending(d => d.DepartmentId)
                .Include(d => d.Students)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private async Task<bool> DepartmentExistsAsync(string id)
        {
            return await _context.Departments.AnyAsync(d => d.DepartmentId == id);
        }

        private async Task<bool> DepartmentNameExistsAsync(string name, string? excludedId = null)
        {
            var normalizedName = name.Trim();
            return await _context.Departments.AnyAsync(d =>
                d.DepartmentName == normalizedName &&
                (excludedId == null || d.DepartmentId != excludedId));
        }
    }
}
