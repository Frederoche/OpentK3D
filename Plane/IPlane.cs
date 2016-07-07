using Camera;
using CustomVertex;
using Lights;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace PrimitiveShapes
{
    public interface IPlane
    {
        float Width { get; set; }
        float Height { get; set; }
        float Depth { get; set; }
        bool IsRender { get; set; }

        int MyProgramHandler { get; set; }
        Vector3[] Points { get;  }
        void Load(BufferUsageHint bufferusageHint = BufferUsageHint.StaticDraw);
        void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, float[] clipPlaneReflection = null, float[] clipPlaneRefraction = null, ILight light = null);
    }
}
