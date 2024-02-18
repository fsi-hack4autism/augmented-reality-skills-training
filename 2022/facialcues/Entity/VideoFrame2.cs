namespace facialcues.Entity
{
    public class VideoFrame2
    {
        public string Time { get; set; }
        public string ImageUrl { get; set; }
        public string ParentVideoUrl { get; set; }
        public List<Face> Faces { get; set; }
    }
}
