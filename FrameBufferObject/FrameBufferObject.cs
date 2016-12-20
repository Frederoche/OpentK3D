using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Textures;

namespace FrameBufferObject
{
    public class FrameBufferObject : IFrameBufferObject
    {
        public int TextureHandle { get; set; }

        private int FrameBufferHandle { get; set; }

        public ITexture FrameBufferTexture { get; set; }

        private int Width{get;}
        private int Height { get;  }

        public FrameBufferObject(int width, int height)
        {
            FrameBufferTexture = TextureFactory.Create(TextureWrapMode.ClampToEdge, width, height); 
            FrameBufferTexture.LoadEmptyTexture(PixelInternalFormat.Rgba, PixelFormat.Bgra, true);
            TextureHandle = FrameBufferTexture.MyTextureHandle;
            Width = width;
            Height = height;
        }

        public void Load()
        {
            int frameBufferHandle;
            GL.GenFramebuffers(1, out frameBufferHandle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, frameBufferHandle);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, TextureHandle ,0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            FrameBufferHandle = frameBufferHandle;
        }

        public void GenerateProjectiveTexture(BeginMode mode, Matrix4 viewMatrix, Matrix4 projectionMatrix, float[] clipPlaneEq, Action<BeginMode, Matrix4, Matrix4, Matrix4, float[]> renderFunc)
        {
            Begin();
            {
                GL.Disable(EnableCap.CullFace);
                {
                    renderFunc(mode, Matrix4.Identity, projectionMatrix, viewMatrix, clipPlaneEq);
                }
                GL.Enable(EnableCap.CullFace);
            }
            End();
            GL.LoadIdentity();
        }

        private void Begin()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FrameBufferHandle);
            GL.PushAttrib(AttribMask.ViewportBit | AttribMask.EnableBit);
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, Width, 0, Height, -1, 1);
        }

        private void End()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();

            GL.PopAttrib();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
        }
    }
}
