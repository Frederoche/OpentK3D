namespace FrameBufferObject
{
    public class FramBufferOBjectFactory
    {
        public static IFrameBufferObject Create(int width, int height)
        {
            return new FrameBufferObject( width, height);
        }
    }
}
