using System.ComponentModel.DataAnnotations;

namespace VOD.Common.Entities
{
    public class Video
    {
        [Key] public int Id { get; set; }

        [MaxLength(80)] [Required] public string Title { get; set; }

        [MaxLength(800)] public string Description { get; set; }

        public int Duration { get; set; }
        [MaxLength(800)] public string Thumbnail { get; set; }

        [MaxLength(800)] public string Url { get; set; }

        public int CourseId { get; set; }
        public int ModuleId { get; set; }

        public Course Course { get; set; }
        public Module Module { get; set; }
    }
}