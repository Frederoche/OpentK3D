using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Textures;
using CustomVertex;
using System.Runtime.InteropServices;
using Utilities;
using Lights;



namespace VertexBuffers
{
    public class VertexBuffer : ShaderProgram
    {
        public VBO[] Vertices {get;set;}
        public List<int> Indices { get; set; }

        public ITexture Texture1 { get; set; }
        public ITexture Texture2 { get; set; }
        public ITexture Texture3 { get; set; }
        public ITexture Texture4 { get; set; }
        public ITexture Texture5 { get; set; }


        private int MyVertexHandle = 0;
        private int MyIndiceHandle = 0;
        private int MyTextureCoordHandle = 0;
        private int MyTextureCoordHandle2 = 0;
        private int MyNormalHandle = 0;
        private int MyBinormalHandle = 0;
        private int MyTangentHandle = 0;
        
        public int MyVertexArrayHandler = 0;

        private int[] indice;

        public VertexBuffer(){}

        public VertexBuffer(VBO[] vertices, List<int> indices,  params ITexture[] textures)
        {
            Vertices = vertices;
            Indices = indices;
            Texture1 = textures[0];
            Texture2 = textures[1];
            Texture3 = textures[2];
            Texture4 = textures[3];
            Texture5 = textures[4];

        }
       
