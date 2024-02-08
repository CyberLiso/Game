using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpawnPersistantGameObjects : MonoBehaviour
    {
        [SerializeField] GameObject PersistantGameObjectsPrefab;
        static bool hasSpawnedPersistantGameobjects = false;

        private void Awake()
        {
            if(hasSpawnedPersistantGameobjects) return;
            SpawnPersistantGameObjectsMethod();
            hasSpawnedPersistantGameobjects = true;
        }

        private void SpawnPersistantGameObjectsMethod()
        {
            GameObject PersistantGameObjects = Instantiate(PersistantGameObjectsPrefab);
            DontDestroyOnLoad(PersistantGameObjects);
        }
    }
}
