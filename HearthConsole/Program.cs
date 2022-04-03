using System;

namespace HearthConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            int width = 10;
            int height = 10;

            // Отрисовываем карту
            var map = RenderMap(width, height);
            // Передаем координаты центра
            var player = new Player((int)(map.Width / 2), (int)(map.Height / 2));
            StartGame(player, map);
        }

        // Возвращает Map с реальным размером поля (+= 2)
        static Map RenderMap(int width, int height)
        {
            // Границы из звездочек
            width += 2;
            height += 2;
            for (int i = 0; i < height; i++)
            {
                if (i == 0 || i == height - 1)
                {
                    for (int j = 0; j < width; j++)
                        Console.Write("♥");
                }
                else
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("♥");
                    Console.SetCursorPosition(width - 1, i);
                    Console.Write("♥");
                    Console.SetCursorPosition(0, i);
                }
                Console.SetCursorPosition(0, i + 1);
            }

            return new Map(width, height);
        }

        static void StartGame(Player player, Map map)
        {
            // Ставим персонажа в центр
            Console.SetCursorPosition(player.X, player.Y);
            Console.Write('♥');
            Console.SetCursorPosition(0, player.Y + 4);
            ShowCoords(map.Height, player);

            while (true)
            {
                var pressedKey = Console.ReadKey(true).KeyChar;
                var moveResult = false;
                switch (pressedKey)
                {
                    case 'w':
                        moveResult = player.Move(MoveDirection.Up, map);
                        break;
                    case 'a':
                        moveResult = player.Move(MoveDirection.Left, map);
                        break;
                    case 's':
                        moveResult = player.Move(MoveDirection.Down, map);
                        break;
                    case 'd':
                        moveResult = player.Move(MoveDirection.Right, map);
                        break;
                    case (char)ConsoleKey.Escape:
                        ShowError(map.Height, "Thanks for game, good bye!");
                        return;
                    default:
                        continue;
                }
                if (moveResult)
                    ShowCoords(map.Width, player);
                else
                    ShowError(map.Height, "Error: Player cannot move out of map bounds");
            }
        }

        static void ShowError(int mapHeight, string error)
        {
            Console.SetCursorPosition(0, (int)mapHeight + 1);
            // Чистит всю строку в консоли
            Console.MoveBufferArea(0, (int)mapHeight + 1, Console.BufferWidth, 1, Console.BufferWidth, (int)mapHeight + 1, ' ', Console.ForegroundColor, Console.BackgroundColor);
            Console.Write($"{error}");
        }

        static void ShowCoords(int mapHeight, Player p)
        {
            Console.SetCursorPosition(0, (int)mapHeight + 1);
            // Чистит всю строку в консоли
            Console.MoveBufferArea(0, (int)mapHeight + 1, Console.BufferWidth, 1, Console.BufferWidth, (int)mapHeight + 1, ' ', Console.ForegroundColor, Console.BackgroundColor);
            Console.Write($"Player coords: X is {p.X} & Y is {p.Y}");
        }
    }

    class Map
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public int MovingWidth { get; set; }
        public int MovingHeight { get; set; }

        public Map(int w, int h)
        {
            Height = h;
            Width = w;

            MovingWidth = w - 2;
            MovingHeight = h - 2;
        }
    }

    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player(int x, int y)
        {
            X = x; Y = y;
        }

        public bool Move(MoveDirection direction, Map map)
        {
            int newX = X, newY = Y;

            switch (direction)
            {
                case MoveDirection.Left:
                    newX--;
                    break;
                case MoveDirection.Right:
                    newX++;
                    break;
                case MoveDirection.Up:
                    newY--;
                    break;
                case MoveDirection.Down:
                    newY++;
                    break;
            }

            if (newX > map.MovingWidth || newX <= 0 || newY > map.MovingHeight || newY <= 0)
                return false;

            Console.SetCursorPosition(X, Y);
            Console.Write(" ");

            X = newX;
            Y = newY;
            Console.SetCursorPosition(X, Y);
            Console.Write("♥");

            return true;
        }
    }
    enum MoveDirection
    {
        Left,
        Right,
        Up,
        Down
    }
}
