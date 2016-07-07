using OpenTK;
using System.Drawing;

namespace EnvironmentMap
{
    public class CubeMapFactory
    {
        public static ICubeMap Create(float width, bool invertAllFaces, Vector3 centeredAt, Image frontTextureImage)
        {
            return new CubeMap(width, invertAllFaces, centeredAt, frontTextureImage);
        }

        public static ICubeMap Create(float width, bool invertAllFaces, Vector3 centeredAt, Image frontTextureImage, Image backTextureImage, Image bottomTextureImage, Image topTextureImage, Image leftTextureImage, Image rigthTextureImage)
        {
            return new CubeMap(width, invertAllFaces, centeredAt, frontTextureImage, backTextureImage, bottomTextureImage, topTextureImage, leftTextureImage, rigthTextureImage);
        }
    }
}
