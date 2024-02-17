using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private TimerService timerService;
    private BalancingConfig balancingConfig;

    [Inject]
    [UsedImplicitly]
    public void Inject(TimerService timerService, BalancingConfig balancingConfig)
    {
        this.balancingConfig = balancingConfig;
        this.timerService = timerService;
    }

    void Start()
    {
        timerService.OnTimerEnd += OnTimerEnd;
        timerService.StartTimer(balancingConfig.GameDuration);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void OnTimerEnd()
    {
        Debug.Log("Game Over");
    }
}
