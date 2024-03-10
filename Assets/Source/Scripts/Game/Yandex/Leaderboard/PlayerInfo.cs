using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CubeProject.UI 
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void ShowInfo(ResultPlayer resultPlayer)
        {
            _rankText.text = resultPlayer.Rank.ToString();
            _nameText.text = resultPlayer.Name;
            _scoreText.text = resultPlayer.Score.ToString();
        }
    }
}

