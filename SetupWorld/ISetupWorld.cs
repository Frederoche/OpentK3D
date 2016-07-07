using OpenTK;

namespace SetupWorld
{
    public interface ISetupWorld
    {
        Matrix4 ProjectionMatrix { get; set; }
        Matrix4 ViewMatrix { get; set; }
        Matrix4 WorldMatrix { get; set; }

        int ProjectionMatrixLocation { get; set; }
        int ViewMatrixLocation { get; set; }
        int WorldMatrixLocation { get; set; }
    }
}
