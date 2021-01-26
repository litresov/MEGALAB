using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    
    class Wall : GameObject
    {
        public Wall(GameObjectLocation objectPlace)
        {
            Figure = GameSettings.Wall;
            GameObjectLocation = objectPlace;
        }

        public static List<GameObject> GetWall() // Генерация стен
        {
            List<GameObject> wall = new List<GameObject>();

            int startX = GameSettings.WallStartX;
            int startY = GameSettings.WallStartY;

            for (int y = 0; y < GameSettings.NumberOfWallRows; y++) 
            {
                if (y == 0 | y == 28)
                {
                    for (int x = 0; x < GameSettings.NumberOfWall; x++)
                    {
                        GameObjectLocation objectPlace = new GameObjectLocation() { X = startX + x, Y = startY + y };
                        GameObject wallObj = new Wall(objectPlace);
                        wall.Add(wallObj);
                    }
                }
                else
                {
                    for (int x = 0; x < GameSettings.NumberOfWall; x++)
                    {
                        if (x == 0 | x == 79)
                        {
                            GameObjectLocation objectPlace = new GameObjectLocation() { X = startX + x, Y = startY + y };
                            GameObject wallObj = new Wall(objectPlace);
                            wall.Add(wallObj);
                        }
                    }
                }
            }
            return wall;
        }
    }
}
