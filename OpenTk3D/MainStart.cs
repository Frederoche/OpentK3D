using System;
using OpenTK;
using Camera;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using Keyboard;
using EnvironmentMap;
using OpenTK.Graphics;
using Landscape;
using Lights;
using FrameBufferObject;
using System.Linq;
using Utilities;
using PrimitiveShapes;
using CustomVertex;
using Frustum;


namespace OpenTk3D
{
    class MainStart
    {
        private static ICamera  Camera;
        
        private static ICubeMap CubeMap;
        private static ICubeMap WoodenChest;

        private static ITerrain Terrain;
        private static ILight   Light;
        private static IFrustum Frustum;

        private static IPlane SeaBed;

        private static IFrameBufferObject BirdTexture;

        private static IWater Water;

        private static Vector3 CameraPosition0 = new Vector3(-378.0f, 75.0f, -8.0f);
        private static Vector3 LookAt0 = new Vector3(0, 0, 0);

        private static float CameraAngle = 0.898f;

        private static Matrix4 ViewMatrix;
        private static Matrix4 BirdViewMatrix;
        private static Matrix4 InvertedViewMatrix;
        private static Matrix4 ProjectionMatrix;

        private const int WINDOW_HEIGHT = 724;
        private const int WINDOW_WIDTH  = 1024;
        private const float FOVY = (float) (Math.PI / 2.0);
        private const float NEAR_PLANE = 1.0f;
        private const float FAR_PLANE  = 5000.0f;

        private const int WATER_WIDTH = 512;
        private const int WATER_HEIGHT = 512;

        [STAThread]
        static void Main(string[] args) 
        {
            var gameWindow = new GameWindow(WINDOW_WIDTH, WINDOW_HEIGHT, new GraphicsMode(32, 24, 0, 8), "Ocean sim (Grestner waves) and terrain", GameWindowFlags.Default, DisplayDevice.AvailableDisplays.Last());

            gameWindow.MakeCurrent();
            gameWindow.Context.LoadAll();

            Utils.GLRenderProperties(WINDOW_WIDTH, WINDOW_HEIGHT);

            Camera = Factory<Camera.Camera>.Create(CameraPosition0, LookAt0, new Vector3(0, 1, 0));
            

            Light = LightFactory.Create(new Vector3((float) -350.0f , 300.0f, 0.0f), new Color4(255, 255, 255, 1), new Color4(255, 255, 255, 1), new Color4(252,252,252,1), LightName.Light0);
            Light.Load();



            Terrain = Terrainfactory.Create(Utils.GetImageResource<ITerrain>("Landscape.Terrains.TOPOMAP1.GIF"),
                                            Utils.GetImageResource<ITerrain>("Landscape.Terrains.Dirt.jpg"),
                                            Utils.GetImageResource<ITerrain>("Landscape.Terrains.sand.jpg"),
                                            Utils.GetImageResource<ITerrain>("Landscape.Terrains.Grass.png"),
                                            Utils.GetImageResource<ITerrain>("Landscape.Terrains.Rock.png"));
            Terrain.Load();

            
            CubeMap = CubeMapFactory.Create(2500, false, new Vector3(256, 0, 256), 
                                                  Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_front.jpg"),
                                                  Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_back.jpg"),
                                                  Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_front.jpg"),
                                                  Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_top.jpg"),
                                                  Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_left.jpg"),
                                                  Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_right.jpg")
                                                  );
            CubeMap.Load();

            WoodenChest = CubeMapFactory.Create(100, true, new Vector3(256, 150, 256), Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.plank.jpg"));
            WoodenChest.Load();

            Water = new Water(WATER_WIDTH, WATER_HEIGHT);
            Water.Load();

            SeaBed = PlaneFactory.Create(true, new VBO() { Position = new Vector3(0, -70, 0), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 0) },
                                                       new VBO() { Position = new Vector3(0, -70, WATER_HEIGHT ), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1) },
                                                       new VBO() { Position = new Vector3(WATER_WIDTH , -70, WATER_HEIGHT ), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(1, 1) },
                                                       new VBO() { Position = new Vector3(WATER_WIDTH , -70, 0), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(1, 0) },
                                                       Utils.GetImageResource<ITerrain>("Landscape.Terrains.seabed.jpg"), TextureWrapMode.ClampToEdge);
            SeaBed.Load();

            Frustum = new Frustum.Frustum(FAR_PLANE, NEAR_PLANE, (float)WINDOW_WIDTH / WINDOW_HEIGHT, FOVY);
            Frustum.Load();

