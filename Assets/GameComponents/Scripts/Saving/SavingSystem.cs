using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        /// <summary>
        /// Serializes all states
        /// </summary>
        /// <param name="saveFile"></param>
        public void Save(string saveFile)
        {
            Dictionary<string,object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }
        /// <summary>
        /// Loads the serialized states
        /// </summary>
        /// <param name="saveFile"></param>
        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetSaveFilePath(saveFile); 
            using (FileStream file = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, state);
            }
            Debug.Log("Saving to... " + path);
        }

        private Dictionary<string,object> LoadFile(string saveFile)
        {
            string path = GetSaveFilePath(saveFile);
            object restoredState;
            if (!File.Exists(path)) return new Dictionary<string, object>();
            using (FileStream file = File.Open(path, FileMode.Open))
            {
                 BinaryFormatter formatter = new BinaryFormatter();
                 restoredState = formatter.Deserialize(file);
            }
            return (Dictionary<string,object>) restoredState;
        }

        public void CaptureState(Dictionary<string, object> state)
        {
            foreach(SavaebleEntity entety in FindObjectsOfType<SavaebleEntity>())
            {
                state[entety.GetUniqueIdentifier()] = entety.CaptureState();
            }
        }
        
        public void RestoreState(Dictionary<string,object> state)
        {
            foreach (SavaebleEntity entety in FindObjectsOfType<SavaebleEntity>())
            {
                if (!state.ContainsKey(entety.GetUniqueIdentifier()))
                {
                    return;
                }
                entety.RestoreState(state[entety.GetUniqueIdentifier()]);
            }
        }
        private string GetSaveFilePath(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
