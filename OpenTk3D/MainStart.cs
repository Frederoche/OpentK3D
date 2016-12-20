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
using PrimitiveShapes;
using CustomVertex;
using Utils;


namespace OpenTk3D
{
    class MainStart
    {
        private static ICamera  _camera;
        
        private static ICubeMap _cubeMap;
        private static ICubeMap _woodenChest;

        private static ITerrain _terrain;
        private static ILight   _light;
        private static IPlane _seaBed;

        private static IFrameBufferObject _birdTexture;

        private static IWater _water;

        private static Vector3 _cameraPosition0 = new Vector3(-378.0f, 75.0f, -8.0f);
        private static readonly Vector3 LookAt0 = new Vector3(0, 0, 0);

        private static float _cameraAngle = 0.898f;

        private static Matrix4 _viewMatrix;
        private static Matrix4 _birdViewMatrix;
        private static Matrix4 _invertedViewMatrix;
        private static Matrix4 _projectionMatrix;

        private const int WindowHeight = 724;
        private const int WindowWidth  = 1024;
        private const float Fovy = (float) (Math.PI / 2.0);
        private const float NearPlane = 1.0f;
        private const float FarPlane  = 5000.0f;

        private const int WaterWidth = 512;
        private const int WaterHeight = 512;

        [STAThread]
        static void Main(string[] args) 
        {
            var gameWindow = new GameWindow(WindowWidth, WindowHeight, new GraphicsMode(32, 24, 0, 8), "Ocean sim (Grestner waves) and terrain", GameWindowFlags.Default, DisplayDevice.AvailableDisplays.Last());

            gameWindow.MakeCurrent();
            gameWindow.Context.LoadAll();

            Utils.Utils.GLRenderProperties(WindowWidth, WindowHeight);

            _camera = Factory<Camera.Camera>.Create(_cameraPosition0, LookAt0, new Vector3(0, 1, 0));
            

            _light = LightFactory.Create(new Vector3(-350.0f , 300.0f, 0.0f), new Color4(255, 255, 255, 1), new Color4(255, 255, 255, 1), new Color4(252,252,252,1), LightName.Light0);
            _light.Load();



            _terrain = Terrainfactory.Create(Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.TOPOMAP1.GIF"),
                                            Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.Dirt.jpg"),
                                            Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.sand.jpg"),
                                            Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.Grass.png"),
                                            Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.Rock.png"));
            _terrain.Load();

            
            _cubeMap = CubeMapFactory.Create(2500, false, new Vector3(256, 0, 256), 
                                                  Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_front.jpg"),
                                                  Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_back.jpg"),
                                                  Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_front.jpg"),
                                                  Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_top.jpg"),
                                                  Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_left.jpg"),
                                                  Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.Desert.Desert_right.jpg")
                                                  );
            _cubeMap.Load();

            _woodenChest = CubeMapFactory.Create(100, true, new Vector3(256, 150, 256), Utils.Utils.GetImageResource<ICubeMap>("EnvironmentMap.Textures.plank.jpg"));
            _woodenChest.Load();

            _water = new Water(WaterWidth, WaterHeight);
            _water.Load();

            _seaBed = PlaneFactory.Create(true, new Vbo() { Position = new Vector3(0, -70, 0), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 0) },
                                                       new Vbo() { Position = new Vector3(0, -70, WaterHeight ), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1) },
                                                       new Vbo() { Position = new Vector3(WaterWidth , -70, WaterHeight ), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(1, 1) },
                                                       new Vbo() { Position = new Vector3(WaterWidth , -70, 0), Normal = new Vector3(0, 1, 0), TexCoord = new Vector2(1, 0) },
                                                       Utils.Utils.GetImageResource<ITerrain>("Landscape.Terrains.seabed.jpg"), TextureWrapMode.ClampToEdge);
            _seaBed.Load();

            _birdTexture = FramBufferOBjectFactory.Create(512, 512);
            _birdTexture.Load();

