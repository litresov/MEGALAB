using System;
using System.Collections.Generic;


namespace SpaceInvaders
{
    
    class ObjectPos
    {
        public List<GameObject> invaders;

        public List<GameObject> wall;

        public object player;
        
        public List<GameObject> playerMissle;
        public List<GameObject> invaderMissle;

        public static Type PlayerT = Type.GetTypeFromProgID("ComObjects.Player"); 
        public static dynamic WinDefMessage = (dynamic)Microsoft.VisualBasic.Interaction.GetObject(@"script:E:\MEGALAB\SpaceInvaders\WSC\MEGALABScript.wsc", null);

        public ObjectPos(int Level) 
        {
            invaders = Invader.GetInvaders(Level);
            wall = Wall.GetWall();

            player = Activator.CreateInstance(PlayerT, GameSettings.PlayerStartX, GameSettings.PlayerStartY, GameSettings.Player); 
            playerMissle = new List<GameObject>();
            invaderMissle = new List<GameObject>();
            
        }
    }
}