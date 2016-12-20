using CustomVertex;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Textures;
using System.Diagnostics;
using Lights;
using VertexBuffers;
using FrameBufferObject;
using Camera;
using Utils;

namespace Landscape
{
    public class Water : VertexBuffer, IWater
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Vector3 Center { get; set; }

        public IFrameBufferObject Refraction { get; set; }
        public IFrameBufferObject Reflection { get; set; }

        private Random _randomAngle;
        private Random _randomWaveNumber;

        public ITexture DataTexture;
        public float[,] WaveNumbers; 

        private Stopwatch stopwatch;
        
        private ITexture Foam;

        private int TimeLocation {get;set;}
        
        private int[] indice;
        private double time;

        public float[] ReflectionClipPlane { get; set; }
        public float[] RefractionClipPlane { get; set; }

        public Water(int width, int height)
        {
            Width  = width;
            Height = height;
            Center = new Vector3(Width / 2, Height / 2, 0);
            Vertices = new Vbo[Width * Height];
            Indices  = new List<int>();
            DataTexture = TextureFactory.Create();
            WaveNumbers = new float[24, 4];

            stopwatch = new Stopwatch();
            stopwatch.Start();

            _randomAngle = new Random();
            _randomWaveNumber = new Random();

            ReflectionClipPlane = new float[] { 0, -1, 0, 0 };
            RefractionClipPlane = new float[] { 0,  1, 0, 0 };
        }

        private void LoadWaveNumbersCalm()
        {
            WaveNumbers[0, 0] = 0.5f;
            WaveNumbers[0, 1] = 0.5f;
            WaveNumbers[0, 2] = (float)(_randomAngle.NextDouble()*1.45);
            WaveNumbers[0, 3] = 1.2f;

            WaveNumbers[1, 0] =  -0.6f;
            WaveNumbers[1, 1] =  -0.6f;
            WaveNumbers[1, 2] = (float)(_randomAngle.NextDouble() * 1.45);
            WaveNumbers[1, 3] = -1.1f;


            WaveNumbers[2, 0] =  0.7f;
            WaveNumbers[2, 1] =  0.7f;
            WaveNumbers[2, 2] = (float)(_randomAngle.NextDouble() * 1.45);
            WaveNumbers[2, 3] = -0.6f;


            WaveNumbers[3, 0] =  0.5f;
            WaveNumbers[3, 1] =  0.5f;
            WaveNumbers[3, 2] = (float)(_randomAngle.NextDouble() * 1.45);
            WaveNumbers[3, 3] = -1.5f;
        }

        private void LoadWaveNumbersModerate()
        {
            for (int j = 0; j <4; j++)
            {
               float x = (float)(_randomWaveNumber.NextDouble() * 4.0);
               WaveNumbers[j, 0] = x;
               WaveNumbers[j, 1] = x;
               WaveNumbers[j, 2] = (float)(_randomAngle.NextDouble() * 1.45);
               WaveNumbers[j, 3] = (float)(_randomAngle.NextDouble() * 1.45);
            }
        }


