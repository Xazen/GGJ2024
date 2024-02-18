using System.Collections.Generic;
using System.Linq;
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

        public Dictionary<int, int> GetScoresByPlayerIndex()
        {
            return scoreModel.ScoreByPlayerIndex;
        }

        public void ResetScores()
        {
            scoreModel.ScoreByPlayerIndex.Clear();
        }

        public List<int> GetWinningPlayerIndex()
        {
            var maxScore = scoreModel.ScoreByPlayerIndex.Values.Max();
            return scoreModel.ScoreByPlayerIndex.Where(x => x.Value == maxScore).Select(x => x.Key).ToList();
        }
    }
}