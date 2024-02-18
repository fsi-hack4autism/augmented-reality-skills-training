using facialcues.Entity;

namespace facialcues
{
    public class FacialCue
    {
        public Face FaceExpression { get; set; }

        public int Rating { get; set; }

        public string Text { get; set; }

        public string? Summary { get; set; }
        public FacialCue()
        {
            FaceExpression = new Face();
            Text = "Unsure";
        }
    }
}