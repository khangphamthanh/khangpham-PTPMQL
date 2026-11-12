using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class FormOfTrainingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormOfTrainingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString = "", int page = 1, int pageSize = 5)
        {
            return View(await GetFormsPageAsync(searchString, page, pageSize));
        }

        public async Task<IActionResult> Table(string searchString = "", int page = 1, int pageSize = 5)
        {
            return PartialView("_FormOfTrainingTable",
                await GetFormsPageAsync(searchString, page, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new FormOfTraining());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormOfTraining formOfTraining)
        {
            if (await FormNameExistsAsync(formOfTraining.FormName))
            {
                ModelState.AddModelError(nameof(FormOfTraining.FormName),
                    "Tên hệ đào tạo đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", formOfTraining);
            }

            try
            {
                _context.FormOfTrainings.Add(formOfTraining);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm hệ đào tạo thành công." });
            }
            catch (DbUpdateException)
            {
                _context.Entry(formOfTraining).State = EntityState.Detached;
                ModelState.AddModelError(nameof(FormOfTraining.FormName),
                    "Không thể thêm hệ đào tạo. Mã hoặc tên có thể vừa được tạo bởi người dùng khác.");
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", formOfTraining);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var formOfTraining = await _context.FormOfTrainings.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FormId == id);
            return formOfTraining == null
                ? NotFound()
                : PartialView("_Edit", formOfTraining);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FormOfTraining formOfTraining)
        {
            if (id != formOfTraining.FormId)
            {
                return NotFound();
            }

            if (await FormNameExistsAsync(formOfTraining.FormName, id))
            {
                ModelState.AddModelError(nameof(FormOfTraining.FormName),
                    "Tên hệ đào tạo đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Edit", formOfTraining);
            }

            var existingForm = await _context.FormOfTrainings
                .FirstOrDefaultAsync(f => f.FormId == id);
            if (existingForm == null)
            {
                return NotFound();
            }

            existingForm.FormName = formOfTraining.FormName;

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật hệ đào tạo thành công." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FormOfTrainingExistsAsync(id))
                {
                    return NotFound();
                }

                return Conflict(new
                {
                    message = "Dữ liệu hệ đào tạo đã được thay đổi bởi người dùng khác. Vui lòng tải lại và thử lại."
                });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể cập nhật hệ đào tạo. Vui lòng kiểm tra dữ liệu và thử lại."
                });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var formOfTraining = await _context.FormOfTrainings
                .FirstOrDefaultAsync(f => f.FormId == id);
            return formOfTraining == null ? NotFound() : View(formOfTraining);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var formOfTraining = await _context.FormOfTrainings.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FormId == id);
            return formOfTraining == null
                ? NotFound()
                : PartialView("_Delete", formOfTraining);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var formOfTraining = await _context.FormOfTrainings.FindAsync(id);
            if (formOfTraining == null)
            {
                return NotFound();
            }

            try
            {
                _context.FormOfTrainings.Remove(formOfTraining);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Xóa hệ đào tạo thành công." });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể xóa vì đang có dữ liệu liên quan."
                });
            }
        }

        private async Task<List<FormOfTraining>> GetFormsPageAsync(
            string searchString,
            int page,
            int pageSize)
        {
            pageSize = pageSize < 1 ? 5 : pageSize;
            page = page < 1 ? 1 : page;
            searchString = searchString?.Trim() ?? string.Empty;

            var query = _context.FormOfTrainings
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(f => f.FormName.Contains(searchString));
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
                .OrderBy(f => f.FormId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private async Task<bool> FormOfTrainingExistsAsync(string id)
        {
            return await _context.FormOfTrainings.AnyAsync(f => f.FormId == id);
        }

        private async Task<bool> FormNameExistsAsync(string name, string? excludedId = null)
        {
            var normalizedName = name.Trim();
            return await _context.FormOfTrainings.AnyAsync(f =>
                f.FormName == normalizedName &&
                (excludedId == null || f.FormId != excludedId));
        }
    }
}
