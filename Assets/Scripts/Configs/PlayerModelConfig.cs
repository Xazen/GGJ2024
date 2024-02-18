using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerModelConfig", menuName = "Configs/PlayerModelConfig")]
    public class PlayerModelConfig : ScriptableObject
    {
        public GameObject[] PlayerModelByIndex;
    }
}