using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lights
{
    public interface ILight
    {
        Vector3 LightPosition { get; set; }
        LightParameter LightType { get; set; }
        Color4 AmbientColor { get; set; }
        Color4 DiffuseColor { get; set; }
        Color4 SpecularColor { get; set; }
        LightName Name { get; set; }

        void Load();
        void Start();
        void Stop();
    }
}
