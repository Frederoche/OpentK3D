using Camera;
using Lights;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Landscape
{
    public interface ITerrain
    {
        int Width { get; set; }
        int Height { get; set; }

        Vector3 Center { get; set; }

        void Load(bool isNoise=false, bool isVoronoi = false);
        void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 viewMatrix, Matrix4 projectionMatrix, ILight light, bool isRendered);
        void SmoothFilter();
    }
}
