using BattleShipCore;

// new game object instantiation 
BattleshipsGame game = new ();
game.InitializeGrid(); // initalize new grid-map
game.PlaceShips(); //place predefined quantity of ships

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

// continue to play while game will not be finished 
while (!gameOver)
{
    Console.Clear();
    if (mode == 1)
        DisplayGridWithShips(game);
    else
        DisplayGrid(game);

    Console.Write("Enter target coordinates (e.g., A5): ");
    string? rawInput = Console.ReadLine();
    string input = rawInput is null? "" : rawInput.ToUpper();
    if (input.Length < 2)
        continue;

    int column = input[0] - 'A'; //calculate column number from letter input
    int row;
    if (!int.TryParse(input.AsSpan(1), out row))
        continue;
    row--; //row index starts from 0, input starts from 1

    if (column < 0 || column >= 10 || row < 0 || row >= 10)
    {
        Console.WriteLine("Invalid coordinates. Try again.");
        continue;
    }

    shots++;

    if (game.ShootTarget(column, row))
    {
        Console.WriteLine("Hit!");

        if (game.CheckAllShipsSunk())
        {
            gameOver = true;
            Console.WriteLine($"You sank all the ships in {shots} shots. Congratulations!");
        }
        else if (game.CheckShipDestroyed(column, row))
        {
            Console.WriteLine("Ship destroyed!");
        }
    }
    else if (game.GetGridCellValue(column, row) == '-')
    {
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
    DisplayGridWithShips(game);
else
    DisplayGrid(game);

Console.ReadLine();

void DisplayGrid(BattleshipsGame game)
{
    Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
    for (int i = 0; i < 10; i++)
    {
        Console.Write((char)('A' + i) + "  ");
        for (int j = 0; j < 10; j++)
        {
            if (game.GetGridCellValue(i, j) == 'S')
                Console.Write("- ");
            else
                Console.Write(game.GetGridCellValue(i, j) + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

void DisplayGridWithShips(BattleshipsGame game)
{
    Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
    for (int i = 0; i < 10; i++)
    {
        Console.Write((char)('A' + i) + "  ");
        for (int j = 0; j < 10; j++)
        {
            Console.Write(game.GetGridCellValue(i, j) + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
