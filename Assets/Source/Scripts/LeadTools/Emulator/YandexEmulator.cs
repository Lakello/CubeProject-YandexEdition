using System;
using System.IO;

namespace LeadTools.Emulator
{
    public class YandexEmulator
    {
        private readonly string _saveSimPath = "Assets/Source/Scripts/LeadTools/Emulator/SaveSim.json";

        public void Init(Action<string> action)
        {
            string data = File.ReadAllText(_saveSimPath);

            action?.Invoke(data);
        }

        public void Save(string save)
        {
            File.WriteAllText(_saveSimPath, save);
        }
    }
}