            BirdTexture = FramBufferOBjectFactory.Create(512, 512);
            BirdTexture.Load();

            gameWindow.RenderFrame += gameWindow_RenderFrame;
            gameWindow.UpdateFrame += gameWindow_UpdateFrame;
            
            gameWindow.Keyboard.KeyDown += Keyboard_KeyDown;
            gameWindow.Run(60.0,30.0);
        }

        static void gameWindow_UpdateFrame(object sender, FrameEventArgs e) 
        {
            ViewMatrix = Matrix4.LookAt(Camera.CameraPosition, Camera.CameraLookAt, Camera.UpVector);
            BirdViewMatrix = Matrix4.LookAt(new Vector3(Camera.CameraPosition.X, Camera.CameraPosition.Y + 300, Camera.CameraPosition.Z), new Vector3(Camera.CameraPosition.X, Camera.CameraPosition.Y, Camera.CameraPosition.Z), new Vector3(0, 0, 1));
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FOVY, (float)WINDOW_WIDTH / WINDOW_HEIGHT, NEAR_PLANE, FAR_PLANE);

            Frustum.ExctractPlanes(Camera);
            

            GameWindow gameWindow = (GameWindow)sender;
            gameWindow.Keyboard.KeyRepeat = true;
        }

        static void gameWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            
            InvertedViewMatrix = Matrix4.Mult(ViewMatrix, new Matrix4(1, 0, 0, 0,
                                                                      0, -1, 0, 0,
                                                                      0, 0, 1, 0,
                                                                      0, 0, 0, 1));
            
            BirdTexture.GenerateProjectiveTexture(BeginMode.Triangles, BirdViewMatrix, ProjectionMatrix,  null, RenderWholeWorld);

            Water.MakeReflectionTextures(BeginMode.Triangles, ViewMatrix, InvertedViewMatrix, ProjectionMatrix, Water.ReflectionClipPlane, RenderAllWithoutWater);
            Water.MakeRefractionTextures(BeginMode.Triangles, ViewMatrix, InvertedViewMatrix, ProjectionMatrix, Water.RefractionClipPlane, RenderAllWithoutWater);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.ClearColor(Color.CornflowerBlue);
            {
                RenderWholeWorld(BeginMode.Triangles, Matrix4.Identity, ProjectionMatrix, ViewMatrix);
            }
            GL.Flush();
            
            Utils.MakeBirdMap(BirdTexture.TextureHandle, UIInputData.IsMapOn);
           

            GameWindow gameWindow = (GameWindow)sender;
            gameWindow.Context.SwapBuffers();
        }

        private static void RenderWholeWorld(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, float[] clipPlaneEq = null)
        {
            RenderAllWithoutWater(mode,worldMatrix, projectionMatrix, viewMatrix, clipPlaneEq);
            Water.Render(mode, worldMatrix, viewMatrix, projectionMatrix, Light, Camera, UIInputData.IsOceanOn);            
        }

        private static void RenderAllWithoutWater(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, float[] clipPlaneEq)
        {
            GL.Disable(EnableCap.DepthTest);
            {
              Matrix4 mat = Matrix4.CreateTranslation(Camera.CameraPosition);
              Matrix4 tempMatrix = viewMatrix;
              tempMatrix = Matrix4.Mult(mat, viewMatrix);

              CubeMap.Render(mode, worldMatrix, projectionMatrix, tempMatrix, UIInputData.IsCubeMapOn, null, clipPlaneEq);
            }
            GL.Enable(EnableCap.DepthTest);

            WoodenChest.Render(mode, worldMatrix, projectionMatrix, viewMatrix, UIInputData.IsWoodenChestOn,Frustum);

            Terrain.Render(mode, worldMatrix, viewMatrix, projectionMatrix, Light, UIInputData.IsTerrainOn);
            Light.Stop();
            //Frustum.ShowFrustum(ProjectionMatrix, viewMatrix);
            Light.Start();
            if (UIInputData.IsOceanOn && (Frustum.SquareInFrustum(SeaBed.Points) == 1 || Frustum.SquareInFrustum(SeaBed.Points) == 2))
                SeaBed.Render(mode, worldMatrix, projectionMatrix, viewMatrix);
         
        }

        static void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            KeyBoard.Keyboard_KeyDown(ref CubeMap, Camera, Light, Terrain, ref CameraPosition0, ref CameraAngle,FAR_PLANE, e);
        }
    }
}
