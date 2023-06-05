using Game_Guess_number;
using Moq;
using static Game_Guess_number.GuessNumberGame;

namespace GuessNumberGameTests
{
    [TestClass]
    public class GameTests
    {
        [DataTestMethod]
        [DataRow(1, 1, ResultComparing.Equal)]
        [DataRow(1, 2, ResultComparing.Higher)]
        [DataRow(3, 2, ResultComparing.Lower)]
        [DataRow(50, 50, ResultComparing.Equal)]
        [DataRow(53, 82, ResultComparing.Higher)]
        [DataRow(99, 82, ResultComparing.Lower)]
        public void TestCompareNumbers(int compNum, int userNum, ResultComparing expected)
        {
            // Create mock input provider
            var mockInputProvider = new Mock<Func<string?>>(MockBehavior.Loose);
            mockInputProvider.Setup(x => x.Invoke()).Returns(userNum.ToString());

            // Create mock output provider
            var mockOutputProvider = new Mock<Action<string>>(MockBehavior.Loose);
            mockOutputProvider.Setup(x => x.Invoke(It.IsAny<string>()));

            // Create a mock object of GuessNumberGame with base class constructor
            var mock = new Mock<GuessNumberGame>(mockInputProvider.Object, mockOutputProvider.Object)
            {
                CallBase = true
            };

            // Setup the mock to override the necessary methods
            mock.Setup(m => m.GetCompNumber()).Returns(compNum);
            mock.Setup(m => m.GetUserNumber()).Returns(userNum);

            // Use the mock object in the test
            GuessNumberGame game = mock.Object;
            ResultComparing result = game.CompareNumbers();

            // Assert the result
            Assert.AreEqual(expected, result);
        }

    }
}