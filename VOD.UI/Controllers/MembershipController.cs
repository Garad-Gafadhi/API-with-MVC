using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.UI.Models.MembershipViewModel;
using VOD.UI.Services;

namespace VOD.UI.Controllers
{
    public class MembershipController : Controller
    {
        #region Cunstructors

        public MembershipController(IHttpContextAccessor httpContextAccessor, UserManager<VodUser> userManager,
            IMapper mapper, IUiReadService db)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _db = db;
        }

        #endregion

        #region Properties & Variables

        private readonly IMapper _mapper;
        private readonly IUiReadService _db;
        private readonly string _userId;

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var courseObjects = _mapper.Map<List<CourseDTO>>(
                (await _db.GetCoursesAsync(_userId)).OrderBy(o => o.Title));
            var dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.Courses = new List<List<CourseDTO>>();

            var nrOfRows = courseObjects.Count <= 3 ? 1 : courseObjects.Count / 3;

            for (var i = 0; i < nrOfRows; i++)
                dashboardViewModel.Courses.Add(courseObjects.Skip(i * 3).Take(3).ToList());

            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Course(int id)
        {
            var course = await _db.GetCourseAsync(_userId, id);
            var CourseDTO = _mapper.Map<CourseDTO>(course);
            var InstructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);
            var ModuleDTO = _mapper.Map<List<ModuleDTO>>(course.Modules.OrderBy(o => o.Title));

            var courseViewModel = new CourseViewModel
            {
                Course = CourseDTO,
                Instructor = InstructorDTO,
                Module = ModuleDTO
            };

            return View(courseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Video(int id)
        {
            var video = await _db.GetVideoAsync(_userId, id);
            var VideoDTO = _mapper.Map<VideoDTO>(video);
            var CourseDTO = _mapper.Map<CourseDTO>(video.Course);
            var InstructorDTO = _mapper.Map<InstructorDTO>(video.Course.Instructor);

            var videos = video.Module.Videos;
            var count = videos.Count();

            var index = videos.FindIndex(v => v.Id.Equals(id));
            var previous = videos.ElementAtOrDefault(index - 1);
            var previousId = previous == null ? 0 : previous.Id;

            var next = videos.ElementAtOrDefault(index + 1);
            var nextId = next == null ? 0 : next.Id;
            var nextTitle = next == null ? string.Empty : next.Title;
            var nextThumb = next == null ? string.Empty : next.Thumbnail;

            var videoViewModel = new VideoViewModel
            {
                Video = VideoDTO,
                Instructor = InstructorDTO,
                Course = CourseDTO,
                LessonInfo = new LessonInfoDTO
                {
                    LessonNumber = index + 1,
                    NumberOfLessons = count,
                    NextVideoOld = nextId,
                    PreviousVideoOld = previousId,
                    NextVideoTitle = nextTitle,
                    NextVideoThumbnail = nextThumb,
                    CurrentVideoTitle = video.Title,
                    CurrentVideoThumbnail = video.Thumbnail
                }
            };

            return View(videoViewModel);
        }

        #endregion
    }
}