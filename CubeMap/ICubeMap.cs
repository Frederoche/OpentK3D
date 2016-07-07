using Frustum;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PrimitiveShapes;
using System.Collections.Generic;

namespace EnvironmentMap
{
    public interface ICubeMap
    {
        float Width { get; set; }

        IPlane BackPlane { get; set; }
        IPlane FrontPlane { get; set; }
        IPlane TopPlane { get; set; }
        IPlane LeftPlane { get; set; }
        IPlane RightPlane { get; set; }
        Vector3 Center { get; set; }
        List<IPlane> Planes { get; set; }

        int[] MyProgramHandler { get; set; }

        void Load();
        void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, bool isShown, IFrustum frustum = null, float[] clipPlaneEquation = null);
    }
}
