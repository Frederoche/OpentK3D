using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Textures;

namespace FrameBufferObject
{
    public interface IFrameBufferObject
    {
        ITexture FrameBufferTexture { get; set; }
        int TextureHandle { get; set; }
        void Load();
        void GenerateProjectiveTexture(BeginMode mode, Matrix4 viewMatrix, Matrix4 projectionMatrix, float[] clipPlaneEq, Action<BeginMode, Matrix4, Matrix4, Matrix4, float[]> renderFunc);
    }
}
