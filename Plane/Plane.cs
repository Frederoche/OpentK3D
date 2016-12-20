using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using CustomVertex;
using System.Drawing;
using Textures;
using Utilities;
using VertexBuffers;
using OpenTK;


namespace PrimitiveShapes
{
    public class Plane : VertexBuffer,  IPlane
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public Vector3[] Points { get; private set; }
        public bool IsRender { get; set; }

        public Plane(bool invertAllFaces, Vbo p1, Vbo p2, Vbo p3, Vbo p4)
        {
            Vertices = new Vbo[4];

            if (invertAllFaces)
            {
                Vertices[0] = p2;
                Vertices[1] = p1;
                Vertices[2] = p4;
                Vertices[3] = p3;
            }
            else
            {
                Vertices[0] = p1;
                Vertices[1] = p2;
                Vertices[2] = p3;
                Vertices[3] = p4;
            }

            Points = new Vector3[4];
            Points[0] = p1.Position;
            Points[1] = p2.Position;
            Points[2] = p3.Position;
            Points[3] = p4.Position;

            Indices = new List<int>();
            Indices.AddRange(new int[] { 0, 1, 2, 2, 3, 0 });

            Height = p1.Position.Y - p2.Position.Y;
            Width =  p3.Position.X - p2.Position.X;
            Depth =  p2.Position.Z - p1.Position.Z;
        }


        public Plane(bool invertAllFaces, Vbo p1, Vbo p2, Vbo p3, Vbo p4, Image textureImage, TextureWrapMode textureWrapMode = TextureWrapMode.ClampToEdge) : this(invertAllFaces, p1, p2, p3, p4)
        {
            if (textureImage != null)
                Texture1 = TextureFactory.Create(textureImage, textureWrapMode);
            else
                Texture1 = TextureFactory.Create(TextureWrapMode.ClampToEdge, 50, 50);
        }

        public void Load(BufferUsageHint bufferusageHint = BufferUsageHint.StaticDraw)
        {
            CreateShaderProgram(Utils.Utils.GetStreamedResource<Plane>("PrimitiveShapes.Shaders.PlaneVertexShader.glsl"),
                                Utils.Utils.GetStreamedResource<Plane>("PrimitiveShapes.Shaders.PlaneFragmentShader.glsl"));
            CreateBuffer(bufferusageHint);
        }
    }
}
