using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuadTrees;
using OpenTK;
using System.Collections.Generic;
using CustomVertex;
using System.Linq;
using Landscape;
using Moq;
using Textures;
using Utilities;
using OpenTK.Graphics.OpenGL;

namespace UnitTest
{
    [TestClass]
    public class QuadTree_Test
    {
        [TestMethod]
        public void QuadTree_TestOK()
        {
            //Arrange
            ITexture texture1 = TextureFactory.Create(Utils.GetImageResource<ITerrain>("Landscape.Terrains.testzi.tif"), TextureWrapMode.Repeat);
            ITexture texture2 = TextureFactory.Create(Utils.GetImageResource<ITerrain>("Landscape.Terrains.blendmap.jpg"), TextureWrapMode.Repeat);
            ITexture texture3 = TextureFactory.Create(Utils.GetImageResource<ITerrain>("Landscape.Terrains.Grass.jpg"), TextureWrapMode.Repeat);
            ITexture texture4 = TextureFactory.Create(Utils.GetImageResource<ITerrain>("Landscape.Terrains.Dirt.jpg"), TextureWrapMode.ClampToEdge);

            
            QuadTree<ITerrain> quadTree = new QuadTree<ITerrain>(MakeGrid(), MakeIndices(), 100, 100, 625, "Landscape.Shaders.TerrainVertexShader.glsl",
                "Landscape.Shaders.TerrainFragmentShader.glsl", OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw, texture1, texture2, texture3, texture4);
            
            //Act
            quadTree.MakeTree(quadTree.Root, quadTree.MeshVertices.Count());

            //Assert
            VerifyTree(quadTree);
        }

        private static void VerifyTree(QuadTree<ITerrain> quadTree)
        {
            Assert.AreEqual(21, quadTree.QuadTreeList.Count);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(0).MeshVertices.Count(), 10000);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(0).MeshIndices.Count(), 58806);

            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(1).MeshVertices.Count(), 2500);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(1).MeshIndices.Count(), 14406);

            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(2).MeshVertices.Count(), 2500);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(2).MeshIndices.Count(), 14406);

            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(3).MeshVertices.Count(), 2500);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(3).MeshIndices.Count(), 14406);

            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(4).MeshVertices.Count(), 2500);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(4).MeshIndices.Count(), 14406);

            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(5).MeshVertices.Count(), 625);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(5).MeshIndices.Count(), 3456);

            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(6).MeshVertices.Count(), 625);
            Assert.AreEqual(quadTree.QuadTreeList.ElementAt(6).MeshIndices.Count(), 3456);
        }

        private VBO[] MakeGrid()
        {
            VBO[] grid = new VBO[10000];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    grid[i + j * 100] = new VBO()
                            {
                                Position = new Vector3((float)(i), (float)0, (float)(j)),
                                TexCoord = new Vector2((float)i / 10, (float)j / 10),
                                TexCoord2 = new Vector2((float)i / 100, (float)j / 100)
                            };
                }
            }

            return grid;
        }

        private List<int> MakeIndices()
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < 100 - 1; i++)
            {
                for (int j = 0; j < 100 - 1; j++)
                {
                    indices.Add(i * 100 + j);
                    indices.Add(i * 100 + j + 1);
                    indices.Add((i + 1) * 100 + j);
                    indices.Add((i + 1) * 100 + j + 1);
                    indices.Add((i + 1) * 100 + j);
                    indices.Add(i * 100 + j + 1);
                }
            }
            return indices;
        }
    }
}
