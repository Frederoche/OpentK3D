using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.IO;

namespace VertexBuffers
{
    public class ShaderProgram : IShaderProgram
    {
        protected int MyVertexShaderHandle;
        protected int MyFragmentShaderHandle;
        protected int MyGeometryShaderHandle;

        public int MyProgramHandler { get; set; }

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
            Utils.Utils.Log(vertexInfo);
            Console.WriteLine(vertexInfo);
            GL.GetShaderInfoLog(MyFragmentShaderHandle, out fragmentInfo);
            Utils.Utils.Log(fragmentInfo);
            Console.WriteLine(fragmentInfo);
            return MyProgramHandler; 
        }
    }
}
