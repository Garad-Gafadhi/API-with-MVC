namespace VOD.Common.DTOModels
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string MarqueeImageUrl { get; set; }
        public string CourseImageUrl { get; set; }
        public int InstructorId { get; set; }
    }
}