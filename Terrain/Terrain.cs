using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Textures;
using CustomVertex;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Lights;
using Utilities;
using VertexBuffers;


namespace Landscape
{
    public class Terrain : VertexBuffer, ITerrain
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public string TexturePath { get; set; }
        public Vector3 Center { get; set; }
        
        private Bitmap heightMap;
            
        private float[,] heights;

        public Terrain(Image heightmapImage, Image blendMap, params Image[] image) 
        {
            Center = new Vector3(Width / 2, 0, Height / 2);

            Texture1 = TextureFactory.Create(image[0], TextureWrapMode.Repeat);
            Texture2 = TextureFactory.Create(image[1], TextureWrapMode.Repeat);
            Texture3 = TextureFactory.Create(image[2], TextureWrapMode.Repeat);
            Texture4 = TextureFactory.Create(blendMap, TextureWrapMode.Repeat);

            heightMap = new Bitmap(heightmapImage);
            Width = heightMap.Width;
            Height = heightMap.Height;
            heights = new float[Width, Height];
            Vertices = new VBO[Width*Height];
            Indices = new List<int>();
        }

        public void SmoothFilter()
        {
            if (Vertices.Count() == 0)
                return;

            for (int i = 1; i < Width-1; i++)
                for (int j = 1; j < Height-1; j++)
                {
                    Vertices[i + j * Width].Position.Y = (float) (Vertices[i - 1 + (j - 1) * Width].Position.Y +
                                                                 Vertices[i + (j + 1) * Width].Position.Y +
                                                                 Vertices[i + 1 + (j + 1) * Width].Position.Y +
                                                                 Vertices[i - 1 + j * Width].Position.Y +
                                                                 Vertices[i + j * Width].Position.Y +
                                                                 Vertices[i + 1 + j * Width].Position.Y +
                                                                 Vertices[i - 1 + (j - 1) * Width].Position.Y +
                                                                 Vertices[i + (j - 1) * Width].Position.Y +
                                                                 Vertices[i + 1 + (j + 1) * Width].Position.Y) / 9.0f;
                };
        }

        public void Load(bool isNoise = false, bool isVoronoi = false)
        {
            if (!isNoise)
                LoadFromImage();
            else
                LoadFromRandomNoise(1024, isVoronoi);

            GenerateVertices();

            GenerateIndices();

            ComputeNormals();
            InitTerrainShader();
        }

        private void GenerateVertices()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Vertices[i + j * Width] = new VBO()
                    {
                        Position = new Vector3((float)-Width  +  i, (float) heights[i, j], (float)-Height + j),
                        TexCoord = new Vector2((float)i / 10, (float)j / 10),
                        TexCoord2 = new Vector2((float)i / Width, (float)j / Height)
                    };
        }

        private void GenerateIndices()
        {
            Indices.Clear();

            for (int i = 0; i < (Width - 1); i++)
            {
                for (int j = 0; j < (Height - 1); j++)
                {
                    Indices.Add(i * Width + j);
                    Indices.Add(i * Width + j + 1);
                    Indices.Add((i + 1) * Width + j);
                    Indices.Add((i + 1) * Width + j + 1);
                    Indices.Add((i + 1) * Width + j);
                    Indices.Add(i * Width + j + 1);
                }
            }
        }

        private void LoadFromImage()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    heights[i, j] = (float)(heightMap.GetPixel(i, j).R + heightMap.GetPixel(i, j).G + heightMap.GetPixel(i, j).B) / 3.0f;
            
        }

       private void LoadFromRandomNoise(int Size, bool isVoronoi)
        {
            DiamondSquare diamondSquare = new DiamondSquare(Size);
            diamondSquare.Execute();

            Voronoi vornoi = new Voronoi(Size, 20);

            if (isVoronoi)
                vornoi.Execute();
            

            Width = Size;
            Height = Size;
            heights = new float[Size,Size];
            Vertices = new VBO[Width * Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (isVoronoi)
                        heights[i, j] = (int)(((diamondSquare.Array[i, j] + 1) * 255) + vornoi.Array[i, j]) / 2;
                    else
                        heights[i, j] = (int)((diamondSquare.Array[i, j] + 1) * 255);
                }
            }
        }

        private void ComputeNormals()
        {
            for (int i = 0; i < Width - 1; i++)
                for (int j = 0; j < Height - 1; j++)
                {
                    Vertices[j + Width * i].Tangent  = Vector3.NormalizeFast(Vertices[(i + 1) * Width + j].Position - Vertices[i * Width + j].Position);
                    Vertices[j + Width * i].Normal   = Vector3.NormalizeFast(Vector3.Cross(Vertices[(i + 1) * Width + j].Position - Vertices[i * Width + j].Position, Vertices[i * Width + j + 1].Position - Vertices[i * Width + j].Position));
                    Vertices[j + Width * i].Binormal = Vector3.NormalizeFast(Vector3.Cross(Vertices[j + Width * i].Normal, Vertices[j + Width * i].Tangent));
                }

            for (int i = 1; i < Width - 1; i++)
            {
                for (int j = 1; j < Height - 1; j++)
                {
                    Vertices[i + j * Width].Normal = (Vertices[i - 1 + (j - 1) * Width].Normal +
                                                                Vertices[i + (j + 1) * Width].Normal +
                                                                Vertices[i + 1 + (j + 1) * Width].Normal +
                                                                Vertices[i - 1 + j * Width].Normal +
                                                                Vertices[i + j * Width].Normal +
                                                                Vertices[i + 1 + j * Width].Normal +
                                                                Vertices[i - 1 + (j - 1) * Width].Normal +
                                                                Vertices[i + (j - 1) * Width].Normal +
                                                                Vertices[i + 1 + (j + 1) * Width].Normal) / 9.0f;
                }
            }
        }

        private void InitTerrainShader()
        {
            
            CreateShaderProgram(Utils.Utils.GetStreamedResource<ITerrain>("Landscape.Shaders.TerrainVertexShader.glsl"),
                                Utils.Utils.GetStreamedResource<ITerrain>("Landscape.Shaders.TerrainFragmentShader.glsl"));
            
            CreateBuffer(BufferUsageHint.StaticDraw);
        }

        public void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 viewMatrix, Matrix4 projectionMatrix, ILight light,  bool isRendered)
        {
            if (isRendered == false)
                return;

            Render(mode, worldMatrix, projectionMatrix, viewMatrix, light: light);
        }
    }
}
