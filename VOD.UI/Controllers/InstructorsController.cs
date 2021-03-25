using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.UI.Controllers
{
    public class InstructorsController : Controller
    {
        #region Constructor

        public InstructorsController(IAPIService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
        }

        #endregion


        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            var instructors = await _apiService.GetAsync<Instructor, InstructorDTO>();

            return View(instructors);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instructorDto = await _apiService.SingleAsync<Instructor, InstructorDTO>(id.Value);
            if (instructorDto == null) return NotFound();

            return View(instructorDto);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorDTO instructorDto)
        {
            if (ModelState.IsValid)
            {
                var instructorId = await _apiService.CreateAsync<Instructor, InstructorDTO>(instructorDto);
                return RedirectToAction(nameof(Index));
            }

            return View(instructorDto);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null && id.Value < 1) return NotFound();

            var instructor = await _apiService.SingleAsync<Instructor, InstructorDTO>(id.Value);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InstructorDTO instructorDto)
        {
            if (id != instructorDto.Id) return NotFound("The id's don't match");

            if (ModelState.IsValid)
            {
                try
                {
                    var updated = await _apiService.UpdateAsync<Instructor, InstructorDTO>(instructorDto);
                }
                catch
                {
                }

                return RedirectToAction(nameof(Index));
            }

            return View(instructorDto);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id.Value < 1) return NotFound();

            var instructorDto = await _apiService.SingleAsync<Instructor, InstructorDTO>(id.Value);
            if (instructorDto == null) return NotFound();

            return View(instructorDto);
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructorDto = await _apiService.SingleAsync<Instructor, InstructorDTO>(id);
            if (instructorDto == null) return NotFound("There's no instructor to Delete");
            var deleted = await _apiService.DeleteAsync<Instructor>(instructorDto.Id);

            return RedirectToAction(nameof(Index));
        }

        #region Properties

        private readonly IMapper _mapper;
        private readonly IAPIService _apiService;

        #endregion
    }
}