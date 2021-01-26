using System.Collections.Generic;

namespace SpaceInvaders
{
   
    class Invader : GameObject
    {
        public Invader(GameObjectLocation objectPlace)
        {
            Figure = GameSettings.Invader;
            GameObjectLocation = objectPlace;
        }

        public static List<GameObject> GetInvaders(int Level)
        {
            List<GameObject> swarm = new List<GameObject>();

            int startX = GameSettings.SwarmStartX;
            int startY = GameSettings.SwarmStartY;

            for (int y = 0; y < GameSettings.NumberOfSwarmRows * Level; y++) 
            {
                for (int x = 0; x < GameSettings.NumberOfSwarm * Level; x++) 
                {
                    GameObjectLocation objectPlace = new GameObjectLocation() { X = startX + x, Y = startY + y };
                    GameObject invader = new Invader(objectPlace);
                    swarm.Add(invader);
                }
            }

            return swarm;
        }
    }
}