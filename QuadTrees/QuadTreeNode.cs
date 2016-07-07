using Camera;
using CustomVertex;
using Frustum;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexBuffers;

namespace QuadTrees
{
    public class QuadTreeNode
    {
        public int Width, Height;
        public int TriangleCount;
        public QuadTreeNode Parent;
        public QuadTreeNode[] Childs;
        public VBO[] MeshVertices;

        public Vector3[] MeshPosition {get;set;}
        
        public Vector3[] MeshNormal {get;set;}

        public Vector2[] MeshTexture { get; set; }


        public List<int> MeshIndices;

        public Vector3[] NodeCorners { get; set; }
        public bool ToRender { get; set; }
    }
}
