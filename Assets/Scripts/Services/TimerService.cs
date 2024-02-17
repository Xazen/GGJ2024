using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimerService : MonoBehaviour
    {
        public Action OnTimerEnd;

        private float gameDuration;
        private bool isRunning;

        public void StartTimer(float gameDuration)
        {
            this.gameDuration = gameDuration;
            isRunning = true;
        }

        private void Update()
        {
            if (!isRunning) return;
            gameDuration -= Time.deltaTime;
            if (gameDuration <= 0)
            {
                isRunning = false;
                OnTimerEnd?.Invoke();
            }
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}