using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace SpaceInvaders
{
    class Program
    {
        
        static GameEngine gameEngine;  

        static UIController Controller; 

        static void Main(string[] args)
        {
            TakeSettings("E:\\MEGALAB\\SpaceInvaders\\SpaceInvaders\\bin\\Debug\\megalaba.xlsx");
            Initialize();
            gameEngine.Run();
        }

        public static void Initialize()
        {
            gameEngine = new GameEngine(); 
            Controller = new UIController();
            
            Controller.LeftArrowPressed += (obj, arg) => gameEngine.MovePlayerLeft();
            Controller.RightArrowPressed += (obj, arg) => gameEngine.MovePlayerRight();
            Controller.SpacebarPressed += (obj, arg) => gameEngine.ShotPlayer();

            Thread uIthread1 = new Thread(Controller.StartListening); 
            uIthread1.Start();
        }

        public static void TakeSettings(string path) // Используем COM обьект для получения игровых настроек
        {
        
            List<dynamic> list; 
            object temp; 
            Type GameSettingsT = Type.GetTypeFromProgID("ComObjectForLab.GameSettings");
            object gameSettings = Activator.CreateInstance(GameSettingsT);
            temp = GameSettingsT.InvokeMember("GetSettings", System.Reflection.BindingFlags.InvokeMethod, null, gameSettings, new object[] { path });
            list = temp as List<dynamic>;

            GameSettings.ConsoleWidth = Convert.ToInt32(list[0]);
            GameSettings.ConsoleHeight = Convert.ToInt32(list[1]);
            GameSettings.NumberOfSwarmRows = Convert.ToInt32(list[2]);
            GameSettings.NumberOfSwarm = Convert.ToInt32(list[3]);
            GameSettings.SwarmStartX = Convert.ToInt32(list[4]);
            GameSettings.SwarmStartY = Convert.ToInt32(list[5]);
            GameSettings.Invader = Convert.ToChar(list[6]);
            GameSettings.SwarmSpeed = Convert.ToInt32(list[7]);
            GameSettings.PlayerStartX = Convert.ToInt32(list[8]);
            GameSettings.PlayerStartY = Convert.ToInt32(list[9]);
            GameSettings.Player = Convert.ToChar(list[10]);
            GameSettings.PlayerLifes = Convert.ToInt32(list[11]);
            GameSettings.MaxPlayerLifes = Convert.ToInt32(list[12]);
            GameSettings.PlayerLifeRecoveryTime = Convert.ToInt32(list[13]);
            GameSettings.WallStartX = Convert.ToInt32(list[14]);
            GameSettings.WallStartY = Convert.ToInt32(list[15]);
            GameSettings.Wall = Convert.ToChar(list[16]);
            GameSettings.NumberOfWallRows = Convert.ToInt32(list[17]);
            GameSettings.NumberOfWall = Convert.ToInt32(list[18]);
            GameSettings.Missle = Convert.ToChar(list[19]);
            GameSettings.MissleDamage = Convert.ToInt32(list[20]);
            GameSettings.MissleSpeed = Convert.ToInt32(list[21]);
            GameSettings.MissleFrequency = Convert.ToInt32(list[22]);
            GameSettings.MaxInvaderMissles = Convert.ToInt32(list[23]);
            GameSettings.GameSpeed = Convert.ToInt32(list[24]);
            GameSettings.Level = Convert.ToInt32(list[25]);
        }
    }
}