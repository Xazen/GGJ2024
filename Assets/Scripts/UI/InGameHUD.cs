using System.Linq;
using DefaultNamespace;
using DG.Tweening;
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

        [SerializeField]
        private CanvasGroup scoreCanvas;

        [SerializeField]
        private GameObject[] playerContainers;

        private TimerService _timerService;
        private ScoreService _scoreService;
        private BalancingConfig _balancingConfig;

        [Inject]
        [UsedImplicitly]
        private void Inject(TimerService timerService, ScoreService scoreService, BalancingConfig balancingConfig)
        {
            _balancingConfig = balancingConfig;
            _scoreService = scoreService;
            _timerService = timerService;
        }

        private void Update()
        {
            UpdateTimer();
            UpdateScore();
        }

        private void UpdateTimer()
        {
            var timeSpan = _timerService.GetTime();
            timerCount.text = timeSpan.Minutes + ":" + timeSpan.Seconds.ToString("D2");

            if (timeSpan.TotalSeconds < _balancingConfig.HideScoreAtTotalSeconds)
            {
                scoreCanvas.DOFade(0, _balancingConfig.FadeScoreTime);
            }
        }

        private void UpdateScore()
        {
            var playerScoresByIndex = _scoreService.GetScoresByPlayerIndex().Values.ToArray();
            for (var i = 0; i < playerScores.Length; i++)
            {
                if (i < playerScoresByIndex.Length)
                {
                    playerScores[i].text = playerScoresByIndex[i].ToString();
                }
                else
                {
                    playerContainers[i].SetActive(false);
                    playerScores[i].gameObject.SetActive(false);
                }
            }
        }
    }
}