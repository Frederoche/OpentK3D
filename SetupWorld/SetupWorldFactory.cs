using OpenTK;

namespace SetupWorld
{
    public class SetupWorldFactory
    {
        public static ISetupWorld Create(Matrix4 projectionMatrix, Matrix4 viewMatrix, Matrix4 worldMatrix)
        {
            return new SetupWorld(projectionMatrix, viewMatrix, worldMatrix);
        }
    }
}
