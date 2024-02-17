using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimerService : MonoBehaviour
    {
        public Action OnTimerEnd;

        private TimeSpan gameDuration;
        private bool isRunning;

        public void StartTimer(float gameDuration)
        {
            this.gameDuration = TimeSpan.FromSeconds(gameDuration);
            isRunning = true;
        }

        private void Update()
        {
            if (!isRunning) return;

            gameDuration -= TimeSpan.FromSeconds(Time.deltaTime);
            if (gameDuration.TotalMilliseconds <= 0)
            {
                isRunning = false;
                OnTimerEnd?.Invoke();
            }
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public TimeSpan GetTime()
        {
            return gameDuration;
        }
    }
}