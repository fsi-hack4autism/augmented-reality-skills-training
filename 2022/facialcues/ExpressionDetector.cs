using facialcues.Entity;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace facialcues
{
    public class ExpressionDetector
    {
        IFaceClient faceClient;
        const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
        private bool Authenticate()
        {
            if (null == faceClient)
                faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Constants.FaceApiKey)) { Endpoint = Constants.FaceApiEndPoint };
            return null != faceClient;
        }
        public async Task<List<Face>> GetExression(string imgUrl)
        {
            if (!Authenticate()) return new List<Face> { new Face { ImageUrl = imgUrl } };
            return await DetectFaceExtract(imgUrl);
        }

        private async Task<List<Face>> DetectFaceExtract(string imgUrl)
        {
            var detectedFaces = await faceClient.Face.DetectWithUrlAsync(imgUrl,
                returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile,
                FaceAttributeType.Smile, FaceAttributeType.QualityForRecognition },
                // We specify detection model 1 because we are retrieving attributes.
                detectionModel: DetectionModel.Detection01,
                recognitionModel: RECOGNITION_MODEL4);
            var faces = new List<Face>();
            foreach (var detectedFace in detectedFaces)
            {
                var face = new Face { ImageUrl = imgUrl};
                face.Smile = detectedFace.FaceAttributes.Smile > 0.5;
                if (detectedFace.FaceAttributes.Emotion.Happiness > 0.5) face.Emotion = "Happiness";
                else if (detectedFace.FaceAttributes.Emotion.Sadness > 0.5) face.Emotion = "Sadness";
                else if (detectedFace.FaceAttributes.Emotion.Disgust > 0.5) face.Emotion = "Disgust";
                else if (detectedFace.FaceAttributes.Emotion.Contempt > 0.5) face.Emotion = "Contempt";
                else if (detectedFace.FaceAttributes.Emotion.Anger > 0.5) face.Emotion = "Anger";
                else if (detectedFace.FaceAttributes.Emotion.Fear > 0.5) face.Emotion = "Fear";
                else face.Emotion = "Neutral";
                faces.Add(face);
            }
            return faces;
        }
    } 
}