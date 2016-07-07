using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lights
{
    public class LightFactory
    {
        public static ILight Create(Vector3 lightPosition, Color4 ambientlightColor, Color4 diffuseLightColor, Color4 specularLightColor, LightName name)
        {
            return new Light(lightPosition, ambientlightColor, diffuseLightColor, specularLightColor, name);
        }
    }
}
