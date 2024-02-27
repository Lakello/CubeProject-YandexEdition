using System;

namespace LeadTools.SaveSystem
{
    [Serializable]
    public abstract class SaveData<TData>
        where TData : SaveData<TData>
    {
        public event Action<TData> ValueUpdated;

        public void TryUpdateValue(TData value, Action successCallback)
        {
            if (Equals(value))
            {
                return;
            }

            UpdateValue(value);
            
            successCallback();
            ValueUpdated?.Invoke(Clone());
        }

        public abstract TData Clone();

        protected abstract void UpdateValue(TData data);

        protected abstract bool Equals(TData data);
    }
}