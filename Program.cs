using System;
using System.Collections.Generic;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayGame();
        }

        public static void PrintBoard(string[] board)
        {
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {board[0]}  |  {board[1]}  |  {board[2]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {board[3]}  |  {board[4]}  |  {board[5]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {board[6]}  |  {board[7]}  |  {board[8]}  ");
            Console.WriteLine("     |     |     ");

        }

        public static Dictionary<int, (int, string)> StartGame()
        {
            Dictionary<int, (int, string)> playerInfo = new Dictionary<int, (int, string)>();
            Random random = new Random();
            int randomPlayer, randomCoinFlip, firstPlayer = 0, secondPlayer = 0;
            string coinInput, signInput;
            bool valid1, valid2;
            string[] coin = { "H", "T" };
            Console.WriteLine("Welcome to Tic Tac Toe!");
            Console.Write("Press any key to play: ");
            Console.ReadKey();
            randomPlayer = random.Next(1, 3); 
            randomCoinFlip = random.Next(0, 2);
            Console.WriteLine("Flip a coin to see who goes first.");
            do
            {
                Console.Write($"Player{randomPlayer}, choose heads or tails (h/t): ");
                coinInput = Console.ReadLine();
                valid1 = coinInput.ToUpper().StartsWith("H") || coinInput.ToUpper().StartsWith("T") ? true : false;
                if (!valid1)
                    Console.WriteLine("Invalid input, try again.");
                else
                {
                    if (coinInput.ToUpper().Substring(0).Equals(coin[randomCoinFlip])) 
                    {
                        firstPlayer = randomPlayer;
                        secondPlayer = firstPlayer == 1 ? 2 : 1;
                    }
                    else
                    {
                        firstPlayer = randomPlayer == 1 ? 2 : 1;
                        secondPlayer = firstPlayer == 1 ? 2 : 1;
                    }
                    Console.WriteLine("Coin flipping...");
                    System.Threading.Thread.Sleep(2 * 1000);
                    Console.WriteLine("Coin was {0}", coin[randomCoinFlip] == "H" ? "heads." : "tails.");
                    Console.WriteLine($"Player{firstPlayer} won the coin flip and will have the first turn.");
                }
            }
            while (!valid1);
            do
            {
                Console.Write($"Player{firstPlayer}, choose a sign (x/o): ");
                signInput = Console.ReadLine();
                valid2 = signInput.ToUpper().Equals("X") || signInput.ToUpper().Equals("O") ? true : false;
                if (!valid2)
                    Console.WriteLine("Invalid input, try again.");
            }
            while (!valid2);
            if (signInput.ToUpper().Equals("X"))
            {
                playerInfo.Add(1, (firstPlayer, "X"));
                playerInfo.Add(2, (secondPlayer, "O"));
            }
            else
            {
                playerInfo.Add(1, (firstPlayer, "O"));
                playerInfo.Add(2, (secondPlayer, "X"));
            }
            Console.WriteLine("---------------------------");
            Console.WriteLine($"First player: player{firstPlayer} => {playerInfo[1].Item2}");
            Console.WriteLine($"Second player: player{secondPlayer} => {playerInfo[2].Item2}");
            Console.Write("Press any key to start the game: ");
            Console.ReadKey();
            Console.Clear();
            return playerInfo;
        }

        public static int GetUserInput(string[] board, int player)
        {
            string inputString;
            int inputInt;
            bool valid1, valid2, valid3;
            do
            {
                valid2 = true;
                valid3 = true;
                Console.Write($"Player{player}, choose a number (1-9): ");
                inputString = Console.ReadLine();
                valid1 = int.TryParse(inputString, out inputInt);
                if (!valid1)
                {
                    Console.WriteLine("Invalid input, try again.");
                    continue;
                }
                if (inputInt < 1 || inputInt > 9)
                {
                    Console.WriteLine("Invalid number, try again.");
                    valid2 = false;
                    continue;
                }
                if (board[inputInt - 1].Equals("X") || board[inputInt - 1].Equals("O"))
                {
                    Console.WriteLine("Space is already taken, try again.");
                    valid3 = false;
                }
            }
            while (!valid1 || !valid2 || !valid3);
            return inputInt;
        }

        public static (bool, bool) CheckWinOrStalemate(string[] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((board[0 + 3 * i].Equals(board[1 + 3 * i]) && board[1 + 3 * i].Equals(board[2 + 3 * i])
                    || (board[i].Equals(board[i + 3]) && board[i + 3].Equals(board[i + 6]))))
                return (true, false);
            }
            if ((board[0].Equals(board[4]) && board[4].Equals(board[8]))
                || (board[2].Equals(board[4]) && board[4].Equals(board[6])))
            return (true, false);
            foreach (string value in board)
            {
                if (!value.Equals("X") && !value.Equals("O"))
                    return (false, false);
            }
            return (true, true);
        }

        public static bool EndGame(int player, bool stale)
        {
            string input;
            bool valid;
            if (stale)
                Console.WriteLine("Stalemate. No winner!");
            else
                Console.WriteLine($"Player{player} won!");
            do
            {
                Console.Write("Play again (y/n)?: ");
                input = Console.ReadLine();
                valid = input.ToUpper().StartsWith("Y") || input.ToUpper().StartsWith("N") ? true : false;
                if (!valid)
                    Console.WriteLine("Invalid input, try again.");
            }
            while (!valid);
            if (input.ToUpper().StartsWith("Y"))
            {
                Console.Clear();
                return true;
            }
            else
            {
                Console.WriteLine("Thanks for playing!");
                return false;
            }
        }

        public static void PlayGame()
        {
            bool continueGame = false;
            int[] wins = { 0, 0 };
            do
            {
                Dictionary<int, (int, string)> playerInfo = StartGame();
                string[] board = {"1", "2", "3", "4", "5", "6", "7", "8", "9" };
                int playerTurn = 1;
                while(!CheckWinOrStalemate(board).Item1)
                {
                    PrintBoard(board);
                    int position = GetUserInput(board, playerInfo[playerTurn].Item1);
                    board[position - 1] = playerInfo[playerTurn].Item2;
                    PrintBoard(board);
                    if (!CheckWinOrStalemate(board).Item1)
                    {
                        if (playerTurn % 2 == 0)
                            playerTurn = 1;
                        else
                            playerTurn = 2;
                    }
                    Console.Clear();
                }
                PrintBoard(board);
                wins[playerInfo[playerTurn].Item1 - 1]++;
                int topPlayer = wins[0] > wins[1] ? 1 : 2;
                int lowPlayer = topPlayer == 1 ? 2 : 1;
                Console.WriteLine("----------------------");
                Console.WriteLine("|  Player#  |  Wins  |");
                Console.WriteLine("----------------------");
                Console.WriteLine($"|  Player{topPlayer}  |   {wins[topPlayer - 1]}    |");
                Console.WriteLine($"|  Player{lowPlayer}  |   {wins[lowPlayer - 1]}    |");
                Console.WriteLine("----------------------");
                continueGame = EndGame(playerInfo[playerTurn].Item1, CheckWinOrStalemate(board).Item2);
            }
            while (continueGame);
        }

    }
}
