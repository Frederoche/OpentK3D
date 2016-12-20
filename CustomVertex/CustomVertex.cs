using OpenTK;
using System.Runtime.InteropServices;

namespace CustomVertex
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vbo
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Binormal;
        public Vector3 Tangent;
        public Vector2 TexCoord;
        public Vector2 TexCoord2;
            
        public static readonly int stride = Marshal.SizeOf(default(Vbo));
    }

    
}
