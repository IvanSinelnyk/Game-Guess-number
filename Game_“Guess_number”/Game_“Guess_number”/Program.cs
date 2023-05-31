using Game_Guess_number;

string logFilePath = "log.txt";
using (StreamWriter writer = new StreamWriter(logFilePath))
{
    GuessNumberGame game = new GuessNumberGame(Console.ReadLine, Console.WriteLine, writer);

    game.Start();
}