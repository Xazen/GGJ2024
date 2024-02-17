using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class InGameHUD : UIMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timerCount;

        [SerializeField]
        private TextMeshProUGUI[] playerScores;

        private TimerService _timerService;
        private ScoreService _scoreService;

        [Inject]
        [UsedImplicitly]
        private void Inject(TimerService timerService, ScoreService scoreService)
        {
            _scoreService = scoreService;
            _timerService = timerService;
        }

        private void Update()
        {
            var timeSpan = _timerService.GetTime();
            timerCount.text = timeSpan.Minutes + ":" + timeSpan.Seconds;
            var playerScoresByIndex = _scoreService.GetScoresByPlayerIndex().Values.ToArray();
            for (var i = 0; i < playerScores.Length; i++)
            {
                if (i < playerScoresByIndex.Length)
                {
                    playerScores[i].text = playerScoresByIndex[i].ToString();
                }
                else
                {
                    playerScores[i].gameObject.SetActive(false);
                }
            }
        }
    }
}