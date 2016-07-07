using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.IO;
using Utilities;

namespace VertexBuffers
{
    public class ShaderProgram : IShaderProgram
    {
        protected int MyVertexShaderHandle = 0;
        protected int MyFragmentShaderHandle = 0;
        protected int MyGeometryShaderHandle = 0;

        public int MyProgramHandler { get; set; }

        private string VertexShaderNamespace { get; set; }
        private string FragmantShaderNamespace { get; set; }
        private string GeometryShaderNamespace { get; set; }

        public int CreateShaderProgram(Stream vertexShaderStream, Stream FragmentShaderStream, Stream GeometryShaderStream = null)
        {
            MyProgramHandler = 0;
            
            MyVertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            MyFragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            MyGeometryShaderHandle = GL.CreateShader(ShaderType.GeometryShaderExt);
            
            using (StreamReader sr = new StreamReader(vertexShaderStream))
            {
                GL.ShaderSource(MyVertexShaderHandle, sr.ReadToEnd());
                sr.Close();
            }

            using (StreamReader sr = new StreamReader(FragmentShaderStream))
            {
                GL.ShaderSource(MyFragmentShaderHandle, sr.ReadToEnd());
                sr.Close();
            }

            if(GeometryShaderStream!=null)
            {
                using(StreamReader sr = new StreamReader(GeometryShaderStream))
                {
                    GL.ShaderSource(MyFragmentShaderHandle, sr.ReadToEnd());
                }
            }

            GL.CompileShader(MyVertexShaderHandle);
            GL.CompileShader(MyFragmentShaderHandle);

            if (GeometryShaderStream != null)
                GL.CompileShader(MyGeometryShaderHandle);

            Debug.WriteLine(GL.GetShaderInfoLog(MyVertexShaderHandle));
            Debug.WriteLine(GL.GetShaderInfoLog(MyFragmentShaderHandle));

            if (MyGeometryShaderHandle != 0)
                Debug.WriteLine(GL.GetShaderInfoLog(MyGeometryShaderHandle));

            MyProgramHandler = GL.CreateProgram();

            GL.AttachShader(MyProgramHandler, MyVertexShaderHandle);
            GL.AttachShader(MyProgramHandler, MyFragmentShaderHandle);

            if (GeometryShaderStream != null)
                GL.AttachShader(MyProgramHandler, MyGeometryShaderHandle);

            GL.LinkProgram(MyProgramHandler);
            GL.UseProgram(MyProgramHandler);

            string vertexInfo, fragmentInfo;

            GL.GetShaderInfoLog(MyVertexShaderHandle, out vertexInfo);
            Utils.Log(vertexInfo);
            Console.WriteLine(vertexInfo);
            GL.GetShaderInfoLog(MyFragmentShaderHandle, out fragmentInfo);
            Utils.Log(fragmentInfo);
            Console.WriteLine(fragmentInfo);
            return MyProgramHandler; 
        }
    }
}
