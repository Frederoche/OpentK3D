using Camera;
using CustomVertex;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities;
using VertexBuffers;

namespace Frustum
{
    public class Plane
    {
        public Vector3 Normal;
        public float d;

        public void MakePlane(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 vec1 = p1 - p2;
            Vector3 vec2 = p3 - p2;

            Vector3.Cross(ref vec1, ref vec2, out Normal);
            Normal.NormalizeFast();

            Vector3 tempPoint = p2;
            d = -Vector3.Dot(Normal, tempPoint);
        }

        public float Distance(Vector3 point)
        {
            return d + Vector3.Dot(Normal, point);
        }
    }


    public class Frustum : VertexBuffer, IFrustum
    {
        public Plane Top { get; set; }
        public Plane Bottom { get; set; }
        public Plane Far { get; set; }
        public Plane Near { get; set; }
        public Plane Left { get; set; }
        public Plane Right { get; set; }

        private Vector3 _fc, _nc, _ftl, _ftr, _fbl, _fbr, _ntl, _ntr, _nbl, _nbr;
        private float _farDist, _nearDist, _aspectRatio, _fov;
        private float HFar, HNear, WFar, WNear;

        private ICamera _camera;

        private Plane[] _planes;

        public enum PointLocation { Outside = 0, Inside = 1, Intersect = 2 };

        public Frustum(float farDist, float nearDist, float aspectRatio, float fov)
        {
            _fov = fov;
            _aspectRatio = aspectRatio;
            _nearDist = nearDist;
            _farDist = farDist;

            Top = new Plane();
            Bottom = new Plane();
            Far = new Plane();
            Near = new Plane();
            Left = new Plane();
            Right = new Plane();

            Indices = new List<int>();
            Vertices = new VBO[4];
            _planes = new Plane[6];
        }

        public void Load()
        {
            GenerateVertices();
            GenerateIndices();

            CreateShaderProgram(Utils.Utils.GetStreamedResource<IFrustum>("Frustum.Shader.FrustumVertexShader.glsl"),
                                Utils.Utils.GetStreamedResource<IFrustum>("Frustum.Shader.FrustumFragmentShader.glsl"));

            CreateBuffer(BufferUsageHint.StreamDraw);

        }

        private void GenerateVertices()
        {
            Vertices[0].Position = new Vector3(_nbl);
            Vertices[1].Position = new Vector3(_ntl);
            Vertices[2].Position = new Vector3(_ntr);
            Vertices[3].Position = new Vector3(_nbr);

            Vertices[0].Normal = new Vector3(0, 1, 0);
            Vertices[1].Normal = new Vector3(0, 1, 0);
            Vertices[2].Normal = new Vector3(0, 1, 0);
            Vertices[3].Normal = new Vector3(0, 1, 0);
        }

        private void GenerateIndices()
        {
            Indices.Clear();

            Indices.Add(0);
            Indices.Add(1);
            Indices.Add(2);
            Indices.Add(2);
            Indices.Add(3);
            Indices.Add(0);

        }

        public void ExctractPlanes(ICamera camera)
        {
            
            _camera = camera;
            

            HFar = (float) (2 * Math.Tan(_fov * 0.5) * _farDist);
            WFar = _aspectRatio * HFar;

            HNear = (float) (2 * Math.Tan(_fov * 0.5) * _nearDist);
            WNear = _aspectRatio * HNear;

            Vector3 right = Vector3.Cross(camera.UpVector, Vector3.NormalizeFast(camera.CameraLookAt));

            _fc = camera.CameraPosition + Vector3.NormalizeFast(camera.CameraLookAt) * _farDist;
            _nc = camera.CameraPosition + Vector3.NormalizeFast(camera.CameraLookAt) * _nearDist;

            _ftl = _fc + Vector3.NormalizeFast(camera.UpVector)*HFar/2  - right * WFar / 2;
            _ftr = _fc + Vector3.NormalizeFast(camera.UpVector)*HFar/2  + right * WFar / 2;
            _fbl = _fc - Vector3.NormalizeFast(camera.UpVector)*HFar/2  - right * WFar / 2;
            _fbr = _fc - Vector3.NormalizeFast(camera.UpVector)*HFar/2  + right * WFar / 2;

            _ntl = _nc + Vector3.NormalizeFast(camera.UpVector)*HNear/2 - right * WNear / 2;
            _ntr = _nc + Vector3.NormalizeFast(camera.UpVector)*HNear/2 + right * WNear / 2;
            _nbl = _nc - Vector3.NormalizeFast(camera.UpVector)*HNear/2 - right * WNear / 2;
            _nbr = _nc - Vector3.NormalizeFast(camera.UpVector)*HNear/2 + right * WNear / 2;

            GenerateVertices();
            UpdateBuffer(Vertices);
            
            Far.MakePlane(_ftr, _fbl, _ftl);
            Near.MakePlane(_ntl, _ntr, _nbr);
            Top.MakePlane(_ntr, _ntl, _ftl);
            Bottom.MakePlane(_nbl, _nbr, _fbr);
            Right.MakePlane(_nbr, _ntr, _fbr);
            Left.MakePlane(_ntl, _nbl, _fbl);

            _planes[0] = Far;
            _planes[1] = Near;
            _planes[2] = Top;
            _planes[3] = Bottom;
            _planes[4] = Right;
            _planes[5] = Left;
        }

        public void ShowFrustum(Matrix4 projectionMatrix, Matrix4 viewMatrix)
        {
            GL.PointSize(10.0f);
            Vector3 p = new Vector3(0, 0, 0);

            GL.Begin(BeginMode.Points);
            {
                GL.Color3(Color.Blue);
                
                GL.Vertex3(p);
            }
            GL.End();

            GL.Disable(EnableCap.CullFace);
                Render(BeginMode.Triangles, Matrix4.Identity, projectionMatrix, viewMatrix);
            GL.Enable(EnableCap.CullFace);
        }

        public int PointInFrustum(Vector3 point)
        {
            PointLocation result = PointLocation.Inside;
            for(int i=0; i< 6;i++)
            {
                if (_planes[i].Distance(point) < 0)
                    return (int)PointLocation.Outside;
            }

            return (int)result;
        }

        public int SquareInFrustum(params Vector3[] points)
        {
            int isIn = 0;
            int isOut = 0;

            for(int i=0; i<6; i++)
            {
                foreach(var point in points)
                {
                    if (_planes[i].Distance(point) < 0)
                        isOut++;
                    else
                        isIn++;
                }
            }

            if (isIn == 0)
                return (int) PointLocation.Outside;
            if (isOut > 0 && isIn > 0)
                return (int) PointLocation.Intersect;
            else
                return (int) PointLocation.Inside;
        }
    }
}
