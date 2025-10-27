using CardMatch.Services.Base;
using CardMatch.UI.Screens;

namespace CardMatch.Services
{
    public class ScoreService : ServiceBase
    {
        private int _currentScore;
        private int _currentTurns;
        private GameplayHudScreen _hudScreen;

        internal void ResetCurrentScore()
        {
            _currentScore = 0;
            _currentTurns = 0;
        }

        internal void UpdateCurrentScoreAndTurns(int matches,int turns)
        {
            _currentScore = matches;
            _currentTurns = turns;
        }

        internal void IncrementCurrentScore(int scoreStep)
        {
            if (!_hudScreen)
                _hudScreen = Bootstrap.GetService<UserInterfaceService>()
                    .CurrentInterface.GetScreen<GameplayHudScreen>();
            _currentScore += scoreStep;
            _hudScreen.UpdateCurrentScore(_currentScore);
        }

        internal void IncrementCurrentTurns(int turnStep)
        {
            if (!_hudScreen)
                _hudScreen = Bootstrap.GetService<UserInterfaceService>()
                    .CurrentInterface.GetScreen<GameplayHudScreen>();
            _currentTurns += turnStep;
            _hudScreen.UpdateCurrentTurns(_currentTurns);
        }

        protected override void RegisterService() =>
            Bootstrap.RegisterService(this);
    }
}