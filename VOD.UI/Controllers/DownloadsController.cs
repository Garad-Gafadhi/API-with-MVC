using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.UI.Controllers
{
    public class DownloadsController : Controller
    {
        #region Constructors

        public DownloadsController(IMapper mapper, IAPIService apiService)
        {
            _mapper = mapper;
            _apiService = apiService;
        }

        #endregion

        // GET: Downloads
        public async Task<IActionResult> Index()
        {
            var downloadDtOs = await _apiService.GetAsync<Download, DownloadDTO>(true);
            return View(downloadDtOs);
        }

        // GET: Downloads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var downloadDto = await _apiService.SingleAsync<Download, DownloadDTO>(id.Value, true);
            if (downloadDto == null) return NotFound();

            return View(downloadDto);
        }

        // GET: Downloads/Create
        public async Task<IActionResult> Create()
        {
            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle");

            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");
            return View();
        }

        // POST: Downloads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DownloadDTO downloadDto)
        {
            if (ModelState.IsValid)
            {
                var createdId = await _apiService.CreateAsync<Download, DownloadDTO>(downloadDto);
                return RedirectToAction(nameof(Index));
            }


            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle", downloadDto.CourseId);

            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle", downloadDto.ModuleId);

            return View(downloadDto);
        }

        // GET: Downloads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var downloadDto = await _apiService.SingleAsync<Download, DownloadDTO>(id.Value);
            if (downloadDto == null) return NotFound();

            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");

            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle");

            return View(downloadDto);
        }

        // POST: Downloads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DownloadDTO downloadDto)
        {
            if (id != downloadDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var edited = await _apiService.UpdateAsync<Download, DownloadDTO>(downloadDto);
                }
                catch
                {
                }

                return RedirectToAction(nameof(Index));
            }

            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle");

            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");

            return View(downloadDto);
        }

        // GET: Downloads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var downloadDto = await _apiService.SingleAsync<Download, DownloadDTO>(id.Value);

            if (downloadDto == null) return NotFound();

            return View(downloadDto);
        }

        // POST: Downloads/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var downloadDto = await _apiService.SingleAsync<Download, DownloadDTO>(id);
            if (downloadDto == null) return NotFound();


            var deleted = await _apiService.DeleteAsync<Download>(id);
            return RedirectToAction(nameof(Index));
        }


        #region Properties

        private readonly IMapper _mapper;
        private readonly IAPIService _apiService;

        #endregion
    }
}