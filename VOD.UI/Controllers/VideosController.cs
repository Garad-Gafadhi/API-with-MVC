using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.UI.Controllers
{
    public class VideosController : Controller
    {
        #region Constructor

        public VideosController(IMapper mapper, IAPIService apiService)
        {
            _mapper = mapper;
            _apiService = apiService;
        }

        #endregion

        #region Properties

        private readonly IMapper _mapper;
        private readonly IAPIService _apiService;

        #endregion

        #region Methods

        // GET: Videos
        public async Task<IActionResult> Index()
        {
            var videos = await _apiService.GetAsync<Video, VideoDTO>(true);
            return View(videos);
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var videoDto = await _apiService.SingleAsync<Video, VideoDTO>(id.Value, true);

            if (videoDto == null) return NotFound();


            return View(videoDto);
        }

        // GET: Videos/Create
        public async Task<IActionResult> Create()
        {
            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle");

            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoDTO videoDto)
        {
            if (ModelState.IsValid)
            {
                var videos = await _apiService.CreateAsync<Video, VideoDTO>(videoDto);
                return RedirectToAction(nameof(Index));
            }

            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle");

            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");
            return View(videoDto);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var downloadDto = await _apiService.SingleAsync<Video, VideoDTO>(id.Value);
            if (downloadDto == null) return NotFound();

            var courseDto = await _apiService.GetAsync<Course, CourseDTO>(true);
            ViewData["CourseId"] = new SelectList(courseDto, "Id", "CourseTitle");

            var moduleDto = await _apiService.GetAsync<Module, ModuleDTO>(true);
            ViewData["ModuleId"] = new SelectList(moduleDto, "Id", "ModuleTitle");

            return View(downloadDto);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideoDTO videoDto)
        {
            if (id != videoDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var edited = await _apiService.UpdateAsync<Video, VideoDTO>(videoDto);
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

            return View(videoDto);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id.Value < 1) return NotFound();

            var VideoDTO = await _apiService.SingleAsync<Video, VideoDTO>(id.Value, true);
            if (VideoDTO == null) return NotFound();

            return View(VideoDTO);
        }

        // POST: Videos/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var VideoDTO = await _apiService.SingleAsync<Video, VideoDTO>(id, true);
            if (VideoDTO == null) return NotFound();

            var sucess = await _apiService.DeleteAsync<Video>(VideoDTO.Id);
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}