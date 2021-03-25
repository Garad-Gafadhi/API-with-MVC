namespace VOD.Common.DTOModels
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public int Duration { get; set; }
        public string Url { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
    }
}