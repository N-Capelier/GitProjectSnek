using UnityEngine;

namespace Saving
{
    /// <summary>
    /// Nico
    /// </summary>
    public class SaveManager : Singleton<SaveManager>
    {
        public SaveState state;

#if UNITY_EDITOR

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Reseting save");
                state = new SaveState();
                Save();
            }
        }

#endif
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
                //Switch when unstable
                state = Serializer.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
                //state = new SaveState();
            }
            else
            {
                Debug.Log("Creating save");
                state = new SaveState();
                Save(); //Deactivate when unstable
            }
        }
    }
}