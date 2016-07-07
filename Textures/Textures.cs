using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;


namespace Textures
{
    public class Textures : ITexture
    {
        public string TexturePath { get; set; }
        public Bitmap TextureImage { get; set; }
        public TextureWrapMode TextureWrapMode { get; set; }
        public Image Image { get; set; }
        public int MyTextureHandle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int MyBufferHandle {get;set;}

        public Textures(string texturePath, TextureWrapMode textureWrapMode)
        {
            TexturePath = texturePath;
            TextureImage = new Bitmap(TexturePath);
            TextureWrapMode = textureWrapMode;
            Height = TextureImage.Width;
            Width = TextureImage.Height;
        }

        public Textures(Image image, TextureWrapMode textureWrapMode)
        {
            TextureImage = new Bitmap(image);
            TextureWrapMode = textureWrapMode;
            Height = TextureImage.Width;
            Width = TextureImage.Height;
        }

        public Textures(TextureWrapMode textureWrapMode, int width, int height)
        {
            TextureWrapMode = textureWrapMode;
            Width = width;
            Height = height;
        }

        public Textures() { }

        public void Load()
        {
            if (TextureImage == null)
                return;

            int myTextureHandle = 0;
            BitmapData bitmapData = TextureImage.LockBits(new Rectangle(0, 0, TextureImage.Width, TextureImage.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GenTextures(1, out myTextureHandle);
            GL.BindTexture(TextureTarget.Texture2D, myTextureHandle);
  
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureImage.Width, TextureImage.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode);
           
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName) ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, (float) 16.0);
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName) ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, (float)16.0);

            MyTextureHandle = myTextureHandle;
            TextureImage.UnlockBits(bitmapData);
            
        }

        public int LoadData(SizedInternalFormat format,  float[,] dataArray)
        {
            int myTextureHandle = 0;
            GL.GenTextures(1, out myTextureHandle);
            GL.BindTexture(TextureTarget.Texture1D, myTextureHandle);

            GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba16f, dataArray.Length, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Float, dataArray);

            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapT, (int)TextureWrapMode);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode);

            MyTextureHandle = myTextureHandle;
            return myTextureHandle;
        }


        public void LoadEmptyTexture(PixelInternalFormat pixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat pixelFormat, bool isAnisotropic)
        {
            int myTextureHandle = 0;
            GL.GenTextures(1, out myTextureHandle);
            GL.BindTexture(TextureTarget.Texture2D, myTextureHandle);

            GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, Width, Height, 0, pixelFormat, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode);

            if (isAnisotropic)
            {
                GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, (float)16.0);
                GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, (float)16.0);
            }

            MyTextureHandle = myTextureHandle;
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
