using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreService
    {
        private ScoreModel scoreModel;

        public ScoreService(ScoreModel scoreModel)
        {
            this.scoreModel = scoreModel;
        }

        public void RegisterPlayer(int userIndex)
        {
            scoreModel.ScoreByPlayerIndex.TryAdd(userIndex, 0);
            Debug.Log("Player " + userIndex + " registered");
        }

        public void AddScore(int inputUserIndex, int score)
        {
            scoreModel.ScoreByPlayerIndex[inputUserIndex] += score;
            Debug.Log("Player " + inputUserIndex + " score: " + scoreModel.ScoreByPlayerIndex[inputUserIndex]);
        }
    }
}