using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Lights
{
    public class Light : ILight
    {
        public Vector3 LightPosition { get; set; }
        public Vector3 LightDirection { get; set; }
        public LightParameter LightType { get; set; }
        public Color4 AmbientColor { get; set; }
        public Color4 DiffuseColor { get; set; }
        public Color4 SpecularColor { get; set; }
        public LightName Name { get; set; }

        public Light(Vector3 lightPosition,  Color4 ambientlightColor, Color4 diffuseLightColor, Color4 specularLightColor, LightName name)
        {
            LightPosition  = lightPosition;
            AmbientColor   = ambientlightColor;
            DiffuseColor   = diffuseLightColor;
            SpecularColor  = specularLightColor;
            Name = name;
        }

        public void Load()
        {
            GL.Light(Name, LightParameter.Position, ToFloat3(LightPosition));
            GL.Light(Name, LightParameter.Ambient, AmbientColor);
            GL.Light(Name, LightParameter.Diffuse, DiffuseColor);
            GL.Light(Name, LightParameter.Specular, SpecularColor);
            GL.Enable(EnableCap.Light0);
        }

        public void Start()
        {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
        }

        public void Stop()
        {
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Light0);
        }

        private float[] ToFloat3(Vector3 vector3)
        {
            return new float[] { vector3.X, vector3.Y, vector3.Z };
        }
    }
}
