namespace VOD.Common.DTOModels
{
    public class DownloadDTO
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }

        public int CourseId { get; set; }
        public string DownloadUrl { get; set; }
        public string DownloadTitle { get; set; }
    }
}