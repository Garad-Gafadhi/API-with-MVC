using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.UI.Controllers
{
    public class ModulesController : Controller
    {
        #region Constructors

        public ModulesController(IAPIService apiService, IMapper mapper)
        {
            _mapper = mapper;
            _apiService = apiService;
        }

        #endregion


        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var modules = await _apiService.GetAsync<Module, ModuleDTO>();
            return View(modules);
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var moduleId = await _apiService.SingleAsync<Module, ModuleDTO>(id.Value, true);
            if (moduleId == null) return NotFound();


            return View(moduleId);
        }


        // GET: Modules/Create
        public async Task<IActionResult> Create()
        {
            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuleDTO moduleDto)
        {
            if (ModelState.IsValid)
            {
                var created = await _apiService.CreateAsync<Module, ModuleDTO>(moduleDto);
                return RedirectToAction(nameof(Index));
            }


            //var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            //ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle", moduleDto.CourseId);
            return View(moduleDto);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var module = await _apiService.SingleAsync<Module, ModuleDTO>(id.Value);
            if (module == null) return NotFound();

            var courseId = await _apiService.GetAsync<Course, CourseDTO>();
            ViewData["CourseId"] = new SelectList(courseId, "Id", "CourseTitle");
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ModuleDTO moduleDto)
        {
            if (id != moduleDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var sucess = await _apiService.UpdateAsync<Module, ModuleDTO>(moduleDto);
                }
                catch
                {
                }

                return RedirectToAction(nameof(Index));
            }

            var course = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(course, "Id", "CourseTitle");

            return View(moduleDto);
        }


        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var moduleDto = await _apiService.SingleAsync<Module, ModuleDTO>(id.Value, true);

            if (moduleDto == null) return NotFound();

            return View(moduleDto);
        }

        // POST: Modules/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await _apiService.SingleAsync<Module, ModuleDTO>(id);
            if (module == null) return NotFound();

            var deleted = await _apiService.DeleteAsync<Module>(module.Id);

            return RedirectToAction(nameof(Index));
        }


        #region Properties

        private readonly IMapper _mapper;
        private readonly IAPIService _apiService;

        #endregion
    }
}