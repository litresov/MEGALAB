using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace SpaceInvaders
{
  
    class SceneRender
    {
        int windowWidth;
        int windowHeight;

        char[,] window;

        public SceneRender()
        {
            windowHeight = GameSettings.ConsoleHeight;
            windowWidth = GameSettings.ConsoleWidth;
            window = new char[GameSettings.ConsoleHeight, GameSettings.ConsoleWidth];

            Console.WindowHeight = GameSettings.ConsoleHeight;
            Console.WindowWidth = GameSettings.ConsoleWidth;

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
        }
        public void Render(ObjectPos objectPos)
        {
            AddInvadersForRendering(objectPos.invaders);
            AddGroundForRendering(objectPos.wall);
            AddPlayerMisslesForRendering(objectPos.playerMissle);
            AddInvadersMisslesForRendering(objectPos.invaderMissle);

            AddPlayerForRendering(objectPos.player);

            string render = $"Ваши жизни: {GameSettings.PlayerLifes}. Уровень: {GameSettings.Level}\r\n";

            for (int y = 0; y < windowHeight; y++)
            {
                for (int x = 0; x < windowWidth; x++)
                {
                    render += window[y, x];
                }
                render += "\r\n";
            }
            Console.WriteLine(render);
            Console.SetCursorPosition(0, 0);
        }

        public void AddGameObjectForRendering(GameObject gameObject)
        {
            if (gameObject.GameObjectLocation.Y < window.GetLength(0) &&
                gameObject.GameObjectLocation.X < window.GetLength(1))
            {
                window[gameObject.GameObjectLocation.Y, gameObject.GameObjectLocation.X] = gameObject.Figure;
            }
        }

        public void AddPlayerForRendering(object gameObject)
        {
            int size = Marshal.SizeOf(typeof(int));
            int xPlayer, yPlayer;
            //проец
            using (var accessorPlayer = GameEngine.PlayerLocation.CreateViewAccessor(0, GameEngine.length, MemoryMappedFileAccess.Read)) 
            {
                accessorPlayer.Read(0, out xPlayer);
                accessorPlayer.Read(size, out yPlayer);
            }
            object gameObjectFigure;
            ObjectPos.PlayerT.GetProperty("X").SetValue(gameObject, xPlayer);
            ObjectPos.PlayerT.GetProperty("Y").SetValue(gameObject, yPlayer);
            gameObjectFigure = ObjectPos.PlayerT.GetProperty("Figure").GetValue(gameObject);
            if (yPlayer < window.GetLength(0) && 
                xPlayer < window.GetLength(1))
            {
                window[yPlayer, xPlayer] = Convert.ToChar(gameObjectFigure);
            }
        }

        public void AddInvadersForRendering(List<GameObject> gameObjects)
        {
            GameEngine.mutex.WaitOne(); 
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
            GameEngine.mutex.ReleaseMutex();
        }

        public void AddPlayerMisslesForRendering(List<GameObject> gameObjects)
        {
            GameEngine.autoResetEvent.WaitOne(); 
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
            GameEngine.autoResetEvent.Set(); 
        }

        public void AddInvadersMisslesForRendering(List<GameObject> gameObjects)
        {
            GameEngine.semaphore.WaitOne();
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
            GameEngine.semaphore.Release();
        }

        public void AddGroundForRendering(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
        }

        public void ClearScene()
        {
            for (int y = 0; y < windowHeight; y++)
            {
                for (int x = 0; x < windowWidth; x++)
                {
                    window[y, x] = ' ';
                }
            }
        }
    }
}