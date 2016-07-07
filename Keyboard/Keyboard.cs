using OpenTK;
using System;
using Camera;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using Utilities;
using Lights;
using EnvironmentMap;
using System.Threading.Tasks;
using Landscape;

namespace Keyboard
{
    public class KeyBoard
    {
        public static void Keyboard_KeyDown(ref ICubeMap cubeMap, ICamera camera, ILight light, ITerrain terrain, ref Vector3 initCameraPosition, ref float cameraAngle, float farDist, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    initCameraPosition = CameraFoward(camera, initCameraPosition, cameraAngle, farDist);
                    break;
                case Key.Down:
                    initCameraPosition = CameraBackward(camera, initCameraPosition, cameraAngle, farDist);
                    break;
                case Key.Right:
                    cameraAngle = CameraRight(camera, cameraAngle, farDist);
                    break;
                case Key.Left:
                    cameraAngle = CameraLeft(camera, cameraAngle, farDist);
                    break;
                case Key.A:
                    initCameraPosition = CameraUp(camera, initCameraPosition, cameraAngle, farDist);
                    break;
                case Key.Z:
                    initCameraPosition = CameraDown(camera, initCameraPosition, cameraAngle, farDist);
                    break;
                case Key.W:
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    break;
                case Key.S:
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    break;
                case Key.M:
                    UIInputData.IsMapOn = true;
                    break;
                case Key.Escape:
                    UIInputData.IsMapOn = false;
                    break;
                case Key.T:
                    UIInputData.IsTerrainOn = true;
                    UIInputData.IsOceanOn = false;
                    break;
                case Key.O:
                    UIInputData.IsTerrainOn = false;
                    UIInputData.IsOceanOn = true;
                    break;
                case Key.R:
                    UIInputData.FresnelParameter += 0.1f;
                    break;
                case Key.F:
                    UIInputData.FresnelParameter -= 0.1f;
                    break;
                case Key.C:
                    if (UIInputData.IsWoodenChestOn)
                        UIInputData.IsWoodenChestOn = false;
                    else
                        UIInputData.IsWoodenChestOn = true;
                    break;
                case Key.F1:
                    UIInputData.IsWaterCalm = false;
                    break;
                case Key.F2:
                    UIInputData.IsWaterCalm = true;
                    break;
                case Key.F12:
                    UIInputData.StormParameter = new float[] { 2, 1.5f, 1.5f, 4, 2 };
                        break;
                case Key.F11:
                        UIInputData.StormParameter = new float[] { 1, 1, 1, 1, 1 };
                    break;
                case Key.D:
                    if(UIInputData.isDiffuseOn)
                        UIInputData.isDiffuseOn = false;
                    else
                        UIInputData.isDiffuseOn = true;
                    break;
                case Key.F3:
                    Vector3 pos = light.LightPosition;
                    light.LightPosition = new Vector3(pos.X + 1.0f, light.LightPosition.Y, light.LightPosition.Z);
                    break;
                case Key.F4:
                    pos = light.LightPosition;
                    light.LightPosition = new Vector3(pos.X - 1.0f, light.LightPosition.Y, light.LightPosition.Z);
                    break;
                case Key.F5:
                    pos = light.LightPosition;
                    light.LightPosition = new Vector3(pos.X, light.LightPosition.Y + 1.0f, light.LightPosition.Z);
                    break;
                case Key.F6:
                    pos = light.LightPosition;
                    light.LightPosition = new Vector3(pos.X, light.LightPosition.Y - 1.0f, light.LightPosition.Z);
                    break;
                case Key.F7:
                    pos = light.LightPosition;
                    light.LightPosition = new Vector3(pos.X, light.LightPosition.Y, light.LightPosition.Z + 1.0f);
                    break;
                case Key.F8:
                    pos = light.LightPosition;
                    light.LightPosition = new Vector3(pos.X, light.LightPosition.Y, light.LightPosition.Z - 1.0f);
                    break;
                case Key.Number1:
                    ChangeCubeMap(ref cubeMap, "Mountain");
                    break;
                case Key.Number2:
                    ChangeCubeMap(ref cubeMap, "Desert");
                    break;
                case Key.Number3:
                    ChangeCubeMap(ref cubeMap, "Frozen");
                    break;
                case Key.Number4:
                    ChangeCubeMap(ref cubeMap, "Spooky");
                    break;
                case Key.Number5:
                    terrain.Load(true);
                    break;
                case Key.Number6:
                    terrain.Load(true, true);
                    break;

                    
            }
        }

