using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;


namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavaebleEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public Dictionary<string,object> CaptureState()
        {
            Dictionary<string, object> allSavaebleStates = new Dictionary<string, object>();
            foreach (IESavaeble savaeble in gameObject.GetComponents<IESavaeble>())
            {
                allSavaebleStates[savaeble.ToString()] = savaeble.CaptureState();
            }
            return allSavaebleStates;
        }
        public void RestoreState(object state)
        {
            Dictionary<string,object> restoredSavaebleStates = (Dictionary<string, object>)state;
            RestorePlayerStates(restoredSavaebleStates);
        }

        private void RestorePlayerStates(Dictionary<string,object> state)
        {
            foreach(IESavaeble savaeble in gameObject.GetComponents<IESavaeble>())
            {
                savaeble.RestoreState(state[savaeble.ToString()]);
            }
        }


        #if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
    #endif
