using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Game_Guess_number
{
    public class GuessNumberGame
    {
        public enum ResultComparing
        {
            Lower,
            Higher,
            Equal,
            Destroyer,
            NotCompaired
        }


        private readonly Func<string?> _inputProvider;
        private readonly Action<string> _outputProvider;
        private readonly int _number;
        private int _userGuess;

        public GuessNumberGame(Func<string?> inputProvider, Action<string> outputProvider)
        {
            _outputProvider = outputProvider;
            _inputProvider = inputProvider;
            Random rnd = new();
            _number = rnd.Next(0, 101);
        }

        public virtual int GetCompNumber()
        {
            return _number;
        }

        public virtual int GetUserNumber()
        {
            return _userGuess;
        }

        public void AskUserNumber()
        {
            Messanger(ResultComparing.NotCompaired);
            var choice = _inputProvider() ?? string.Empty;
            if (!int.TryParse(choice, out int result) || result < 0 || result > 100)
            {
                Messanger(ResultComparing.Destroyer);
                AskUserNumber();
            }
            _userGuess = result;
        }

        public ResultComparing CompareNumbers()
        {
            int userNum = GetUserNumber();
            int compNum = GetCompNumber();
            if (userNum == compNum)
            {
                return ResultComparing.Equal;
            }
            else if (userNum > compNum)
            {
                return ResultComparing.Higher;
            }
            return ResultComparing.Lower;
        }

        public void Messanger(ResultComparing comparator)
        {
            using IHost host = Host.CreateDefaultBuilder().Build();
            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
            string? askValue = config.GetValue<string>("GameRuleQuestion") ?? string.Empty;
            string? brokeRuleValue = config.GetValue<string>("BrokeRule") ?? string.Empty;
            string? equalValue = config.GetValue<string>("Equal") ?? string.Empty;
            string? greaterValue = config.GetValue<string>("Greater") ?? string.Empty;
            string? lessValue = config.GetValue<string>("Less") ?? string.Empty;
            switch (comparator)
            {
                case ResultComparing.Lower:
                    _outputProvider(lessValue);
                    break;
                case ResultComparing.Higher:
                    _outputProvider(greaterValue);
                    break;
                case ResultComparing.Destroyer:
                    _outputProvider(brokeRuleValue);
                    break;
                case ResultComparing.NotCompaired:
                    _outputProvider(askValue);
                    break;
                default:
                    _outputProvider(equalValue);
                    break;
            }
        }

    }
}
