using CubeProject.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeViewer : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private TextMeshProUGUI _viewText;   

    private void OnEnable()
    {
        _timer.VisualTime += SetTime;
    }

    private void OnDisable()
    {
        _timer.VisualTime -= SetTime;
    }

    private void SetTime(float minutes, float seconds)
    {
       _viewText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
