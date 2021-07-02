using UnityEngine;

namespace Saving
{
    /// <summary>
    /// Nico
    /// </summary>
    public class SaveManager : Singleton<SaveManager>
    {
        public SaveState state;
        public static bool loaded = false;

        private void Awake()
        {
            CreateSingleton(true);
            if(!loaded)
            {
                loaded = true;
                Load();
                print("Loaded save");
            }
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