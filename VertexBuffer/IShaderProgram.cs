using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexBuffers
{
    public interface IShaderProgram
    {
        int MyProgramHandler { get; set; }
        int CreateShaderProgram(Stream vertexShaderStream, Stream FragmentShaderStream, Stream GeometryShaderStream = null);
    }
}
