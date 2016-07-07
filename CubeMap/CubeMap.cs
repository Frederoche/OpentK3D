using System.Collections.Generic;
using PrimitiveShapes;
using OpenTK;
using System.Drawing;
using CustomVertex;
using OpenTK.Graphics.OpenGL;
using Frustum;


namespace EnvironmentMap
{
    public class CubeMap : ICubeMap
    {
        public float  Width  { get; set; }

        public IPlane BackPlane {get;set; }
        public IPlane FrontPlane { get; set; }
        public IPlane TopPlane { get; set; }
        public IPlane LeftPlane { get; set; }
        public IPlane RightPlane { get; set; }
        public IPlane BottomPlane { get; set; }

        public Vector3 CubeCenterPosition { get; set; }
        public Vector3 Center { get; set; }

        public List<IPlane> Planes { get; set; }
    


        public int[] MyProgramHandler { get; set; }

        public CubeMap(float width, bool invertAllFaces, Vector3 centeredAt, Image frontTextureImage)
            : this(width, invertAllFaces, centeredAt, frontTextureImage, frontTextureImage, frontTextureImage, frontTextureImage, frontTextureImage, frontTextureImage){}
    

        public CubeMap(float width, bool invertAllFaces, Vector3 centeredAt, Image frontTextureImage, Image backTextureImage, Image bottomTextureImage, Image topTextureImage, Image leftTextureImage, Image rightTextureImage)
        {

            Planes = new List<IPlane>();
            Width = width;
            Center = new Vector3(0, 0, 0);

            MyProgramHandler = new int[5];
            CubeCenterPosition = new Vector3(Width / 2 + centeredAt.X, Width / 2 + centeredAt.Y, Width / 2 +centeredAt.Z);
            IPlane backPlane = PlaneFactory.Create(invertAllFaces,
                                                    new VBO()
                                                    {
                                                        Position = new Vector3(-Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, -Width / 2.0f +centeredAt.Z),
                                                        Normal = new Vector3(0, 0, 1),
                                                        TexCoord = new Vector2(1, 1)
                                                    },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(-Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, 0, 1),
                                                       TexCoord = new Vector2(1, 0)
                                                   },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, 0, 1),
                                                       TexCoord = new Vector2(0, 0)
                                                   },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, 0, 1),
                                                       TexCoord = new Vector2(0, 1)
                                                   }, backTextureImage, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);

            IPlane frontPlane = PlaneFactory.Create(invertAllFaces,
                                                    new VBO()
                                                    {
                                                        Position = new Vector3(Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                        Normal = new Vector3(0, 0, 1),
                                                        TexCoord = new Vector2(1, 1)
                                                    },

                                                     new VBO()
                                                     {
                                                         Position = new Vector3(Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                         Normal = new Vector3(0, 0, 1),
                                                         TexCoord = new Vector2(1, 0)
                                                     },

                                                     new VBO()
                                                     {
                                                         Position = new Vector3(-Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                         Normal = new Vector3(0, 0, 1),
                                                         TexCoord = new Vector2(0, 0)
                                                     },

                                                     new VBO()
                                                     {
                                                         Position = new Vector3(-Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                         Normal = new Vector3(0, 0, 1),
                                                         TexCoord = new Vector2(0, 1)
                                                     }, frontTextureImage, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);
            IPlane topPlane = PlaneFactory.Create(invertAllFaces,
                                                    new VBO()
                                                    {
                                                        Position = new Vector3(-Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                        Normal = new Vector3(0, -1, 0),
                                                        TexCoord = new Vector2(0, 0)
                                                    },
                                                   new VBO()
                                                   {
                                                       Position = new Vector3(-Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, -1, 0),
                                                       TexCoord = new Vector2(0, 1)
                                                   },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, -1, 0),
                                                       TexCoord = new Vector2(1, 1)
                                                   },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, -1, 0),
                                                       TexCoord = new Vector2(1, 0)
                                                   }, topTextureImage, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);

            IPlane leftPlane = PlaneFactory.Create(invertAllFaces,new VBO()
                                                    {
                                                        Position = new Vector3(-Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                        Normal = new Vector3(-1, 0, 0),
                                                        TexCoord = new Vector2(1, 1)
                                                    },

                                                  new VBO()
                                                  {
                                                      Position = new Vector3(-Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                      Normal = new Vector3(1, 0, 0),
                                                      TexCoord = new Vector2(1, 0)
                                                  },

                                                  new VBO()
                                                  {
                                                      Position = new Vector3(-Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                      Normal = new Vector3(1, 0, 0),
                                                      TexCoord = new Vector2(0, 0)
                                                  },

                                                  new VBO()
                                                  {
                                                      Position = new Vector3(-Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                      Normal = new Vector3(1, 0, 0),
                                                      TexCoord = new Vector2(0, 1)
                                                  }, leftTextureImage, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);


            IPlane rightPlane = PlaneFactory.Create(invertAllFaces,
                                                new VBO()
                                                {
                                                    Position = new Vector3(Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                    Normal = new Vector3(1, 0, 0),
                                                    TexCoord = new Vector2(1, 1)
                                                },

                                                 new VBO()
                                                 {
                                                     Position = new Vector3(Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                     Normal = new Vector3(1, 0, 0),
                                                     TexCoord = new Vector2(1, 0)
                                                 },

                                                 new VBO()
                                                 {
                                                     Position = new Vector3(Width / 2.0f + centeredAt.X, Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                     Normal = new Vector3(1, 0, 0),
                                                     TexCoord = new Vector2(0, 0)
                                                 },

                                                 new VBO()
                                                 {
                                                     Position = new Vector3(Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                     Normal = new Vector3(1, 0, 0),
                                                     TexCoord = new Vector2(0, 1)
                                                 }, rightTextureImage, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);

            IPlane bottomPlane = PlaneFactory.Create(invertAllFaces,
                                                    new VBO()
                                                    {
                                                        Position = new Vector3(-Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, Width / 2.0f + centeredAt.Z),
                                                        Normal = new Vector3(0, -1, 0),
                                                        TexCoord = new Vector2(0, 0)
                                                    },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(-Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, -1, 0),
                                                       TexCoord = new Vector2(0, 1)
                                                   },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, -Width / 2.0f + centeredAt.Z),
                                                       Normal = new Vector3(0, -1, 0),
                                                       TexCoord = new Vector2(1, 1)
                                                   },

                                                   new VBO()
                                                   {
                                                       Position = new Vector3(Width / 2.0f + centeredAt.X, -Width / 2.0f + centeredAt.Y, Width / 2.0f +centeredAt.Z),
                                                       Normal = new Vector3(0, -1, 0),
                                                       TexCoord = new Vector2(1, 0)
                                                   }, topTextureImage, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);

            TopPlane = topPlane;
            BackPlane = backPlane;
            FrontPlane = frontPlane;
            LeftPlane = leftPlane;
            RightPlane = rightPlane;
            BottomPlane = bottomPlane;
            

            Planes.AddRange(new List<IPlane> { topPlane, backPlane, frontPlane, leftPlane, rightPlane, bottomPlane});
        }

        public void Load()
        {
            Planes.ForEach(p => p.Load());
        }


        public void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, bool isShown, IFrustum frustum, float[] clipPlaneEquation)
        {
            if (isShown)
            {
                Planes.ForEach(p => 
                {
                    if (frustum != null && frustum.SquareInFrustum(p.Points) == 0)
                        p.IsRender = false;
                    else
                        p.IsRender = true;
                });
                
                Planes.ForEach(p => 
                    {
                        if (p.IsRender == true)
                            p.Render(mode, worldMatrix, projectionMatrix, viewMatrix, clipPlaneEquation);
                    });
            }
        }
    }
}
