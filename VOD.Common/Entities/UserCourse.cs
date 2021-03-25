﻿namespace VOD.Common.Entities
{
    public class UserCourse
    {
        public string UserId { get; set; }
        public VodUser User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}