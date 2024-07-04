using UnityEngine;
using UnityEngine.InputSystem;
using cowsins;
using System.Data.Common;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject prefab; // The prefab to be spawned
    public GameObject spawnPoint; // The GameObject used for the spawn position and rotation

    // Update is called once per frame
    void Update()
    {
        // Check for user input (e.g., pressing the space key) to spawn the prefab
        if (InputManager.ShieldSpawner)
        {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        if (spawnPoint != null)
        {
            // Use the position and rotation of the spawnPoint GameObject
            Vector3 spawnPosition = spawnPoint.transform.position;
            Quaternion spawnRotation = spawnPoint.transform.rotation;

            // Instantiate the prefab at the specified position and rotation
            Instantiate(prefab, spawnPosition, spawnRotation);
            Debug.Log("Prefab spawned at position: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("Spawn point is not assigned.");
        }
    }
}