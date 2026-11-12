using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString = "", int page = 1, int pageSize = 5)
        {
            var students = await GetStudentsPageAsync(searchString, page, pageSize);
            return View(students);
        }

        public async Task<IActionResult> Table(string searchString = "", int page = 1, int pageSize = 5)
        {
            var students = await GetStudentsPageAsync(searchString, page, pageSize);
            return PartialView("_StudentTable", students);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateLookupDataAsync();
            return PartialView("_Create", new Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                await PopulateLookupDataAsync();
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Create", student);
            }

            var nam = student.EntryYear >= 100 ? student.EntryYear % 100 : student.EntryYear;
            var count = await _context.Students.CountAsync();
            student.StudentId = $"{nam:D2}{student.FormId:D2}{student.DisciplineId:D2}{count + 1:D4}";

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm sinh viên thành công." });
            }
            catch (DbUpdateException)
            {
                _context.Entry(student).State = EntityState.Detached;
                await PopulateLookupDataAsync();
                ModelState.AddModelError(string.Empty,
                    "Không thể tạo sinh viên vì mã sinh viên bị trùng hoặc dữ liệu không hợp lệ. Vui lòng thử lại.");
                return Conflict(new
                {
                    message = "Mã sinh viên vừa được tạo đã tồn tại hoặc dữ liệu không hợp lệ. Vui lòng thử lại."
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            await PopulateLookupDataAsync();
            return PartialView("_Edit", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateLookupDataAsync();
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_Edit", student);
            }

            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FullName = student.FullName;
            existingStudent.Address = student.Address;
            existingStudent.University = student.University;
            existingStudent.EntryYear = student.EntryYear;
            existingStudent.DepartmentId = student.DepartmentId;
            existingStudent.DisciplineId = student.DisciplineId;
            existingStudent.FormId = student.FormId;

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật sinh viên thành công." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExistsAsync(student.StudentId))
                {
                    return NotFound();
                }

                return Conflict(new
                {
                    message = "Dữ liệu sinh viên đã được thay đổi bởi người dùng khác. Vui lòng tải lại và thử lại."
                });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể cập nhật sinh viên. Vui lòng kiểm tra dữ liệu và thử lại."
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == id);
            return student == null ? NotFound() : PartialView("_Delete", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Xóa sinh viên thành công." });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể xóa sinh viên này. Vui lòng thử lại."
                });
            }
        }

        private async Task<List<Student>> GetStudentsPageAsync(
            string searchString,
            int page,
            int pageSize)
        {
            pageSize = pageSize < 1 ? 5 : pageSize;
            page = page < 1 ? 1 : page;
            searchString = searchString?.Trim() ?? string.Empty;

            var query = _context.Students.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(s =>
                    s.StudentId.Contains(searchString) ||
                    s.FullName.Contains(searchString));
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
                .OrderBy(s => s.StudentId)
                .Include(s => s.Department)
                .Include(s => s.Discipline)
                .Include(s => s.FormOfTraining)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private async Task PopulateLookupDataAsync()
        {
            ViewData["Departments"] = await _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId,
                    Text = d.DepartmentName
                })
                .ToListAsync();

            ViewData["Disciplines"] = await _context.Disciplines
                .Select(d => new SelectListItem
                {
                    Value = d.DisciplineId,
                    Text = d.DisciplineName
                })
                .ToListAsync();

            ViewData["FormOfTraining"] = await _context.FormOfTrainings
                .Select(f => new SelectListItem
                {
                    Value = f.FormId,
                    Text = f.FormName
                })
                .ToListAsync();
        }

        private async Task<bool> StudentExistsAsync(string id)
        {
            return await _context.Students.AnyAsync(s => s.StudentId == id);
        }

    }
}
