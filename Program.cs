using Snake;

Random random = new Random();
Coord gridDimensions = new Coord(50, 20);

// Initial values
List<Coord> snake = new List<Coord> { new Coord(10, 1) };
Direction movementDirection = Direction.Right;
Coord applePos = GenerateApple();
int score = 0;
int frameDelayMilli = 100;
while (true)
{
    // Movement logic
    Coord head = new Coord(snake.Last().X, snake.Last().Y);
    head.ApplyMovementDirection(movementDirection);

    // Game Over if hit wall or self
    if (head.X == 0 || head.Y == 0 || head.X == gridDimensions.X - 1 || head.Y == gridDimensions.Y - 1 ||
        snake.Any(c => c.X == head.X && c.Y == head.Y))
    {
        Console.Clear();
        Console.WriteLine("Game Over! Final Score: " + score);
        Thread.Sleep(2000);
        // Reset game
        snake = new List<Coord> { new Coord(10, 1) };
        movementDirection = Direction.Right;
        applePos = GenerateApple();
        score = 0;
        continue;
    }

    // Add new head
    snake.Add(head);

    // Check if apple is eaten
    if (head.X == applePos.X && head.Y == applePos.Y)
    {
        score++;
        applePos = GenerateApple();
    }
    else
    {
        // Remove tail
        snake.RemoveAt(0);
    }

    // Draw
    Console.Clear();
    Console.WriteLine("Score: " + score);
    for (int y = 0; y < gridDimensions.Y; y++)
    {
        for (int x = 0; x < gridDimensions.X; x++)
        {
            if (x == 0 || y == 0 || x == gridDimensions.X - 1 || y == gridDimensions.Y - 1)
                Console.Write("#");
            else if (applePos.X == x && applePos.Y == y)
                Console.Write("a");
            else if (snake.Any(c => c.X == x && c.Y == y))
                Console.Write("■");
            else
                Console.Write(" ");
        }
        Console.WriteLine();
    }

    // Input handling with direction reversal prevention
    DateTime time = DateTime.Now;
    while ((DateTime.Now - time).Milliseconds < frameDelayMilli)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            Direction newDir = movementDirection;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newDir = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    newDir = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    newDir = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    newDir = Direction.Right;
                    break;
            }

            // Prevent snake from reversing directly
            if (!IsOppositeDirection(movementDirection, newDir))
                movementDirection = newDir;
        }
    }
}

// Helpers
Coord GenerateApple()
{
    Coord pos;
    do
    {
        pos = new Coord(random.Next(1, gridDimensions.X - 1), random.Next(1, gridDimensions.Y - 1));
    } while (snake.Any(c => c.X == pos.X && c.Y == pos.Y));
    return pos;
}

bool IsOppositeDirection(Direction current, Direction next)
{
    return (current == Direction.Up && next == Direction.Down) ||
           (current == Direction.Down && next == Direction.Up) ||
           (current == Direction.Left && next == Direction.Right) ||
           (current == Direction.Right && next == Direction.Left);
}
