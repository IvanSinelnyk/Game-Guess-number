using Game_Guess_number;
using static Game_Guess_number.GuessNumberGame;

GuessNumberGame game = new(Console.ReadLine, Console.WriteLine);

ResultComparing result;
do
{
    game.AskUserNumber();

    result = game.CompareNumbers();
    game.Messanger(result);
}
while (result != ResultComparing.Equal);