using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.UI.Controllers
{
    public class CoursesController : Controller
    {
        #region Constructors

        public CoursesController(IMapper mapper, IAPIService apiService)
        {
            _mapper = mapper;
            _apiService = apiService;
        }

        #endregion


        #region Methods

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _apiService.GetAsync<Course, CourseDTO>(true);
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var courseDto = await _apiService.SingleAsync<Course, CourseDTO>(id.Value, true);
            if (courseDto == null) return NotFound();


            return View(courseDto);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            var instructors = await _apiService.GetAsync<Instructor, InstructorDTO>(true);
            ViewData["InstructorId"] = new SelectList(instructors, "Id", "InstructorName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDTO courseDto)
        {
            if (ModelState.IsValid)
            {
                var courseId = await _apiService.CreateAsync<Course, CourseDTO>(courseDto);
                return RedirectToAction(nameof(Index));
            }

            var instructors = await _apiService.GetAsync<Instructor, InstructorDTO>(true);
            ViewData["InstructorId"] = new SelectList(instructors, "Id", "InstructorName", courseDto.InstructorId);
            return View(courseDto);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id.Value < 1) return NotFound();

            var CourseDTO = await _apiService.SingleAsync<Course, CourseDTO>(id.Value, true);
            if (CourseDTO == null) return NotFound();

            var instructors = await _apiService.GetAsync<Instructor, InstructorDTO>(true);
            ViewData["InstructorId"] = new SelectList(instructors, "Id", "InstructorName");
            return View(CourseDTO);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var sucess = await _apiService.UpdateAsync<Course, CourseDTO>(courseDto);
                }
                catch
                {
                }

                return RedirectToAction(nameof(Index));
            }

            var instructors = await _apiService.GetAsync<Instructor, InstructorDTO>(true);
            ViewData["InstructorId"] = new SelectList(instructors, "Id", "InstructorName");

            return View(courseDto);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id.Value < 1) return NotFound();

            var courseDto = await _apiService.SingleAsync<Course, CourseDTO>(id.Value, true);
            if (courseDto == null) return NotFound();

            return View(courseDto);
        }

        // POST: Courses/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseDto = await _apiService.SingleAsync<Course, CourseDTO>(id);
            if (courseDto == null) return NotFound();

            var sucess = await _apiService.DeleteAsync<Course>(courseDto.Id);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Properties

        private readonly IMapper _mapper;
        private readonly IAPIService _apiService;

        #endregion
    }
}