using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConsoleSnake
{
    class Program
    {
        char[,] Map = new char[20, 20];
        int Up = 0;
        int Down = 0;
        int Left = 0;
        int Right = 1;
        int X = 4;
        int Y = 4;
        char Apple = '@';
        char MapChar = '.';
        int AppleX = 1;
        int AppleY = 1;
        bool isRunning = true;
        List<int[]> Snake = new List<int[]>();
        
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Program program = new Program();
            Thread RenderThred = new Thread(program.Render);
            Thread InputThread = new Thread(program.Input);
            program.SetUpSnake();
            RenderThred.Start();
            InputThread.Start();
        }
        void Render()
        {
            while (isRunning)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                FillMap();                             
                Map[AppleX, AppleY] = '@';
                SnakeCollision();
                Collision();              
                DeterminatePlayerPos();
                Boundary();
                if (isRunning == false)
                {
                    break;
                }               
                SnakeMoving();
                SnakeRender();               
                for (int n = 0; n < Map.GetLength(0); n++)
                {
                    for (int m = 0; m < Map.GetLength(1); m++)
                    {
                        
                        Console.Write(Map[n, m] + " ");
                        
                    }
                    Console.Write("\n");
                }              
                Thread.Sleep(500);
            }
        }
        void FillMap()
        {
            for (int n = 0; n < Map.GetLength(0); n++)
            {
                for (int m = 0; m < Map.GetLength(1); m++)
                {
                    if (m == 0)
                    {
                        Map[n, m] = '█';
                    }
                    else if (m == Map.GetLength(0) - 1)
                    {
                        Map[n, m] = '█';
                    }
                    else if (n == 0)
                    {
                        Map[n, m] = '█';
                    }
                    else if (n == Map.GetLength(0) - 1)
                    {
                        Map[n, m] = '█';
                    }
                    else
                    {
                        Map[n, m] = MapChar;
                    }                   
                }
            }
        }
        void SetUpSnake()
        {
            int[] Head = new int[2] { X, Y };
            int[] Torso = new int[2] {X, Y + 1};
            int[] Back = new int[2] {X , Y + 2};
            Snake.Add(Head);
            Snake.Add(Torso);
            Snake.Add(Back);
        }

        
        void DeterminatePlayerPos()
        {
            if (Up == 1)
            {
                X--;
                
            }
            else if (Down == 1)
            {
                X++;
            }
            else if (Left == 1)
            {
                Y--;
            }
            else if (Right == 1)
            {
                Y++;
            }
            Map[X, Y] = '#';
        }

        void Boundary()
        {
            if (X == Map.GetLength(0) - 1)
            {
                isRunning = false;              
            }
            else if (X == 0)
            {
                isRunning = false;
            }
            else if (Y == Map.GetLength(1) - 1)
            {
                isRunning = false;
            }
            else if (Y == 0)
            {
                isRunning = false;
            }

        }

        void Collision()
        {
            Random RNG = new Random();
            
            if (Map[X, Y] == Apple)
            {
                Map[AppleX, AppleY] = MapChar;
                AppleX = RNG.Next(1, Map.GetLength(0) - 1);
                AppleY = RNG.Next(1, Map.GetLength(1) - 1);
                int[] Pos = new int[2] { X, Y};
                int[] PosToAdd = new int[2] { X, Y};
                Snake.Add(PosToAdd);                         
                for (int n = Snake.Count - 2; n >= 0; n--)
                {
                    Snake[n + 1] = Snake[n];                  
                }
                Snake[0] = Pos;
            }                       
            
        }

        void Debugging()
        {
            string Data = "";
            for (int n = 0; n < Snake.Count; n++)
            {              
                Data += (Snake[n][0] + " " + Snake[n][1] + "\n");                          
            }
            System.IO.File.WriteAllText(@"C:\Users\Németh\Desktop\DebugSnake.txt", Data);
        } 

        void SnakeCollision()
        {
            for (int n = 3; n < Snake.Count; n++)
            {
                if (Snake[0][0] == Snake[n][0] && Snake[0][1] == Snake[n][1])
                {
                    Debugging();
                    isRunning = false;
                }              
            }
        }

        void SnakeRender()
        {
            for (int n = 0; n < Snake.Count; n++)
            {
                Map[Snake[n][0], Snake[n][1]] = '#';             
            }
        }

        void SnakeMoving()
        {          
                  
            for (int n = Snake.Count - 2; n >= 0; n--) // Snake.Count - 2 is the right
            {
                Snake[n + 1] = Snake[n];
            }
            int[] Pos = new int[2] { X, Y };
            Snake[0] = Pos;

        }
   
 
        void Input()
        {
            while (isRunning)
            {
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.W)
                {
                    Up = 1;
                    Down = 0;
                    Left = 0;
                    Right = 0;
                }
                else if (key == ConsoleKey.S)
                {
                    Up = 0;
                    Down = 1;
                    Left = 0;
                    Right = 0;
                    
                }
                else if (key == ConsoleKey.A)
                {
                    Up = 0;
                    Down = 0;
                    Left = 1;
                    Right = 0;
                }
                else if (key == ConsoleKey.D)
                {
                    Up = 0;
                    Down = 0;
                    Left = 0;
                    Right = 1;
                }
            }
        }
    }
}