        public void Load()
        {
            
            Foam = TextureFactory.Create(Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.foam.jpg"), TextureWrapMode.Repeat);

            LoadWaveNumbersModerate();
            DataTexture.LoadData(SizedInternalFormat.Rgba16f, WaveNumbers);

            Texture3 = Foam;
            Texture4 = DataTexture;
            Texture5 = TextureFactory.Create(Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.IKT4l.jpg"), TextureWrapMode.Repeat);
            

            for (int i = 0; i < Width; i+=1)
            {
                for (int j = 0; j < Height; j+=1)
                {
                    Vertices[i + j * Width] = new Vbo
                      {
                          Position = new Vector3((float)i, (float)1, (float)j),
                          TexCoord = new Vector2((float)i / Width, (float)j / Height),
                          Normal = new Vector3(0, 1, 0),
                      };
                }
            }

            for (int i = 0; i < Width - 1; i++)
            {
                for (int j = 0; j < Height - 1; j++)
                {
                    Indices.Add(j * Width + i);
                    Indices.Add(j * Width + i + 1);
                    Indices.Add((j + 1) * Width + i);
                    Indices.Add((j + 1) * Width + i + 1);
                    Indices.Add((j + 1) * Width + i);
                    Indices.Add(j * Width + i + 1);
                }
            }
            indice = Indices.ToArray();

            Refraction = FramBufferOBjectFactory.Create(Width, Height);
            Refraction.Load();

            Reflection = FramBufferOBjectFactory.Create(Width, Height);
            Reflection.Load();

            InitWaterShader();
        }

        public void MakeReflectionTextures(BeginMode mode, Matrix4 viewMatrix, Matrix4 invertedViewMatrix, Matrix4 projectionMatrix, float[] clipPlaneEq, Action<BeginMode, Matrix4, Matrix4, Matrix4,float[]> renderFunc)
        {
            Reflection.GenerateProjectiveTexture(mode, viewMatrix, projectionMatrix, clipPlaneEq, renderFunc);
        }

        public void MakeRefractionTextures(BeginMode mode, Matrix4 viewMatrix, Matrix4 invertedViewMatrix, Matrix4 projectionMatrix, float[] clipPlaneEq, Action<BeginMode, Matrix4, Matrix4, Matrix4, float[]> renderFunc)
        {
            Refraction.GenerateProjectiveTexture(mode, invertedViewMatrix, projectionMatrix, clipPlaneEq, renderFunc);
        }

        public void Render(BeginMode mode, Matrix4 worldMatrix, Matrix4 viewMatrix, Matrix4 projectionMatrix, ILight light, ICamera camera,  bool isOceanOn)
       {
           if (isOceanOn == false)
               return;

               if (GL.GetError() == ErrorCode.NoError)
               {
                   GL.Disable(EnableCap.CullFace);
                   {
                       GL.BindVertexArray(MyVertexArrayHandler);
                       GL.UseProgram(MyProgramHandler);
                       time += 0.05;
                       
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "calm"), UIInputData.IsWaterCalm == true ? 1 : 0);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "diffuse"), UIInputData.isDiffuseOn == true ? 1 : 0);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "fresnelParameter"), UIInputData.FresnelParameter);
                       GL.Uniform3(GL.GetUniformLocation(MyProgramHandler, "cameraPosition"), camera.CameraPosition);

                       GL.UniformMatrix4(GL.GetUniformLocation(MyProgramHandler, "worldMatrix"), false, ref worldMatrix);
                       GL.UniformMatrix4(GL.GetUniformLocation(MyProgramHandler, "projMatrix"), false, ref projectionMatrix);
                       GL.UniformMatrix4(GL.GetUniformLocation(MyProgramHandler, "viewMatrix"), false, ref viewMatrix);

                       GL.Uniform3(GL.GetUniformLocation(MyProgramHandler, "lightPosition"), light.LightPosition);

                       GL.Uniform4(GL.GetUniformLocation(MyProgramHandler, "lightAmbientColor"), light.AmbientColor);
                       GL.Uniform4(GL.GetUniformLocation(MyProgramHandler, "lightDiffuseColor"), light.DiffuseColor);
                       GL.Uniform4(GL.GetUniformLocation(MyProgramHandler, "lightSpecularColor"), light.SpecularColor);

                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "time"), (float)time);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "stormParameter"), 5, UIInputData.StormParameter);

                       GL.BindTexture(TextureTarget.Texture2D, Refraction.FrameBufferTexture.MyTextureHandle);
                       GL.ActiveTexture(TextureUnit.Texture1);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "textureSample1"), (int)TextureUnit.Texture0 - (int)TextureUnit.Texture0);

                       GL.BindTexture(TextureTarget.Texture2D, Reflection.FrameBufferTexture.MyTextureHandle);
                       GL.ActiveTexture(TextureUnit.Texture2);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "textureSample2"), (int)TextureUnit.Texture1 - (int)TextureUnit.Texture0);

                       GL.BindTexture(TextureTarget.Texture2D, Texture3.MyTextureHandle);
                       GL.ActiveTexture(TextureUnit.Texture3);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "textureSample3"), (int)TextureUnit.Texture2 - (int)TextureUnit.Texture0);

                       GL.BindTexture(TextureTarget.Texture1D, Texture4.MyTextureHandle);
                       GL.ActiveTexture(TextureUnit.Texture4);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "textureSample4"), (int)TextureUnit.Texture3 - (int)TextureUnit.Texture0);

                       GL.BindTexture(TextureTarget.Texture2D, Texture5.MyTextureHandle);
                       GL.ActiveTexture(TextureUnit.Texture5);
                       GL.Uniform1(GL.GetUniformLocation(MyProgramHandler, "textureSample5"), (int)TextureUnit.Texture4 - (int)TextureUnit.Texture0);

                       GL.DrawRangeElements(BeginMode.Triangles, 0, indice.Length, indice.Length, DrawElementsType.UnsignedInt, indice);

                       GL.BindTexture(TextureTarget.Texture2D, 0);
                       GL.ActiveTexture(TextureUnit.Texture0);

                       GL.UseProgram(0);
                       GL.BindVertexArray(0);
                   }
                   GL.Enable(EnableCap.CullFace);
               }
               else
                   Console.WriteLine(GL.GetError().ToString());
       }


        private void InitWaterShader()
        {
            CreateShaderProgram(Utils.Utils.GetStreamedResource<IWater>("Landscape.Shaders.WaterVertexShader.glsl"),
                                Utils.Utils.GetStreamedResource<IWater>("Landscape.Shaders.WaterFragmentShader.glsl")
                                );

            CreateBuffer(BufferUsageHint.StaticDraw);
        }
    }
}
