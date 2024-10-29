using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Program
    {
        static List<Player> players = new List<Player>();
        static string filePath = "C:\\Users\\Nikita\\Desktop\\Учеба\\otladka po\\Игра с числами\\players.txt";

        static void Main(string[] args)
        {
            LoadPlayers(); // Загружаем данные игроков при запуске программы

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Войти в игру");
                Console.WriteLine("2. Зарегистрироваться");
                Console.WriteLine("3. Выход");

                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Write("Введите ваш никнейм: ");
                    string playerName = Console.ReadLine();

                    // Проверяем, зарегистрирован ли игрок
                    Player player = AuthenticatePlayer(playerName);
                    if (player == null)
                    {
                        Console.WriteLine("Игрок с таким никнеймом не найден. Попробуйте снова или зарегистрируйтесь.");
                    }
                    else
                    {
                        Console.WriteLine($"Добро пожаловать обратно, {player.Name}! Ваши текущие ходы: {player.Moves}");
                        int moves = PlayGame();
                        player.Moves += moves; // Обновляем количество ходов
                        SavePlayers(); // Сохраняем весь список игроков
                    }
                }
                else if (choice == "2")
                {
                    string playerName;
                    while (true)
                    {
                        Console.Write("Введите ваш никнейм для регистрации: ");
                        playerName = Console.ReadLine();

                        // Проверяем уникальность имени
                        if (IsPlayerNameUnique(playerName))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Имя уже занято. Пожалуйста, выберите другое.");
                        }
                    }

                    // Регистрируем нового игрока
                    Player player = new Player(playerName, 0); // Начальные ходы 0 для нового игрока
                    players.Add(player);
                    SavePlayers(); // Сохраняем нового игрока
                    Console.WriteLine($"Регистрация завершена! Добро пожаловать, {player.Name}!");
                }
                else if (choice == "3")
                {
                    break;
                }
            }
        }

        static bool IsPlayerNameUnique(string playerName)
        {
            // Проверяем, есть ли игрок с таким именем
            foreach (var player in players)
            {
                if (player.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                {
                    return false; // Имя не уникально
                }
            }
            return true; // Имя уникально
        }

        static Player AuthenticatePlayer(string playerName)
        {
            // Ищем игрока в списке по имени
            foreach (var player in players)
            {
                if (player.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                {
                    return player; // Если игрок найден, возвращаем его
                }
            }
            return null; // Если игрок не найден, возвращаем null
        }

        static void SavePlayers()
        {
            // Перезаписываем файл со всеми игроками
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach (var player in players)
                {
                    sw.WriteLine($"{player.Name},{player.Moves}");
                }
            }
        }

        static void LoadPlayers()
        {
            if (!File.Exists(filePath))
            {
                return; // Если файл не существует, выходим
            }

            // Загружаем данные из файла
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int moves))
                {
                    players.Add(new Player(parts[0], moves));
                }
            }
        }

        static int PlayGame()
        {
            int generate = GenerateUniqueDigitNumber();
            string x = generate.ToString();
            Console.WriteLine(x);
            int position = 0;
            int digit = 0;
            int moves = 0;

            while (true)
            {
                string ot;
                while (true)
                {
                    Console.Write("Введите 4-значное число: ");
                    ot = Console.ReadLine();
                    if (ot.Length == 4 && int.TryParse(ot, out _))
                        break;
                    Console.WriteLine("Ошибка: введите ровно 4 цифры.");
                }

                moves++;
                for (int i = 0; i < 4; i++)
                {
                    if (x[i] == ot[i])
                        position++;
                    if (x.Contains(ot[i]) && x[i] != ot[i])
                        digit++;
                }

                Console.WriteLine($"{position} на месте, всего {digit + position}.");

                if (position == 4)
                {
                    Console.WriteLine("Поздравляю! Вы угадали число!");
                    break;
                }

                position = 0;
                digit = 0;
            }

            Console.WriteLine($"Загаданное число: {x}");
            return moves;
        }

        static int GenerateUniqueDigitNumber()
        {
            Random rnd = new Random();
            string number = "";

            while (number.Length < 4)
            {
                int digit = rnd.Next(0, 10);
                if (!number.Contains(digit.ToString())) // Проверяем на уникальность
                {
                    number += digit;
                }
            }

            return int.Parse(number);
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Moves { get; set; }

        public Player(string name, int moves)
        {
            Name = name;
            Moves = moves;
        }
    }
}


