using System;

namespace LeadTools.SaveSystem
{
    public interface ISaver
    {
        public TData Get<TData>()
            where TData : SaveData<TData>, new();

        public void Set<TData>(TData value) 
            where TData : SaveData<TData>, new();

        public void SubscribeValueUpdated<TData>(Action<TData> subAction)
            where TData : SaveData<TData>, new();
        
        public void UnsubscribeValueUpdated<TData>(Action<TData> unsubAction)
            where TData : SaveData<TData>, new();
    }
}