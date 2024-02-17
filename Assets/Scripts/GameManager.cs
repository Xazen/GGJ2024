using System;
using Constants;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour, IInitializable, IDisposable
{
    private TimerService timerService;
    private BalancingConfig balancingConfig;
    private GamePlayerManagerController gamePlayerManagerController;
    private GameService gameService;
    private DiContainer diContainer;

    [Inject]
    [UsedImplicitly]
    public void Inject(TimerService timerService, BalancingConfig balancingConfig, GamePlayerManagerController gamePlayerManagerController,
        GameService gameService, DiContainer diContainer)
    {
        this.diContainer = diContainer;
        this.gameService = gameService;
        this.gamePlayerManagerController = gamePlayerManagerController;
        this.balancingConfig = balancingConfig;
        this.timerService = timerService;
    }

    private void Start()
    {
        LoadLevel((scene, _) =>
        {
            foreach (var environmentGameObject in scene.GetRootGameObjects())
            {
                diContainer.InjectGameObject(environmentGameObject);
            }
            EnablePlayerManager();
            StartGame();
        });
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        gameService.RegisterPlayer(playerInput.user);
    }

    private void EnablePlayerManager()
    {
        gamePlayerManagerController.AllowJoining();
    }

    private void StartGame()
    {
        timerService.OnTimerEnd += OnTimerEnd;
        timerService.StartTimer(balancingConfig.GameDuration);
    }

    private void LoadLevel(UnityAction<Scene,LoadSceneMode> OnSceneLoaded)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneBuildIndex.GameEnvironment, LoadSceneMode.Additive);
    }

    private void OnTimerEnd()
    {
        Debug.Log("Game Over");
    }

    public void Initialize()
    {
        gamePlayerManagerController.PlayerJoined += OnPlayerJoined;
    }

    public void Dispose()
    {
        gamePlayerManagerController.PlayerJoined -= OnPlayerJoined;
    }
}
