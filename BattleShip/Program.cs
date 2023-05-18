using System;
using System.Collections.Generic;

namespace Battleships
{
    class Program
    {
        static char[,] grid = new char[10, 10];
        static Random random = new Random();
        static List<Ship> ships = new List<Ship>();

        static void Main(string[] args)
        {
            InitializeGrid();
            PlaceShips();

            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Choose a game mode:");
            Console.WriteLine("1. Visible Ships");
            Console.WriteLine("2. Hidden Ships");

            int mode;
            while (true)
            {
                Console.Write("Enter the game mode (1 or 2): ");
                if (int.TryParse(Console.ReadLine(), out mode) && (mode == 1 || mode == 2))
                    break;
                else
                    Console.WriteLine("Invalid input. Try again.");
            }

            bool gameOver = false;
            int shots = 0;

            while (!gameOver)
            {
                Console.Clear();
                if (mode == 1)
                    DisplayGridWithShips();
                else
                    DisplayGrid();

                Console.Write("Enter target coordinates (e.g., A5): ");
                string input = Console.ReadLine().ToUpper();
                if (input.Length < 2)
                    continue;

                int column = input[0] - 'A';
                int row;
                if (!int.TryParse(input.Substring(1), out row))
                    continue;
                row--;

                if (column < 0 || column >= 10 || row < 0 || row >= 10)
                {
                    Console.WriteLine("Invalid coordinates. Try again.");
                    continue;
                }

                shots++;
                if (grid[column, row] == 'S')
                {
                    grid[column, row] = 'X';
                    Console.WriteLine("Hit!");

                    if (CheckAllShipsSunk())
                    {
                        gameOver = true;
                        Console.WriteLine($"You sank all the ships in {shots} shots. Congratulations!");
                    }
                    else if (CheckShipDestroyed(column, row))
                    {
                        Console.WriteLine("Ship destroyed!");
                    }
                }
                else if (grid[column, row] == '-')
                {
                    grid[column, row] = 'O';
                    Console.WriteLine("Miss!");
                }
                else
                {
                    Console.WriteLine("You've already shot at this location. Try again.");
                }

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            Console.WriteLine("Game over.");
            if (mode == 1)
                DisplayGridWithShips();
            else
                DisplayGrid();

            Console.ReadLine();
        }

        static void InitializeGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = '-';
                }
            }
        }

        static void PlaceShips()
        {
            // Place battleship (5 squares)
            PlaceShip(5);

            // Place destroyers (4 squares)
            PlaceShip(4);
            PlaceShip(4);
        }

        static void PlaceShip(int size)
        {
            bool placed = false;

            while (!placed)
            {
                int startColumn = random.Next(10);
                int startRow = random.Next(10);
                bool horizontal = random.Next(2) == 0;

                if (CheckValidPlacement(startColumn, startRow, size, horizontal))
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (horizontal)
                            grid[startColumn + i, startRow] = 'S';
                        else
                            grid[startColumn, startRow + i] = 'S';
                    }

                    ships.Add(new Ship(startColumn, startRow, size, horizontal));
                    placed = true;
                }
            }
        }

        static bool CheckValidPlacement(int column, int row, int size, bool horizontal)
        {
            if (horizontal && column + size > 10)
                return false;
            if (!horizontal && row + size > 10)
                return false;

            for (int i = 0; i < size; i++)
            {
                if (horizontal && grid[column + i, row] != '-')
                    return false;
                if (!horizontal && grid[column, row + i] != '-')
                    return false;
            }

            return true;
        }

        static bool CheckAllShipsSunk()
        {
            foreach (char c in grid)
            {
                if (c == 'S')
                    return false;
            }
            return true;
        }

        static bool CheckShipDestroyed(int column, int row)
        {
            foreach (Ship ship in ships)
            {
                if (ship.Hit(column, row))
                {
                    if (ship.IsDestroyed())
                        return true;
                }
            }

            return false;
        }

        static void DisplayGrid()
        {
            Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)('A' + i) + "  ");
                for (int j = 0; j < 10; j++)
                {
                    if (grid[i, j] == 'S')
                        Console.Write("- ");
                    else
                        Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void DisplayGridWithShips()
        {
            Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)('A' + i) + "  ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Ship
    {
        private int startColumn;
        private int startRow;
        private int size;
        private bool horizontal;
        private int hits;

        public Ship(int startColumn, int startRow, int size, bool horizontal)
        {
            this.startColumn = startColumn;
            this.startRow = startRow;
            this.size = size;
            this.horizontal = horizontal;
            hits = 0;
        }

        public bool Hit(int column, int row)
        {
            if (horizontal && row == startRow && column >= startColumn && column < startColumn + size)
            {
                hits++;
                return true;
            }
            else if (!horizontal && column == startColumn && row >= startRow && row < startRow + size)
            {
                hits++;
                return true;
            }

            return false;
        }

        public bool IsDestroyed()
        {
            return hits == size;
        }
    }
}
