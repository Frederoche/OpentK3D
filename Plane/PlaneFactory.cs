using CustomVertex;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;

namespace PrimitiveShapes
{
    public class PlaneFactory
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public List<int> Indices { get; set; }

        public static IPlane Create(bool invertAllFaces, Vbo p1, Vbo p2, Vbo p3, Vbo p4, Image textureImage, TextureWrapMode textureWrapMode)
        {
            return new Plane(invertAllFaces, p1, p2, p3, p4, textureImage, textureWrapMode);
        }
    }
}
