using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Game__Guess_number_
{
    public class GuessNumberGame
    {
        private readonly Func<string?> _inputProvider;
        private readonly Action<string> _outputProvider;
        private readonly int _number;

        public GuessNumberGame(Func<string?> inputProvider, Action<string> outputProvider)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
            Random rnd = new();
            _number = rnd.Next(0, 101);
        }

        public void Start()
        {
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
                var choice = _inputProvider() ?? string.Empty;
                if (!int.TryParse(choice, out int result) || result < 0 || result > 100)
                {
                    _outputProvider(brokeRuleValue);
                }
                else
                {
                    if (result == _number)
                    {
                        _outputProvider(equalValue);
                        return;
                    }
                    if (result > _number)
                    {
                        _outputProvider(greaterValue);
                    }
                    if (result < _number)
                    {
                        _outputProvider(lessValue);
                    }                    
                }
            } while (true);
        }
    }
}
