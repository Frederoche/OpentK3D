using OpenTK;

namespace Camera
{
    public class Camera : ICamera
    {
        public Vector3 CameraPosition{ get; set;}
        public Vector3 CameraLookAt { get; set; }
        public Vector3 UpVector { get; set; }

        public Camera(Vector3 cameraPosition, Vector3 cameraLookAt, Vector3 upVector)
        {
            CameraPosition = cameraPosition;
            CameraLookAt = cameraLookAt;
            UpVector = upVector;
        }
    }
}
