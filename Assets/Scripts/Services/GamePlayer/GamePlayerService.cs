using System.Collections.Generic;
using UnityEngine.InputSystem.Users;

namespace DefaultNamespace
{
    public class GamePlayerService
    {
        private readonly GamePlayerModel gamePlayerModel;

        public GamePlayerService(GamePlayerModel gamePlayerModel)
        {
            this.gamePlayerModel = gamePlayerModel;
        }

        public void RegisterPlayer(InputUser inputUser)
        {
            var playerState = new PlayerModel
            {
                InputUser = inputUser
            };
            gamePlayerModel.PlayerStates[inputUser.index] = playerState;
        }

        public PlayerModel GetPlayerModel(int userIndex)
        {
            return gamePlayerModel.PlayerStates.GetValueOrDefault(userIndex);
        }
    }
}