using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CubeProject.UI
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private Timer _timer;
        [SerializeField] private RecorderScore _recorderScore;

        private float _maxTime = 180;      
        private int _reward;
        private int _baseReward = 100;

        public int Reward => _reward;

        private void OnEnable()
        {
            _timer.TimeLeft += CalculateReward;         
        }

        private void OnDisable()
        {
            _timer.TimeLeft -= CalculateReward;
        }
               
        private void CalculateReward(float currentTime)
        {
            float tempTime = _maxTime / currentTime;

            if (tempTime <= 1)
            {
                _reward = 0;
            }
            else
            {
                _reward = Mathf.RoundToInt((tempTime / 100) * _baseReward);
            }
                       
            _recorderScore.TrySetScore(_reward);           
        }
    }
}
