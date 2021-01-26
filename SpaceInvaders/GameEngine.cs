using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SpaceInvaders
{
    
    class GameEngine
    {
        private bool IsGameRunning;
        private SceneRender sceneRender; 
        private ObjectPos objectPos;

        private int level = 1;
        private bool XInvaderMoveFlag = true;
        private bool CanShoot = true; 
        private System.Timers.Timer CanShootTimer;
        public static Mutex mutex = new Mutex();
        public static AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        public static Semaphore semaphore = new Semaphore(1, 1);
        public static int length = 100; 
        public static MemoryMappedFile PlayerLocation = MemoryMappedFile.CreateNew("Player", length); 

        public GameEngine()
        {
            IsGameRunning = true;
            objectPos = new ObjectPos(GameSettings.Level);
            sceneRender = new SceneRender();
            CanShootTimer = new System.Timers.Timer(GameSettings.MissleFrequency);
            CanShootTimer.Elapsed += ChangeCanShootFlagToTrue;
            CanShootTimer.AutoReset = true;
            CanShootTimer.Enabled = true;
        }

        public void Run()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Thread tSwarmMove = new Thread(InvadersMove); 
            tSwarmMove.Start();
            Thread tPlayerMissleMove = new Thread(PlayerMissleMove);
            tPlayerMissleMove.Start();
            Thread tInvaderMissleMove = new Thread(InvaderMissleMove); 
            tInvaderMissleMove.Start();
            Thread tShotInvader = new Thread(ShotInvader); 
            tShotInvader.Start();
            
            int size = Marshal.SizeOf(typeof(int));
            using (var accessorPlayer = PlayerLocation.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
            {
                accessorPlayer.Write(0, GameSettings.PlayerStartX);
                accessorPlayer.Write(size, GameSettings.PlayerStartY);
            }

            do
            {
                sceneRender.Render(objectPos);
                Thread.Sleep(GameSettings.GameSpeed);
                // if (scene.invaders.Count == 0)
                //    IsGameRunning = false;
                sceneRender.ClearScene();
                
                if (objectPos.invaders.Count == 0 & GameSettings.Level != 3)
                {
                    GameSettings.Level++;
                    GameSettings.MaxInvaderMissles += 2;
                    objectPos.invaders = Invader.GetInvaders(GameSettings.Level);
                }
                else if (objectPos.invaders.Count == 0 & GameSettings.Level == 3)
                {
                    ObjectPos.WinDefMessage.Win();
                }
                else if (GameSettings.PlayerLifes == 0)
                {
                    IsGameRunning = false;
                }
            } while (IsGameRunning);
            ObjectPos.WinDefMessage.Defeat();
            Process.GetCurrentProcess().Kill();
        }

        public void MovePlayerLeft()
        {
            object x = ObjectPos.PlayerT.GetProperty("X").GetValue(objectPos.player);
            if (Convert.ToInt32(x) > 1)
                ObjectPos.PlayerT.GetProperty("X").SetValue(objectPos.player, Convert.ToInt32(x) - 1);
            using (var accessorPlayer = PlayerLocation.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
            {
                accessorPlayer.Write(0, Convert.ToInt32(x) - 1);
            }
        }

        public void MovePlayerRight()
        {
            object x = ObjectPos.PlayerT.GetProperty("X").GetValue(objectPos.player);
            if (Convert.ToInt32(x) > 1)
                ObjectPos.PlayerT.GetProperty("X").SetValue(objectPos.player, Convert.ToInt32(x) + 1);
            using (var accessorPlayer = PlayerLocation.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
            {
                accessorPlayer.Write(0, Convert.ToInt32(x) + 1);
            }
        }

        public void InvadersMove()
        {
            do
            {
                if (objectPos.invaders.Count != 0)
                {
                    object y = ObjectPos.PlayerT.GetProperty("Y").GetValue(objectPos.player);
                    GameObject LastInvader = objectPos.invaders[objectPos.invaders.Count-1];

                    GameObject FirstInvader = objectPos.invaders[0];
                    if (LastInvader.GameObjectLocation.X == 78 || FirstInvader.GameObjectLocation.X == 2)
                    {
                        if (XInvaderMoveFlag == true) XInvaderMoveFlag = false;
                        else XInvaderMoveFlag = true;
                        for (int i = 0; i < objectPos.invaders.Count; i++)
                        {
                            GameObject invader = objectPos.invaders[i];
                            invader.GameObjectLocation.Y++;

                            if (XInvaderMoveFlag)
                            {
                                invader.GameObjectLocation.X += 1;
                            }
                            else
                            {
                                invader.GameObjectLocation.X -= 1;
                            }

                            if (invader.GameObjectLocation.Y == Convert.ToInt32(y))
                                IsGameRunning = false;
                        }
                        Thread.Sleep(GameSettings.SwarmSpeed);
                    }
                    else
                    {
                        for (int i = 0; i < objectPos.invaders.Count; i++)
                        {
                            GameObject invader = objectPos.invaders[i];
                            if (XInvaderMoveFlag == true)
                            {
                                invader.GameObjectLocation.X++;
                            }
                            else if (XInvaderMoveFlag == false)
                            {
                                invader.GameObjectLocation.X--;
                            }
                        }
                        Thread.Sleep(GameSettings.SwarmSpeed);
                    }
                }
            } while (IsGameRunning);
        }

        public void ShotPlayer()
        {
            if (CanShoot == true)
            {
                object x = ObjectPos.PlayerT.GetProperty("X").GetValue(objectPos.player);
                object y = ObjectPos.PlayerT.GetProperty("Y").GetValue(objectPos.player);
                GameObjectLocation location = new GameObjectLocation()
                {
                    X = Convert.ToInt32(x),
                    Y = Convert.ToInt32(y)
                };
                PlayerMissle missle = new PlayerMissle(location);
                autoResetEvent.WaitOne();
                objectPos.playerMissle.Add(missle);
                autoResetEvent.Set();
                CanShoot = false;
            }
        }

        /// <summary>
        /// ObjectPos
        /// objectPos
        /// </summary>



        public void ShotInvader()
        {
            Random rnd = new Random();
            int temp;
            IEnumerable<GameObject> invadersPlaces;
            List<GameObject> list = new List<GameObject>();
            InvaderMissle missle;
            do
            {
                
                if ((objectPos.invaderMissle.Count < GameSettings.MaxInvaderMissles) & objectPos.invaders.Count != 0)
                {
                    do
                    {
                        temp = rnd.Next(0, objectPos.invaders.Count);
                        missle = new InvaderMissle(objectPos.invaders[temp].GameObjectLocation);
                        mutex.WaitOne(); 
                        invadersPlaces = (from a in objectPos.invaders
                                          where a.GameObjectLocation.Equals(missle.GameObjectLocation)
                                          select a);
                        list = invadersPlaces.ToList();
                        mutex.ReleaseMutex(); 
                        if (list.Count == 0) 
                        {
                            semaphore.WaitOne();
                            objectPos.invaderMissle.Add(missle);
                            semaphore.Release(); 
                        }
                    } while (list.Count != 0);
                }
            } while (IsGameRunning);
        }

        public void PlayerMissleMove()
        {
            do
            {
                for (int x = 0; x < objectPos.playerMissle.Count; x++)
                {
                    GameObject missle = objectPos.playerMissle[x];
                    if (missle.GameObjectLocation.Y == 1)
                    {
                        
                        autoResetEvent.WaitOne();
                        objectPos.playerMissle.RemoveAt(x);
                        autoResetEvent.Set();
                    }

                    missle.GameObjectLocation.Y--;
                    for (int i = 0; i < objectPos.invaders.Count; i++)
                    {
                        GameObject invader = objectPos.invaders[i];
                        if (missle.GameObjectLocation.Equals(invader.GameObjectLocation))
                        {
                            
                            mutex.WaitOne(); 
                            autoResetEvent.WaitOne();

                            objectPos.invaders.RemoveAt(i);
                            objectPos.playerMissle.RemoveAt(x);

                            mutex.ReleaseMutex();
                            autoResetEvent.Set();
                            
                            break;
                        }
                    }
                }
                Thread.Sleep(GameSettings.MissleSpeed);
            } while (IsGameRunning);

        }

        public void InvaderMissleMove() // Метод обработки снарядов противника.
        {
            do
            {
                object X = ObjectPos.PlayerT.GetProperty("X").GetValue(objectPos.player);
                object Y = ObjectPos.PlayerT.GetProperty("Y").GetValue(objectPos.player);
                GameObjectLocation location = new GameObjectLocation() // Получаем координаты игрока в данный момент
                {
                    X = Convert.ToInt32(X),
                    Y = Convert.ToInt32(Y)
                };
                for (int x = 0; x < objectPos.invaderMissle.Count; x++) // Проходим во всем существующим обьектам снарядов
                {
                    GameObject missle = objectPos.invaderMissle[x];
                    if (missle.GameObjectLocation.Y == GameSettings.WallStartY+28) // Проверка на уничстожение обьекта снаряда после того как он пройдет нижнюю стену
                    {
                        
                        semaphore.WaitOne();

                        objectPos.invaderMissle.RemoveAt(x); // Если true, то снаряд анигилируется

                        semaphore.Release();
                    }

                    missle.GameObjectLocation.Y++;
                    if (missle.GameObjectLocation.Equals(location))
                    {
                        if (GameSettings.PlayerLifes - GameSettings.MissleDamage < 1)
                        {
                            IsGameRunning = false;
                            break;
                        }
                        else
                        {
                            GameSettings.PlayerLifes -= GameSettings.MissleDamage;
                        }
                    }
                }
                Thread.Sleep(GameSettings.MissleSpeed);
            } while (IsGameRunning);

        }

        public void ChangeCanShootFlagToTrue(Object source, ElapsedEventArgs e)
        {
            CanShoot = true;
        }
    }
}
