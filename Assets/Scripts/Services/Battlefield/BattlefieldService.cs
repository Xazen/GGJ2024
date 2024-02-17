using UnityEngine;

namespace DefaultNamespace
{
    public class BattlefieldService
    {
        private readonly BattlefieldModel _battlefieldModel;

        public BattlefieldService(BattlefieldModel battlefieldModel)
        {
            this._battlefieldModel = battlefieldModel;
        }

        public void SetSpawnPoints(Transform[] spawnPoints)
        {
            _battlefieldModel.SpawnPositions.Clear();
            foreach (var spawnPoint in spawnPoints)
            {
                _battlefieldModel.SpawnPositions.Add(new SpawnLocation
                {
                    Position = spawnPoint.position
                });
            }
        }
    }
}