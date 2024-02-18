using System.Collections.Generic;
using UnityEngine.InputSystem.Users;

namespace DefaultNamespace
{
    public class GamePlayerModel
    {
        public Dictionary<int, PlayerModel> PlayerStates = new ();
    }

    public class PlayerModel
    {
        public float CurrentStaggeredDuration;
        public bool IsStaggered => CurrentStaggeredDuration > 0;
        public bool IsAttacking;
        public bool IsScreaming;

        public InputUser InputUser;
    }
}