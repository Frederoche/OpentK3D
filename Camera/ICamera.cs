using OpenTK;

namespace Camera
{
    public interface ICamera
    {
        Vector3 CameraPosition { get; set; }
        Vector3 CameraLookAt { get; set; }
        Vector3 UpVector { get; set; }
    }
}
