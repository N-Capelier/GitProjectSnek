using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public class SaveManager : Singleton<SaveManager>
    {
        public SaveState state;

        private void Awake()
        {
            CreateSingleton(true);
            Load();
        }

        public void Save()
        {
            PlayerPrefs.SetString("save", Serializer.Serialize(state));
        }

        public void Load()
        {
            if(PlayerPrefs.HasKey("save"))
            {
                state = Serializer.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
            }
            else
            {
                Debug.Log("Creating save");
                state = new SaveState();
                Save();
            }
        }
    }
}