using Lights;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics;

namespace UnitTest
{
    [TestClass]
    public class Light_Test
    {
        [TestMethod]
        public void Light_OK()
        {
            //Act
            ILight light = LightFactory.Create(new Vector3(0, 1, 0), new Color4(250, 248, 248, 1), new Color4(250, 246, 247, 1), new Color4(0, 1, 0, 1), OpenTK.Graphics.OpenGL.LightName.Light0);

            //Assert
            Assert.AreEqual(new Vector3(0, 1, 0), light.LightPosition, "LightPosition");
            Assert.AreEqual(new Color4(250, 248, 248, 1), light.AmbientColor, "AmbientColor");
            Assert.AreEqual(new Color4(250, 246, 247, 1), light.DiffuseColor, "DiffuseColor");
            Assert.AreEqual(new Color4(0, 1, 0, 1), light.SpecularColor, "SpecularColor");
        }
    }
}
