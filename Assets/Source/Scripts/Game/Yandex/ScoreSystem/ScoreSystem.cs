using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeProject.UI
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private Timer _timer;

        private float _maxTime = 180;      
        private int _reward;
        private int _baseReward = 100;

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

            Debug.Log($"Полученная награда = {_reward}");
        }
    }
}
