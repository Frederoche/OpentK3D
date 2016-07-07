using EnvironmentMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using PrimitiveShapes;
using Utilities;

namespace UnitTest
{   
    [TestClass]
    public class CubeMap_Test
    {
        [TestMethod]
        public void CubeMap_OK()
        {
            //Act
            ICubeMap cubeMap = CubeMapFactory.Create(100, false, new Vector3(0,0,0),
                Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Front.bmp"),
                Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Left.bmp"),
                Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Front.bmp"),
                Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Top.bmp"),
                Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Back.bmp"),
                Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Right.bmp"));

            //Assert
            Assert.AreEqual(new Vector3(0, 0, 0), cubeMap.Center, "CubeMap center");
            Assert.AreEqual(100, cubeMap.Width);
            Assert.AreEqual(typeof(Plane), cubeMap.FrontPlane.GetType(), "PlaneType");
        }
    }
}
