using System;
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
    private Button restartButton;

    private ScoreService _scoreService;

    [Inject]
    [UsedImplicitly]
    private void Inject(ScoreService scoreService)
    {
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
                playerScores[i].gameObject.SetActive(false);
            }
        }
    }
}