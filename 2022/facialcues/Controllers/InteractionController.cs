using facialcues.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace facialcues.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InteractionController : ControllerBase
    {
        private readonly ILogger<InteractionController> _logger;

        public InteractionController(ILogger<InteractionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{videoName}", Name = "GetFacialExpressions")]
        public string Get(string videoName)
        {
            var ed = new ExpressionDetector();
            var vm = new VideoManager();
            var videoUrl = Constants.FacesBlobUri + "/" + videoName;
            //var videoFrames = await vm.SplitViedoIntoFrames(videoUrl);
            //foreach(var frame in videoFrames)
            //{
            //    frame.Faces = await ed.GetExression(frame.ImageUrl);                
            //}
            return "work in progress";
        }
    }
}
