using OpenTK;

namespace Camera
{
    public class CameraFactory
    {
        public static ICamera Create(Vector3 cameraPosition, Vector3 cameraLookAt, Vector3 upVector)
        {
            return new Camera(cameraPosition, cameraLookAt, upVector);
        }
    }
}
