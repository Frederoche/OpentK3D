using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Textures
{
    public class TextureFactory
    {
        public static ITexture Create(string texturePath, TextureWrapMode textureWrapMode)
        {
            return new Textures(texturePath, textureWrapMode);
        }

        public static ITexture Create(Image image, TextureWrapMode textureWrapMode)
        {
            return new Textures(image, textureWrapMode);
        }

        public static ITexture Create(TextureWrapMode textureWrapMode, int width, int height)
        {
            return new Textures(textureWrapMode, width, height);
        }

        public static ITexture Create()
        {
            return new Textures();
        }
    }
}
