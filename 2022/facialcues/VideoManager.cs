using facialcues.Entity;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace facialcues
{
    public class VideoManager
    {
        IFaceClient faceClient;
        private bool Authenticate()
        {
            if (null == faceClient)
                faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Constants.FaceApiKey)) { Endpoint = Constants.FaceApiEndPoint };
            return null != faceClient;
        }
    }
}
