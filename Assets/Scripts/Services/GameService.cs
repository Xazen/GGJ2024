using System;
using UnityEngine.InputSystem.Users;

namespace DefaultNamespace
{
    public class GameService
    {
        private ScoreService scoreService;
        private GamePlayerService gamePlayerService;
        private GamePlayerManagerController gamePlayerManagerController;
        private TimerService timerService;

        public Action OnRestart;

        public GameService(ScoreService scoreService, GamePlayerService gamePlayerService, GamePlayerManagerController gamePlayerManagerController,
            TimerService timerService)
        {
            this.timerService = timerService;
            this.gamePlayerManagerController = gamePlayerManagerController;
            this.gamePlayerService = gamePlayerService;
            this.scoreService = scoreService;
        }

        public void RegisterPlayer(InputUser inputUser)
        {
            scoreService.RegisterPlayer(inputUser.index);
            gamePlayerService.RegisterPlayer(inputUser);
        }

        public int GetPlayerCount()
        {
            return gamePlayerManagerController.GetPlayerCount();
        }

        public bool IsGameRunning()
        {
            return timerService.IsRunning();
        }

        public void RestartGame()
        {
            OnRestart?.Invoke();
        }
    }
}