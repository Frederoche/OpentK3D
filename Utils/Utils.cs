using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Textures;
using OpenTK.Graphics;

namespace Utilities
{
    public class Factory<T>
    {
        public static T Create(params object[] parameters)
        {
            Type type = typeof(T);
            return (T) Activator.CreateInstance(type, parameters);
        }
    }

    public static class ColorExtention
    {
        public static Vector3 ToRgb(this Color4 color)
        {
            return new Vector3(color.R, color.G, color.B);
        } 
    }

    public class UIInputData
    {
        public static bool IsMapOn { get; set; }
        public static bool IsTerrainOn { get; set; }
        public static bool IsOceanOn { get; set; }
        public static bool IsWoodenChestOn { get; set; }
        public static bool IsCubeMapOn = true;
        public static float FresnelParameter { get; set; }
        public static bool IsWaterCalm { get; set; }
        public static bool isDiffuseOn { get; set; }
        private static float[] _stormParameter = new float[] { 1, 1, 1, 1, 1 };
        public static float[] StormParameter { get { return _stormParameter; } set { _stormParameter = value; } }
    }

    public class Utils
    {
        public static void Log(string lines)
        {
            StreamWriter writer = new StreamWriter("C:\\temp\\Log.txt",true);
            writer.WriteLine(lines);
            writer.Close();
        }

        public static Image GetImageResource<TAssembly>(string embeddedResource)
        {
            Stream stream = Assembly.GetAssembly(typeof(TAssembly)).GetManifestResourceStream(embeddedResource);
            return Image.FromStream(stream);
        }

        public static Stream GetStreamedResource<TAssembly>(string embeddedResource)
        {
            return Assembly.GetAssembly(typeof(TAssembly)).GetManifestResourceStream(embeddedResource);
        }

        public static List<FieldInfo> GetStructMemberTypes<TStruct>() where TStruct : struct
        {
            return typeof(TStruct).GetFields(BindingFlags.Public | BindingFlags.Instance).ToList();
        }

        public static double VectorLenght(Vector2 vec)
        {
            return Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2));
        }

        public static void MakeBirdMap(int textureHandle, bool isActive)
        {
            if (isActive == false)
                return;

            GL.BindTexture(TextureTarget.Texture2D, textureHandle);

            GL.Begin(BeginMode.Quads);
            {
                GL.Color3(Color.White);
                GL.Vertex2(0.5, 0.5);
                GL.TexCoord2(0, 0);

                GL.Vertex2(0.5, 1);
                GL.TexCoord2(0, 1);

                GL.Vertex2(1, 1);
                GL.TexCoord2(1, 1);

                GL.Vertex2(1, 0.5);
                GL.TexCoord2(1, 0);
            }
            GL.End();
        }

        public static void GLRenderProperties(int viewPortWidth, int viewPortHeight)
        {
            GL.Viewport(0, 0, viewPortWidth, viewPortHeight);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            GL.Enable(EnableCap.VertexArray);
            GL.Enable(EnableCap.CullFace);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            

            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            

            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);
        }

        public static void BindTexture(ITexture texture, TextureUnit textureUnit)
        {
            if (texture == null)
                return;

            GL.BindTexture(TextureTarget.Texture2D, texture.MyTextureHandle);
            GL.ActiveTexture(textureUnit);
        }

        public static void LoadTexture(ITexture texture, TextureUnit textureUnit, int v0, string textureName, int programHandler)
        {
            if (texture == null)
                return;

            texture.Load();
            GL.ActiveTexture(textureUnit);
            GL.Uniform1(GL.GetUniformLocation(programHandler, textureName), v0);
        }
    }
}
