using UnityEngine.InputSystem.Users;

namespace DefaultNamespace
{
    public class GameService
    {
        private ScoreService scoreService;
        private GamePlayerService gamePlayerService;

        public GameService(ScoreService scoreService, GamePlayerService gamePlayerService)
        {
            this.gamePlayerService = gamePlayerService;
            this.scoreService = scoreService;
        }

        public void RegisterPlayer(InputUser inputUser)
        {
            scoreService.RegisterPlayer(inputUser.index);
            gamePlayerService.RegisterPlayer(inputUser);
        }

    }
}