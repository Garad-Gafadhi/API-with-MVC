using VOD.Common.DTOModels;

namespace VOD.UI.Models.MembershipViewModel
{
    public class VideoViewModel
    {
        public CourseDTO Course { get; set; }
        public InstructorDTO Instructor { get; set; }

        //public ModuleDTO Module { get; set; }
        public LessonInfoDTO LessonInfo { get; set; }

        public VideoDTO Video { get; set; }
    }
}