using Camera;
using Frustum;
using OpenTK;

namespace Frustum
{
    public interface IFrustum
    {
       Plane Top { get; set; }
       Plane Bottom { get; set; }
       Plane Far { get; set; }
       Plane Near { get; set; }
       Plane Left { get; set; }
       Plane Right { get; set; }

       void ExctractPlanes(ICamera camera);
       int PointInFrustum(Vector3 point);
       int SquareInFrustum(params Vector3[] points);
       void Load();
       void ShowFrustum(Matrix4 projectionMatrix, Matrix4 viewMatrix);
    }
}
