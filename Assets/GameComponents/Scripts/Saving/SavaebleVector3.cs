using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Saving
{
    [System.Serializable]
    public class SavaebleVector3
    {
        float x, y, z;
        public SavaebleVector3(Vector3 position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }

        public Vector3 ToVector3()
        {
            Vector3 deserializedVector3 = new Vector3();
            deserializedVector3.x = x;
            deserializedVector3.y = y;
            deserializedVector3.z = z;
            return deserializedVector3;
        }
    }
}
