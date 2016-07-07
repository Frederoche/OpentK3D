using Camera;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class Camera_Test
    {
        [TestMethod]
        public void Camera_OK()
        {
            //Act
            ICamera camera = CameraFactory.Create(new Vector3(1, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 1));

            //Assert
            Assert.AreEqual(new Vector3(1, 1, 1), camera.CameraPosition, "Camera Position");
            Assert.AreEqual(new Vector3(0, 1, 0), camera.CameraLookAt, "camera LookAt");
            Assert.AreEqual(new Vector3(0, 0, 1), camera.UpVector, "Up Vector");
        }
    }
}
