using Camera;
using FrameBufferObject;
using Lights;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Landscape
{
    public interface IWater
    {
        int Width { get; set; }
        int Height { get; set; }
        Vector3 Center { get; set; }

        IFrameBufferObject Refraction { get; set; }
        IFrameBufferObject Reflection { get; set; }

        float[] ReflectionClipPlane { get; set; }
        float[] RefractionClipPlane { get; set; }

        void Load();
        void MakeReflectionTextures(BeginMode mode, Matrix4 viewMatrix, Matrix4 invertedViewMatrix, Matrix4 projectionMatrix, float[] clipPlaneEq, Action<BeginMode, Matrix4, Matrix4, Matrix4, float[]> renderFunc);
        void MakeRefractionTextures(BeginMode mode, Matrix4 viewMatrix, Matrix4 invertedViewMatrix, Matrix4 projectionMatrix, float[] clipPlaneEq, Action<BeginMode, Matrix4, Matrix4, Matrix4, float[]> renderFunc);
        void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 viewMatrix, Matrix4 projectionMatrix, ILight light, ICamera camera, bool isOceanOn);
    }
}
