using Landscape;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Textures;
using Utilities;

namespace UnitTest
{
    [TestClass]
    public class Texture_Test
    {
        [TestMethod]
        public void Texture_OK()
        {
            //Act
            ITexture texture = TextureFactory.Create(Utils.GetImageResource<ITerrain>("Landscape.Terrains.Dirt.jpg"), TextureWrapMode.ClampToEdge);

            //Assert
            Bitmap bitmap = new Bitmap(Utils.GetImageResource<ITerrain>("Landscape.Terrains.Dirt.jpg"));
            VerifyImage(bitmap, texture.TextureImage);
        }

        private void VerifyImage(Bitmap expectedBitmap, Bitmap actualBitmap)
        {
            Assert.AreEqual(expectedBitmap.Height, actualBitmap.Height, "Bitmap Height");
            Assert.AreEqual(expectedBitmap.Width, actualBitmap.Width, "BitmapWidth");
            Assert.AreEqual(expectedBitmap.RawFormat, actualBitmap.RawFormat, "Raw Format");
            Assert.AreEqual(expectedBitmap.HorizontalResolution, actualBitmap.HorizontalResolution, "Horizontal resolution");
        }
    }
}
