using System;

namespace SpaceInvaders
{
    class UIController
    {
        public event EventHandler LeftArrowPressed;
        public event EventHandler RightArrowPressed;
        public event EventHandler SpacebarPressed; 

        
        public void StartListening()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true); 

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey();
                    }

                    if (keyInfo.Key.Equals(ConsoleKey.LeftArrow))
                    {
                        LeftArrowPressed(this, new EventArgs());
                    }

                    if (keyInfo.Key.Equals(ConsoleKey.RightArrow))
                    {
                        RightArrowPressed(this, new EventArgs());
                    }

                    if (keyInfo.Key.Equals(ConsoleKey.Spacebar))
                    {
                        SpacebarPressed(this, new EventArgs());
                    }
                }
            }

        }
    }
}
