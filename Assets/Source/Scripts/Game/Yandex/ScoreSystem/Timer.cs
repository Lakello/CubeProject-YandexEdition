using System;
using UnityEngine;

namespace CubeProject.UI
{
    public class Timer : MonoBehaviour
    {
        private float _minutes;
        private float _seconds;
        private bool _isStopTimer = false;

        public event Action<float> TimeLeft;
        public event Action<float, float> VisualTime;

        public float CurrentTime { get; private set; } = 0;

        private void Update()
        {
            if (_isStopTimer == true)
                return;

            CurrentTime += Time.deltaTime;
            UpdateTimeText();

            if (CurrentTime >= 40)
            {
                StopTimer();
            }
        }

        private void UpdateTimeText()
        {
            _minutes = Mathf.FloorToInt(CurrentTime / 60);
            _seconds = Mathf.FloorToInt(CurrentTime % 60);
            VisualTime?.Invoke(_minutes, _seconds);
        }

        private void StopTimer()
        {
            _isStopTimer = true;
            TimeLeft?.Invoke(CurrentTime);
        }
    }
}
