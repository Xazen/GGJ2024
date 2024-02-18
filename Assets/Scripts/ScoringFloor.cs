using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class ScoringFloor : MonoBehaviour
{
    private ScoreService _scoreService;

    [Inject]
    [UsedImplicitly]
    public void Inject(ScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Stuffing))
        {
            var stuffing = other.gameObject.GetComponent<Stuffing>();
            _scoreService.AddScore(stuffing.PlayerIndex, 1);
            Debug.Log("Player " + stuffing.PlayerIndex + " scored!");
        }
    }
}
