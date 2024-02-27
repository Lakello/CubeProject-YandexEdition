using System;
using UnityEngine;

namespace CubeProject.Save.Data
{
    [Serializable]
    public sealed class CurrentLevel : SimpleValueSave<int, CurrentLevel>
    {
        private const int DefaultValue = 0;
        
        public CurrentLevel() =>
            Init(DefaultValue);

        public CurrentLevel(int value) =>
            Init(value);
        
        public override CurrentLevel Clone() =>
            new CurrentLevel(Value);
        
        private void Init(int value) =>
            Value = Mathf.Clamp(value, 0, int.MaxValue);
    }
}