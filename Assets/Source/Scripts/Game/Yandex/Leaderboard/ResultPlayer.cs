using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeProject.UI
{
    public class ResultPlayer : MonoBehaviour
    {
        public string Name { get; private set; }
        public int Score { get; private set; }
        public int Rank { get; private set; }

        public ResultPlayer(string name, int score, int rank)
        {
            Name = name;
            Score = score;
            Rank = rank;
        }
    }
}

