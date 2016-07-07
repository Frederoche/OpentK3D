using System.Drawing;

namespace Landscape
{
    public class Terrainfactory
    {
        public static ITerrain Create(Image heightmapImage, Image textureImage1, Image textureImage2, Image textureImage3, Image blendMap)
        {
            return new Terrain(heightmapImage, textureImage1, textureImage2, textureImage3, blendMap);
        }
    }
}