        private static void ChangeCubeMap(ref ICubeMap cubeMap, string cubeNameType)
        {
            cubeMap = CubeMapFactory.Create(2500, false, new Vector3(0, 0, 0),
                                                  Utils.GetImageResource<ICubeMap>(string.Format("EnvironmentMap.Textures.{0}.{0}_front.jpg", cubeNameType)),
                                                  Utils.GetImageResource<ICubeMap>(string.Format("EnvironmentMap.Textures.{0}.{0}_back.jpg", cubeNameType)),
                                                  Utils.GetImageResource<ICubeMap>(string.Format("EnvironmentMap.Textures.{0}.{0}_back.jpg", cubeNameType)),
                                                  Utils.GetImageResource<ICubeMap>(string.Format("EnvironmentMap.Textures.{0}.{0}_top.jpg", cubeNameType)),
                                                  Utils.GetImageResource<ICubeMap>(string.Format("EnvironmentMap.Textures.{0}.{0}_left.jpg", cubeNameType)),
                                                  Utils.GetImageResource<ICubeMap>(string.Format("EnvironmentMap.Textures.{0}.{0}_right.jpg", cubeNameType))
                                                  );
            cubeMap.Load();
        }


        private static Vector3 CameraDown(ICamera camera, Vector3 initCameraPosition, float cameraAngle, float farDist)
        {
            initCameraPosition.Y -= 6.0f;
            camera.CameraPosition = new Vector3(initCameraPosition.X, initCameraPosition.Y, initCameraPosition.Z);
            camera.CameraLookAt = camera.CameraPosition + farDist * new Vector3((float)Math.Sin(cameraAngle * Math.PI / 180), 0, (float)Math.Cos(cameraAngle * Math.PI / 180));
            Console.WriteLine(camera.CameraPosition);
            return initCameraPosition;
        }

        private static Vector3 CameraUp(ICamera camera, Vector3 initCameraPosition, float cameraAngle, float farDist)
        {
            initCameraPosition.Y += 6.0f;
            camera.CameraPosition = new Vector3(initCameraPosition.X, initCameraPosition.Y, initCameraPosition.Z);
            camera.CameraLookAt = camera.CameraPosition + farDist * new Vector3((float)Math.Sin(cameraAngle * Math.PI / 180), 0, (float)Math.Cos(cameraAngle * Math.PI / 180));
            Console.WriteLine(camera.CameraPosition);
            return initCameraPosition;
        }

        private static float CameraLeft(ICamera camera, float cameraAngle, float farDist)
        {
            cameraAngle += 4.0f;
            camera.CameraLookAt = camera.CameraPosition + farDist* new Vector3((float)Math.Sin(cameraAngle * Math.PI / 180), 0, (float)Math.Cos(cameraAngle * Math.PI / 180));
            Console.WriteLine(cameraAngle);
            return cameraAngle;
        }

        private static float CameraRight(ICamera camera, float cameraAngle, float farDist)
        {
            cameraAngle -= 4.0f;
            camera.CameraLookAt = camera.CameraPosition + farDist * new Vector3((float)Math.Sin(cameraAngle * Math.PI / 180), 0, (float)Math.Cos(cameraAngle * Math.PI / 180));
            Console.WriteLine(cameraAngle);
            return cameraAngle;
        }

        private static Vector3 CameraBackward(ICamera camera, Vector3 initCameraPosition, float cameraAngle, float farDist)
        {
            camera.CameraPosition = new Vector3(initCameraPosition.X -= 10 * (float)Math.Sin(cameraAngle * Math.PI / 180), initCameraPosition.Y, initCameraPosition.Z -= 10 * (float)Math.Cos(cameraAngle *Math.PI/180));
            camera.CameraLookAt = camera.CameraPosition + farDist * new Vector3((float)Math.Sin(cameraAngle * Math.PI / 180), 0, (float)Math.Cos(cameraAngle * Math.PI / 180));
            camera.UpVector = new Vector3(0, 1, 0);
            Console.WriteLine(camera.CameraPosition);
            return initCameraPosition;
        }

        private static Vector3 CameraFoward(ICamera camera, Vector3 initCameraPosition, float cameraAngle, float farDist)
        {
            camera.CameraPosition = new Vector3(initCameraPosition.X += 10 * (float)Math.Sin(cameraAngle * Math.PI / 180), initCameraPosition.Y, initCameraPosition.Z += 10 * (float)Math.Cos(cameraAngle * Math.PI/180));
            camera.CameraLookAt = camera.CameraPosition + farDist * new Vector3((float)Math.Sin(cameraAngle * Math.PI / 180), 0, (float)Math.Cos(cameraAngle * Math.PI / 180));
            camera.UpVector = new Vector3(0, 1, 0);
            Console.WriteLine(camera.CameraPosition);
            return initCameraPosition;
        }
    }
}
