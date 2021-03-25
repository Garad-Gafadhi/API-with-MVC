namespace VOD.Common.DTOModels
{
    public class LessonInfoDTO
    {
        public int LessonNumber { get; set; }
        public int NumberOfLessons { get; set; }
        public int PreviousVideoOld { get; set; }
        public int NextVideoOld { get; set; }
        public string NextVideoTitle { get; set; }
        public string NextVideoThumbnail { get; set; }
        public string CurrentVideoTitle { get; set; }
        public string CurrentVideoThumbnail { get; set; }
    }
}