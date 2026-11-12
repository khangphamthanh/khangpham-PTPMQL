using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class DisciplineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisciplineController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString = "", int page = 1, int pageSize = 5)
        {
            return View(await GetDisciplinesPageAsync(searchString, page, pageSize));
        }

        public async Task<IActionResult> Table(string searchString = "", int page = 1, int pageSize = 5)
        {
            return PartialView("_DisciplineTable",
                await GetDisciplinesPageAsync(searchString, page, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new Discipline());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discipline discipline)
        {
            if (await DisciplineNameExistsAsync(discipline.DisciplineName))
            {
                ModelState.AddModelError(nameof(Discipline.DisciplineName),
                    "Tên ngành đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", discipline);
            }

            try
            {
                _context.Disciplines.Add(discipline);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm ngành thành công." });
            }
            catch (DbUpdateException)
            {
                _context.Entry(discipline).State = EntityState.Detached;
                ModelState.AddModelError(nameof(Discipline.DisciplineName),
                    "Không thể thêm ngành. Mã hoặc tên ngành có thể vừa được tạo bởi người dùng khác.");
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", discipline);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DisciplineId == id);
            return discipline == null
                ? NotFound()
                : PartialView("_Edit", discipline);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Discipline discipline)
        {
            if (id != discipline.DisciplineId)
            {
                return NotFound();
            }

            if (await DisciplineNameExistsAsync(discipline.DisciplineName, id))
            {
                ModelState.AddModelError(nameof(Discipline.DisciplineName),
                    "Tên ngành đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Edit", discipline);
            }

            var existingDiscipline = await _context.Disciplines
                .FirstOrDefaultAsync(d => d.DisciplineId == id);
            if (existingDiscipline == null)
            {
                return NotFound();
            }

            existingDiscipline.DisciplineName = discipline.DisciplineName;

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật ngành thành công." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DisciplineExistsAsync(id))
                {
                    return NotFound();
                }

                return Conflict(new
                {
                    message = "Dữ liệu ngành đã được thay đổi bởi người dùng khác. Vui lòng tải lại và thử lại."
                });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể cập nhật ngành. Vui lòng kiểm tra dữ liệu và thử lại."
                });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .FirstOrDefaultAsync(d => d.DisciplineId == id);
            return discipline == null ? NotFound() : View(discipline);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DisciplineId == id);
            return discipline == null
                ? NotFound()
                : PartialView("_Delete", discipline);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null)
            {
                return NotFound();
            }

            try
            {
                _context.Disciplines.Remove(discipline);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Xóa ngành thành công." });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể xóa vì đang có dữ liệu liên quan."
                });
            }
        }

        private async Task<List<Discipline>> GetDisciplinesPageAsync(
            string searchString,
            int page,
            int pageSize)
        {
            pageSize = pageSize < 1 ? 5 : pageSize;
            page = page < 1 ? 1 : page;
            searchString = searchString?.Trim() ?? string.Empty;

            var query = _context.Disciplines
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(d => d.DisciplineName.Contains(searchString));
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
                .OrderBy(d => d.DisciplineId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private async Task<bool> DisciplineExistsAsync(string id)
        {
            return await _context.Disciplines.AnyAsync(d => d.DisciplineId == id);
        }

        private async Task<bool> DisciplineNameExistsAsync(string name, string? excludedId = null)
        {
            var normalizedName = name.Trim();
            return await _context.Disciplines.AnyAsync(d =>
                d.DisciplineName == normalizedName &&
                (excludedId == null || d.DisciplineId != excludedId));
        }
    }
}
