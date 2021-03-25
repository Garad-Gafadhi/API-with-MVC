using System.Collections.Generic;
using VOD.Common.DTOModels;

namespace VOD.UI.Models.MembershipViewModel
{
    public class DashboardViewModel
    {
        public List<List<CourseDTO>> Courses { get; set; }
    }
}