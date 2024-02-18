using System.Linq;
using Constants;
using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class ResultScreen : UIMonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] playerScores;

    [SerializeField]
    private GameObject[] playerContainers;

    [SerializeField]
    private Button restartButton;

    private ScoreService _scoreService;
    private GameService _gameService;

    [Inject]
    [UsedImplicitly]
    private void Inject(ScoreService scoreService, GameService gameService)
    {
        _gameService = gameService;
        _scoreService = scoreService;
    }

    private void Start()
    {
        foreach (var playerInput in PlayerInput.all)
        {
            playerInput.SwitchCurrentActionMap(ActionMaps.UI);
        }
        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        restartButton.onClick.AddListener(OnRestart);
        UpdateScore();
    }

    private void OnRestart()
    {
        _gameService.RestartGame();
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