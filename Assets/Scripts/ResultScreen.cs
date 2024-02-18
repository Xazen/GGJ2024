using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Constants;
using DefaultNamespace;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class ResultScreen : UIMonoBehaviour
{
    private const float Delay = 2f;

    [SerializeField]
    private TextMeshProUGUI[] playerScores;
    [SerializeField]
    private Image[] playerPortraits;

    [SerializeField]
    private TextMeshProUGUI headerText;
    [SerializeField]
    private GameObject restartText;

    [SerializeField]
    private GameObject[] playerContainers;

    private ScoreService _scoreService;
    private GameService _gameService;

    private bool canRestart;

    [Inject]
    [UsedImplicitly]
    private void Inject(ScoreService scoreService, GameService gameService)
    {
        _gameService = gameService;
        _scoreService = scoreService;
    }

    private void Start()
    {
        headerText.text = "And the best cook is...";

        foreach (var score in playerScores)
        {
            score.gameObject.SetActive(false);
        }
        foreach (var score in playerPortraits)
        {
            score.gameObject.SetActive(false);
        }
        restartText.gameObject.SetActive(false);

        foreach (var playerInput in PlayerInput.all)
        {
            playerInput.SwitchCurrentActionMap(ActionMaps.UI);
        }

        StartCoroutine(ShowResult());

    }

    private IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(Delay);
        ShowScore();
        canRestart = true;

    }

    private void Update()
    {
        if (canRestart && Input.anyKeyDown)
        {
            _gameService.RestartGame();
        }
    }

    private void ShowScore()
    {
        headerText.text = "Congrats!!!";
        restartText.gameObject.SetActive(true);

        List<int> winningIndices = _scoreService.GetWinningPlayerIndex();

        var playerScoresByIndex = _scoreService.GetScoresByPlayerIndex().Values.ToArray();
        for (var i = 0; i < playerScores.Length; i++)
        {
            var textMeshProUGUI = playerScores[i];
            if (winningIndices.Contains(i))
            {
                playerScores[i].transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                playerPortraits[i].transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                textMeshProUGUI.color = Color.green;
            }

            if (i < playerScoresByIndex.Length)
            {
                playerScores[i].gameObject.SetActive(true);
                playerPortraits[i].gameObject.SetActive(true);
                textMeshProUGUI.text = playerScoresByIndex[i].ToString();
            }
            else
            {
                playerContainers[i].SetActive(false);
                textMeshProUGUI.gameObject.SetActive(false);
            }
        }
    }
}