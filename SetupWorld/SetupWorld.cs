using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace SetupWorld
{
    public class SetupWorld : ISetupWorld
    {
        public Matrix4 ProjectionMatrix { get; set; }
        public Matrix4 ViewMatrix { get; set; }
        public Matrix4 WorldMatrix { get; set; }

        public int ProjectionMatrixLocation { get; set; }
        public int ViewMatrixLocation { get; set; }
        public int WorldMatrixLocation { get; set; }

        public SetupWorld(Matrix4 projectionMatrix, Matrix4 viewMatrix, Matrix4 worldMatrix)
        {
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
            
            ProjectionMatrix = projectionMatrix;
            ViewMatrix = viewMatrix;
            WorldMatrix = worldMatrix = Matrix4.Identity;
        }
    }
}
