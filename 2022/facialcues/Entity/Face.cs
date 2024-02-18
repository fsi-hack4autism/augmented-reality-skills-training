namespace facialcues.Entity
{
    public class Face
    {
        public bool Smile { get; set; }
        public string Emotion { get; set; }
        public string HeadPose { get; set; }
        public string ImageUrl { get; set; }
        public Face()
        {
            Smile = false;
            Emotion = "Neutral";
            HeadPose = "Straight";
        }
    }
}
