﻿namespace SpaceInvaders
{
    
    class PlayerMissle : GameObject
    {
        public PlayerMissle(GameObjectLocation objectPlace)
        {
            GameObjectLocation misslePlace = new GameObjectLocation() { X = objectPlace.X, Y = objectPlace.Y - 1 };
            Figure = GameSettings.Missle;
            GameObjectLocation = misslePlace;
        }
    }
}