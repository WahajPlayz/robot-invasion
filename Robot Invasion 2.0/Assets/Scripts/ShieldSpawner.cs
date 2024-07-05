using UnityEngine;
using UnityEngine.InputSystem;
using cowsins;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to be spawned
    public GameObject spawnPoint; // The GameObject used for the spawn position and rotation
    public float cooldownTime = 2.0f; // Cooldown time in seconds

    private float lastSpawnTime;

    // Update is called once per frame
    void Update()
    {
        // Check for user input (e.g., pressing the space key) to spawn a prefab
        if (InputManager.ShieldSpawner && Time.time >= lastSpawnTime + cooldownTime)
        {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        if (spawnPoint != null && prefabs.Length > 0)
        {
            // Use the position and rotation of the spawnPoint GameObject
            Vector3 spawnPosition = spawnPoint.transform.position;
            Quaternion spawnRotation = spawnPoint.transform.rotation;

            // Select a random prefab from the array
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject prefabToSpawn = prefabs[randomIndex];

            // Instantiate the selected prefab at the specified position and rotation
            Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
            Debug.Log("Prefab spawned at position: " + spawnPosition);

            // Update the last spawn time
            lastSpawnTime = Time.time;
        }
        else
        {
            Debug.LogWarning("Spawn point is not assigned or no prefabs available.");
        }
    }
}