        public virtual void CreateBuffer(BufferUsageHint bufferUsageHint)
        {
            indice = Indices.ToArray();

            GL.GetUniformLocation(MyProgramHandler, "viewMatrix");
            GL.GetUniformLocation(MyProgramHandler, "projMatrix");
            GL.GetUniformLocation(MyProgramHandler, "worldMatrix");
            GL.GetUniformLocation(MyProgramHandler, "cameraPosition");

            GL.GetUniformLocation(MyProgramHandler, "lightPosition");
            GL.GetUniformLocation(MyProgramHandler, "lightAmbientColor");
            GL.GetUniformLocation(MyProgramHandler, "lightDiffuseColor");
            GL.GetUniformLocation(MyProgramHandler, "lightSpecularColor");

            Utils.LoadTexture(Texture1, TextureUnit.Texture1, (int)TextureUnit.Texture0 - (int)TextureUnit.Texture0, "textureSample1", MyProgramHandler);
            Utils.LoadTexture(Texture2, TextureUnit.Texture2, (int)TextureUnit.Texture1 - (int)TextureUnit.Texture0, "textureSample2", MyProgramHandler);
            Utils.LoadTexture(Texture3, TextureUnit.Texture3, (int)TextureUnit.Texture2 - (int)TextureUnit.Texture0, "textureSample3", MyProgramHandler);
            Utils.LoadTexture(Texture4, TextureUnit.Texture4, (int)TextureUnit.Texture3 - (int)TextureUnit.Texture0, "textureSample4", MyProgramHandler);
            Utils.LoadTexture(Texture5, TextureUnit.Texture5, (int)TextureUnit.Texture4 - (int)TextureUnit.Texture0, "textureSample5", MyProgramHandler);
         
            GL.GenVertexArrays(1, out MyVertexArrayHandler);
            GL.BindVertexArray(MyVertexArrayHandler);

            GenerateBuffer<Vector3>(MyVertexHandle, Vertices.Select(p => p.Position).ToArray(), "inputPosition", 0, false, 3, bufferUsageHint);
            GenerateBuffer<Vector3>(MyNormalHandle, Vertices.Select(p => p.Normal).ToArray(), "inputNormal", 1, true, 3, bufferUsageHint);
            GenerateBuffer<Vector3>(MyBinormalHandle, Vertices.Select(p => p.Binormal).ToArray(), "inputBinormal", 2, true, 3, bufferUsageHint);
            GenerateBuffer<Vector3>(MyTangentHandle, Vertices.Select(p => p.Tangent).ToArray(), "inputTangent", 3, true, 3, bufferUsageHint);
            GenerateBuffer<Vector2>(MyTextureCoordHandle, Vertices.Select(p => p.TexCoord).ToArray(), "inputTexCoord", 4, false, 2, bufferUsageHint);
            GenerateBuffer<Vector2>(MyTextureCoordHandle2, Vertices.Select(p => p.TexCoord2).ToArray(), "inputTexCoord2", 5, false, 2, bufferUsageHint);

            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.GenBuffers(1, out MyIndiceHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, MyIndiceHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indice.Length * sizeof(int)), indice, bufferUsageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public virtual void UpdateBuffer(VBO[] vertices)
        {
            GL.BindVertexArray(MyVertexArrayHandler);
            GL.UseProgram(MyProgramHandler);
            GenerateBuffer<Vector3>(MyVertexHandle, vertices.Select(p => p.Position).ToArray(), "inputPosition", 0, false, 3, BufferUsageHint.StreamDraw);
            GL.UseProgram(0);
            GL.BindVertexArray(0);
        }

        public virtual void Render(BeginMode mode , Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, float[] clipPlaneReflection = null, float[] clipPlaneRefraction = null, ILight light = null)
        {
           
                GL.BindVertexArray(MyVertexArrayHandler);
                GL.UseProgram(MyProgramHandler);

                Utils.BindTexture(Texture1, TextureUnit.Texture1);
                Utils.BindTexture(Texture2, TextureUnit.Texture2);
                Utils.BindTexture(Texture3, TextureUnit.Texture3);
                Utils.BindTexture(Texture4, TextureUnit.Texture4);
                Utils.BindTexture(Texture4, TextureUnit.Texture5);

                if (light != null)
                {
                    GL.Uniform3(GL.GetUniformLocation(MyProgramHandler, "lightPosition"), light.LightPosition);
                    GL.Uniform3(GL.GetUniformLocation(MyProgramHandler, "lightAmbientColor"),light.AmbientColor.ToRgb());
                    GL.Uniform3(GL.GetUniformLocation(MyProgramHandler, "lightDiffuseColor"), light.DiffuseColor.ToRgb());
                }

                if (clipPlaneReflection != null)
                    GL.Uniform4(GL.GetUniformLocation(MyProgramHandler, "clipPlaneReflection"), new Vector4(clipPlaneReflection[0], clipPlaneReflection[1], clipPlaneReflection[2], clipPlaneReflection[3]));

                if (clipPlaneRefraction != null)
                    GL.Uniform4(GL.GetUniformLocation(MyProgramHandler, "clipPlaneRefraction"), new Vector4(clipPlaneRefraction[0], clipPlaneRefraction[1], clipPlaneRefraction[2], clipPlaneRefraction[3]));
                
               
                GL.UniformMatrix4(GL.GetUniformLocation(MyProgramHandler, "worldMatrix"), false, ref worldMatrix);
                GL.UniformMatrix4(GL.GetUniformLocation(MyProgramHandler, "projMatrix"), false, ref projectionMatrix);
                GL.UniformMatrix4(GL.GetUniformLocation(MyProgramHandler, "viewMatrix"), false, ref viewMatrix);

                GL.DrawRangeElements(mode, 0, indice.Length, indice.Length, DrawElementsType.UnsignedInt, indice);
                
                GL.BindTexture(TextureTarget.Texture2D, 0);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.UseProgram(0);
                GL.BindVertexArray(0);
                
        }

        private void GenerateBuffer<TVector>(int Handle, TVector[] inputVector, string shaderParameterName, int shaderIndex, bool normalized, int typeElementNumber, BufferUsageHint bufferUsageHint) where TVector : struct
        {
            if (inputVector == null)
                return;

            GL.GenBuffers(1, out  Handle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(inputVector.Length * Marshal.SizeOf(typeof(TVector))), inputVector, bufferUsageHint);

            GL.EnableVertexAttribArray(shaderIndex);
            GL.BindAttribLocation(MyProgramHandler, shaderIndex, shaderParameterName);
            GL.VertexAttribPointer(shaderIndex, typeElementNumber, VertexAttribPointerType.Float, normalized, Marshal.SizeOf(typeof(TVector)), 0);
        }
    }
}
