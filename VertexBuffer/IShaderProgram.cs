using System.IO;


namespace VertexBuffers
{
    public interface IShaderProgram
    {
        int MyProgramHandler { get; set; }
        int CreateShaderProgram(Stream vertexShaderStream, Stream FragmentShaderStream, Stream GeometryShaderStream = null);
    }
}
