using System.Collections.Generic;
using System.Threading.Tasks;
using VOD.Common.Entities;

namespace VOD.UI.Services
{
    public interface IUiReadService
    {
        Task<IEnumerable<Course>> GetCoursesAsync(string userId);

        Task<Course> GetCourseAsync(string userId, int courseId);

        Task<IEnumerable<Video>> GetVideosAsync(string userId, int moduleId = default);

        Task<Video> GetVideoAsync(string userId, int videoId);
    }
}