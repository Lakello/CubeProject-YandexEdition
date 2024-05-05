using System;
using UnityEngine;

namespace CubeProject.UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private EndLevelResult _levelResult;

        private float _minutes;
        private float _seconds;      

        public event Action<float> TimeLeft;
        public event Action<float, float> VisualTime;

        public float CurrentTime { get; private set; } = 0;

        private void OnEnable()
        {
            _levelResult.LevelCompleted += StopTimer;            
        }

        private void OnDisable()
        {
            _levelResult.LevelCompleted -= StopTimer;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;
            UpdateTimeText();
        }

        private void UpdateTimeText()
        {
            _minutes = Mathf.FloorToInt(CurrentTime / 60);
            _seconds = Mathf.FloorToInt(CurrentTime % 60);
            VisualTime?.Invoke(_minutes, _seconds);
        }

        private void StopTimer()
        {           
            TimeLeft?.Invoke(CurrentTime);
        }
    }
}

