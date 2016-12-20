using CustomVertex;
using Landscape;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using PrimitiveShapes;
using System.Drawing;
using Utilities;

namespace UnitTest
{
    [TestClass]
    public class Plane_Test
    {
        [TestMethod]
        public void Plane_OK()
        {
            Image image = Utils.Utils.GetImageResource<Terrain>("Landscape.Terrains.Grass.png");
            

            IPlane plane = PlaneFactory.Create(false,
                new VBO() { Position = new Vector3(-1, 0, -1), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 0)},
                new VBO() { Position = new Vector3(-1, 0, 1), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1)},
                new VBO() { Position = new Vector3(1, 0, 1), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1)},
                new VBO() { Position = new Vector3(1, 0, -1), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1)}, image, OpenTK.Graphics.OpenGL.TextureWrapMode.Clamp);

            Assert.AreEqual(2, plane.Width, "Plane Width");
            Assert.AreEqual(0, plane.Height, "Plane Height");
            Assert.AreEqual(2, plane.Depth, "Plane Depth");
        }
    }
}
