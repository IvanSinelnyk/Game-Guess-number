using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Game_Guess_number
{
    public class GuessNumberGame
    {
        private readonly Func<string?> _inputProvider;
        private readonly Action<string> _outputProvider;
        private readonly TextWriter _logWriter;
        private readonly int _number;

        public GuessNumberGame(Func<string?> inputProvider, Action<string> outputProvider, TextWriter logWriter)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
            _logWriter = logWriter;
            Random rnd = new();
            _number = rnd.Next(0, 101);
        }

        public void Start(int number = -1)
        {
            if (number == -1)
            {
                number = _number;
            }
            using IHost host = Host.CreateDefaultBuilder().Build();

            // Ask the service provider for the configuration abstraction.
            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

            // Get values from the config given their key and their target type.
            string? equalValue = config.GetValue<string>("Equal") ?? string.Empty;
            string? greaterValue = config.GetValue<string>("Greater") ?? string.Empty;
            string? lessValue = config.GetValue<string>("Less") ?? string.Empty;
            string? askValue = config.GetValue<string>("GameRuleQuestion") ?? string.Empty;
            string? brokeRuleValue = config.GetValue<string>("BrokeRule") ?? string.Empty;

            do
            {
                _outputProvider(askValue);
                WriteLog(askValue);
                var choice = _inputProvider() ?? string.Empty;
                if (!int.TryParse(choice, out int result) || result < 0 || result > 100)
                {
                    _outputProvider(brokeRuleValue);
                    WriteLog($"{result} \n {brokeRuleValue}");
                }
                else
                {
                    if (result == number)
                    {
                        _outputProvider(equalValue);
                        WriteLog($"{result} \n {equalValue}");
                        return;
                    }
                    else if (result > number)
                    {
                        _outputProvider(greaterValue);
                        WriteLog($"{result} \n {greaterValue}");
                    }
                    else
                    {
                        _outputProvider(lessValue);
                        WriteLog($"{result} \n {lessValue}");
                    }                    
                }
            } while (true);
        }

        private void WriteLog(string message)
        {
            _logWriter.WriteLine(message);
        }
    }
}
