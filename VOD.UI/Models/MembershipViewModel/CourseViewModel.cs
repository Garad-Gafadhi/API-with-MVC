using System.Collections.Generic;
using VOD.Common.DTOModels;

namespace VOD.UI.Models.MembershipViewModel
{
    public class CourseViewModel
    {
        public CourseDTO Course { get; set; }
        public InstructorDTO Instructor { get; set; }

        //public DownloadDTO Download { get; set; }
        public IEnumerable<ModuleDTO> Module { get; set; }
    }
}