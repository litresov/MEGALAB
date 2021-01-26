using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    static class GameSettings
    {
        //Размеры консоли
        public static int ConsoleWidth { get; set; }
        public static int ConsoleHeight { get; set; }

        //Количество рядов противников, Общее количество противников
        public static int NumberOfSwarmRows { get; set; }
        public static int NumberOfSwarm { get; set; }

        //Начальные координаты пачки противников X,Y
        public static int SwarmStartX { get; set; }
        public static int SwarmStartY { get; set; }
        
        //Символ обьекта противник
        public static char Invader { get; set; }

        //Скорость пачки
        public static int SwarmSpeed { get; set; } 

        //Начальные координаты игрока X,Y
        public static int PlayerStartX { get; set; } 
        public static int PlayerStartY { get; set; } 

        //Символ обьекта игрок
        public static char Player { get; set; } 

        //Здоровье игрока
        public static int PlayerLifes { get; set; }

        //Максимальное здоровье игрока
        public static int MaxPlayerLifes { get; set; }

        //Время регенерации хп в м.сек
        public static int PlayerLifeRecoveryTime { get; set; }

        //Начальные координаты стены X,Y
        public static int WallStartX { get; set; }
        public static int WallStartY { get; set; }

        //Символ обьекта стена
        public static char Wall { get; set; }

        //Количество рядов стен
        public static int NumberOfWallRows { get; set; }

        //Количество стен
        public static int NumberOfWall { get; set; }

        //Символ обьекта снаряд
        public static char Missle { get; set; } 

        //Дамаг от снаряда
        public static int MissleDamage { get; set; }

        //Скорость снаряда
        public static int MissleSpeed { get; set; } 

        //Задержка между выстрелами игрока
        public static int MissleFrequency { get; set; } 

        //Максимальное количество снарядов противника в один момент
        public static int MaxInvaderMissles { get; set; } 

        //Скорость игры
        public static int GameSpeed { get; set; }

        public static int Level { get; set; }
    }
}