            gameWindow.RenderFrame += gameWindow_RenderFrame;
            gameWindow.UpdateFrame += gameWindow_UpdateFrame;
            
            gameWindow.Keyboard.KeyDown += Keyboard_KeyDown;
            gameWindow.Run(60.0,30.0);
        }

        static void gameWindow_UpdateFrame(object sender, FrameEventArgs e) 
        {
            _viewMatrix = Matrix4.LookAt(_camera.CameraPosition, _camera.CameraLookAt, _camera.UpVector);
            _birdViewMatrix = Matrix4.LookAt(new Vector3(_camera.CameraPosition.X, _camera.CameraPosition.Y + 300, _camera.CameraPosition.Z), new Vector3(_camera.CameraPosition.X, _camera.CameraPosition.Y, _camera.CameraPosition.Z), new Vector3(0, 0, 1));
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Fovy, (float)WindowWidth / WindowHeight, NearPlane, FarPlane);

            GameWindow gameWindow = (GameWindow)sender;
            gameWindow.Keyboard.KeyRepeat = true;
        }

        static void gameWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            
            _invertedViewMatrix = Matrix4.Mult(_viewMatrix, new Matrix4(1, 0, 0, 0,
                                                                      0, -1, 0, 0,
                                                                      0, 0, 1, 0,
                                                                      0, 0, 0, 1));
            
            _birdTexture.GenerateProjectiveTexture(BeginMode.Triangles, _birdViewMatrix, _projectionMatrix,  null, RenderWholeWorld);

            _water.MakeReflectionTextures(BeginMode.Triangles, _viewMatrix, _invertedViewMatrix, _projectionMatrix, _water.ReflectionClipPlane, RenderAllWithoutWater);
            _water.MakeRefractionTextures(BeginMode.Triangles, _viewMatrix, _invertedViewMatrix, _projectionMatrix, _water.RefractionClipPlane, RenderAllWithoutWater);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.ClearColor(Color.CornflowerBlue);
            {
                RenderWholeWorld(BeginMode.Triangles, Matrix4.Identity, _projectionMatrix, _viewMatrix);
            }
            GL.Flush();
            
            Utils.Utils.MakeBirdMap(_birdTexture.TextureHandle, UIInputData.IsMapOn);
           

            GameWindow gameWindow = (GameWindow)sender;
            gameWindow.Context.SwapBuffers();
        }

        private static void RenderWholeWorld(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, float[] clipPlaneEq = null)
        {
            RenderAllWithoutWater(mode,worldMatrix, projectionMatrix, viewMatrix, clipPlaneEq);
            _water.Render(mode, worldMatrix, viewMatrix, projectionMatrix, _light, _camera, UIInputData.IsOceanOn);            
        }

        private static void RenderAllWithoutWater(BeginMode mode, Matrix4 worldMatrix, Matrix4 projectionMatrix, Matrix4 viewMatrix, float[] clipPlaneEq)
        {
            GL.Disable(EnableCap.DepthTest);
            {
              Matrix4 mat = Matrix4.CreateTranslation(_camera.CameraPosition);
                var tempMatrix = Matrix4.Mult(mat, viewMatrix);

              _cubeMap.Render(mode, worldMatrix, projectionMatrix, tempMatrix, UIInputData.IsCubeMapOn, clipPlaneEq);
            }
            GL.Enable(EnableCap.DepthTest);

            _woodenChest.Render(mode, worldMatrix, projectionMatrix, viewMatrix, UIInputData.IsWoodenChestOn);

            _terrain.Render(mode, worldMatrix, viewMatrix, projectionMatrix, _light, UIInputData.IsTerrainOn);
            _light.Stop();
            
            _light.Start();
            if (UIInputData.IsOceanOn)
                _seaBed.Render(mode, worldMatrix, projectionMatrix, viewMatrix);
         
        }

        static void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            KeyBoard.Keyboard_KeyDown(ref _cubeMap, _camera, _light, _terrain, ref _cameraPosition0, ref _cameraAngle,FarPlane, e);
        }
    }
}
