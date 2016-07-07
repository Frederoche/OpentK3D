using System.Collections.Generic;
using System.Linq;
using OpenTK;
using CustomVertex;
using Textures;
using Camera;
using Utilities;
using VertexBuffers;
using Lights;
using OpenTK.Graphics.OpenGL;
using System;
using Frustum;


namespace QuadTrees
{
    /*public class QuadTree<T> : VertexBuffer
    {
        public int TrianglePrecision { get; set; }
        public VBO[] MeshVertices { get; set; }
        public List<int> MeshIndices { get; set; }
        public List<QuadTreeNode> QuadTreeList { get; private set; }
        public List<QuadTreeNode> QuadTreeListToRender { get; private set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public QuadTreeNode Root { get; set; }
        public ITexture[] Textures { get; set; }

        public string VertexShaderNamespace { get; set; }
        public string FragmentShaderNamespace { get; set; }

        public QuadTree(VBO[] meshVertices, List<int> meshIndices, int width, int height, int trianglePrecision, string vertexShaderNamespace, string fragmentShaderNamespace, OpenTK.Graphics.OpenGL.BufferUsageHint bufferUsageHint, params ITexture[] textures):
            base(meshVertices, meshIndices,  textures)
        {
            MeshVertices = meshVertices;
            TrianglePrecision = trianglePrecision;
            Height = height;
            Width  = width;
            MeshIndices = meshIndices;
            Textures = textures;

            VertexShaderNamespace = vertexShaderNamespace;
            FragmentShaderNamespace = fragmentShaderNamespace;

            QuadTreeList = new List<QuadTreeNode>();

            var root = new QuadTreeNode()
            {
                TriangleCount = MeshVertices.Count(),
                Width = width,
                Height = height,
                MeshVertices = meshVertices,
                MeshIndices = meshIndices.ToList(),
                MeshPosition = this.MeshVertices.Select(p => p.Position).ToArray(),
                MeshNormal = this.MeshVertices.Select(p => p.Normal).ToArray(),
                MeshTexture = this.MeshVertices.Select(p => p.TexCoord).ToArray()
            };

            Root = root;
        }

        public void Execute()
        {
            MakeTree(Root, MeshVertices.Count());
            CreateShaderProgram(Utils.GetStreamedResource<T>(VertexShaderNamespace), Utils.GetStreamedResource<T>(FragmentShaderNamespace));
            CreateBuffer(OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw);
        }


        public void Render(BeginMode beginMode, IFrustum frustum, QuadTreeNode quadTreeNode, Matrix4 worldMatrix, Matrix4 viewMatrix, Matrix4 projectionMatrix, ILight light)
        {
            UpdateBuffer(this.Vertices);
            Render(BeginMode.Triangles, worldMatrix, projectionMatrix, viewMatrix, light: light);
        }

        public void MakeTree(QuadTreeNode parentNode, int count)
        {
            if (count > TrianglePrecision)
            {
                QuadTreeNode[] child = new QuadTreeNode[4];

                child[0] = new QuadTreeNode();
                child[0].Height = parentNode.Height / 2;
                child[0].Width = parentNode.Width / 2;
                child[0].Parent = parentNode;
                child[0].MeshVertices = GetVerticesOdd(parentNode.MeshVertices, 0, parentNode.Width / 2, 0, parentNode.Height / 2);
                child[0].MeshIndices  = GetIndices(parentNode);
                child[0].NodeCorners = GetNodeCorners(child[0].MeshVertices, child[0].Width, child[0].Height);
                child[0].MeshPosition = child[0].MeshVertices.Select(p => p.Position).ToArray();
                child[0].MeshNormal = child[0].MeshVertices.Select(p => p.Normal).ToArray();
                child[0].MeshTexture = child[0].MeshVertices.Select(p => p.TexCoord).ToArray();
                child[0].TriangleCount = child[0].MeshVertices.Count(); //1

                child[1] = new QuadTreeNode();
                child[1].Height = parentNode.Height / 2;
                child[1].Width = parentNode.Width / 2;
                child[1].Parent = parentNode;
                child[1].MeshVertices = GetVertices(parentNode.MeshVertices, parentNode.Width / 2, parentNode.Width, 0, parentNode.Height / 2);
                child[1].MeshIndices = GetIndices(parentNode);
                child[1].NodeCorners = GetNodeCorners(child[1].MeshVertices, child[1].Width, child[1].Height);
                child[1].MeshPosition = child[1].MeshVertices.Select(p => p.Position).ToArray();
                child[1].MeshNormal = child[1].MeshVertices.Select(p => p.Normal).ToArray();
                child[1].MeshTexture = child[1].MeshVertices.Select(p => p.TexCoord).ToArray();
                child[1].TriangleCount = child[1].MeshVertices.Count();  //2

                child[2] = new QuadTreeNode();
                child[2].Height = parentNode.Height / 2;
                child[2].Width = parentNode.Width / 2;
                child[2].Parent = parentNode;
                child[2].MeshVertices = GetVerticesOdd2(parentNode.MeshVertices, 0, parentNode.Width / 2, parentNode.Height / 2, parentNode.Height);
                child[2].MeshIndices = GetIndices(parentNode);
                child[2].NodeCorners = GetNodeCorners(child[2].MeshVertices, child[2].Width, child[2].Height);
                child[2].MeshPosition = child[2].MeshVertices.Select(p => p.Position).ToArray();
                child[2].MeshNormal = child[2].MeshVertices.Select(p => p.Normal).ToArray();
                child[2].MeshTexture = child[2].MeshVertices.Select(p => p.TexCoord).ToArray();
                child[2].TriangleCount = child[2].MeshVertices.Count(); //3

                child[3] = new QuadTreeNode();
                child[3].Height = parentNode.Height / 2;
                child[3].Width =  parentNode.Width / 2;
                child[3].Parent = parentNode;
                child[3].MeshVertices = GetVertices(parentNode.MeshVertices, parentNode.Width / 2 , parentNode.Width, parentNode.Height / 2 , parentNode.Height);
                child[3].MeshIndices = GetIndices(parentNode);
                child[3].NodeCorners = GetNodeCorners(child[3].MeshVertices, child[3].Width, child[3].Height);
                child[3].MeshPosition = child[3].MeshVertices.Select(p => p.Position).ToArray();
                child[3].MeshNormal = child[3].MeshVertices.Select(p => p.Normal).ToArray();
                child[3].MeshTexture = child[3].MeshVertices.Select(p => p.TexCoord).ToArray();
                child[3].TriangleCount = child[3].MeshVertices.Count(); //4

                parentNode.Childs = child;

                if (parentNode.Parent == null)
                    QuadTreeList.Add(parentNode); //0

                QuadTreeList.AddRange(child);

                for (int i = 0; i < 4; i++)
                {
                    MakeTree(child[i], child[i].MeshVertices.Count());
                }
            }
        }

        private Vector3[] GetNodeCorners(VBO[] vertices, int width, int height)
        {
            Vector3[] result = new Vector3[4];

            result[0] = vertices[0].Position;
            result[1] = vertices[width - 1].Position;
            result[2] = vertices[(width - 1) * height].Position;
            result[3] = vertices.Last().Position;
            return result;
        }

        private List<int> GetIndices(QuadTreeNode node) 
        {
            var subMeshIndices = new List<int>();

            for (int i = 0; i < node.Width / 2 - 1; i++)
            {
                for (int j = 0; j < node.Height / 2 - 1; j++)
                {
                    subMeshIndices.Add(i * node.Width / 2   + j);
                    subMeshIndices.Add(i * node.Width / 2   + j + 1);
                    subMeshIndices.Add((i + 1) * node.Width / 2 + j);
                    subMeshIndices.Add((i + 1) * node.Width / 2 + j + 1);
                    subMeshIndices.Add((i + 1) * node.Width / 2 + j);
                    subMeshIndices.Add(i * node.Width / 2 + j + 1);
                }
            }
            return subMeshIndices;
        }

        private VBO[] GetVertices(VBO[] meshVertices, int IMin, int IMax, int JMin, int JMax)
        {
            VBO[] subMeshVertices = new VBO[meshVertices.Count()/4];
            int index = 0;

            for (int i = IMin; i < IMax;  i++)
            {
                for (int j = JMin; j < JMax; j++)
                {
                    index = i - IMin + (j - JMin) * (IMax - IMin);
                    subMeshVertices[index] = meshVertices[i + j * IMax];
                }
            }
            return subMeshVertices;
        }


        private VBO[] GetVerticesOdd(VBO[] meshVertices, int IMin, int IMax, int JMin, int JMax)
        {
            VBO[] subMeshVertices = new VBO[meshVertices.Count() / 4];
            int index = 0;

            for (int i = IMin; i < IMax; i++)
            {
                for (int j = JMin; j < JMax; j++)
                {
                    subMeshVertices[index] = meshVertices[j +  JMax * i * 2];
                    index++;
                }
            }
            return subMeshVertices;
        }

        private VBO[] GetVerticesOdd2(VBO[] meshVertices, int IMin, int IMax, int JMin, int JMax)
        {
            VBO[] subMeshVertices = new VBO[meshVertices.Count() / 4];
            int index = 0;

            for (int j = JMin; j < JMax; j++)
            {
                for (int i = IMin; i < IMax; i++)
                {
                    subMeshVertices[index] = meshVertices[i + 2 * j * IMax];
                    index++;
                }
            }
            return subMeshVertices;
        }
    }*/
}
