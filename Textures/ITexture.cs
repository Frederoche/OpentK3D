using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Textures
{
    public interface ITexture
    {
        string TexturePath { get; set; }
        Bitmap TextureImage { get; set; }
        TextureWrapMode TextureWrapMode { get; set; }
        int MyTextureHandle { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        void Load();
        void LoadEmptyTexture(PixelInternalFormat pixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat pixelFormat, bool isAnisotropic);
        int LoadData(SizedInternalFormat format,float[,] dataArray);
    }
}
