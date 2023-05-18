namespace BattleShipCore
{
    public class BattleshipsGame
    {
        private char[,] grid; // The grid representing the game board
        private List<Ship> ships; // The list of ships on the grid
        private Random random; // Random number generator for ship placement

        public BattleshipsGame()
        {
            grid = new char[10, 10];
            ships = new List<Ship>();
            random = new Random();
        }

        /// <summary>
        /// Initializes the game grid with empty cells
        /// </summary>
        public void InitializeGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = '-';
                }
            }
        }

        /// <summary>
        /// Randomly places the ships on the grid
        /// </summary>
        public void PlaceShips()
        {
            PlaceShip(5);
            PlaceShip(4);
            PlaceShip(4);
        }

        private void PlaceShip(int size)
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

        private bool CheckValidPlacement(int column, int row, int size, bool horizontal)
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

        /// <summary>
        /// Shoots at the specified target coordinates
        /// </summary>
        /// <param name="column">The column index of the target</param>
        /// <param name="row">The row index of the target</param>
        /// <returns>True if a ship is hit, false otherwise</returns>
        public bool ShootTarget(int column, int row)
        {
            if (column < 0 || column >= 10 || row < 0 || row >= 10)
                return false;

            if (grid[column, row] == 'S')
            {
                grid[column, row] = 'X';
                return true;
            }
            else if (grid[column, row] == '-')
            {
                grid[column, row] = 'O';
            }

            return false;
        }

        /// <summary>
        /// Checks if all ships have been sunk
        /// </summary>
        /// <returns>True if all ships are sunk, false otherwise</returns>
        public bool CheckAllShipsSunk()
        {
            foreach (char c in grid)
            {
                if (c == 'S')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a ship at the specified coordinates is destroyed
        /// </summary>
        /// <param name="column">The column index of the target</param>
        /// <param name="row">The row index of the target</param>
        /// <returns>True if the ship is destroyed, false otherwise</returns>
        public bool CheckShipDestroyed(int column, int row)
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

        /// <summary>
        /// Retrieves the cell value on the grid at the specified coordinates
        /// </summary>
        /// <param name="column">The column index of the cell</param>
        /// <param name="row">The row index of the cell</param>
        /// <returns>The cell value</returns>
        public char GetGridCellValue(int column, int row)
        {
            return grid[column, row];
        }

        /// <summary>
        /// Retrieves the list of ships on the grid
        /// </summary>
        /// <returns>The list of ships</returns>
        public List<Ship> GetShips()
        {
            return ships;
        }
    }

    public class Ship
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

        /// <summary>
        /// Checks if the ship is hit at the specified coordinates
        /// </summary>
        /// <param name="column">The column index of the hit</param>
        /// <param name="row">The row index of the hit</param>
        /// <returns>True if the ship is hit, false otherwise</returns>
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

        /// <summary>
        /// Checks if the ship is destroyed
        /// </summary>
        /// <returns>True if the ship is destroyed, false otherwise</returns>
        public bool IsDestroyed()
        {
            return hits == size;
        }
    }
}