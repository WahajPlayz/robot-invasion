using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject[] shieldPrefabs; // Array of shield prefabs
    public float shieldDuration = 5f; // Duration in seconds the shield will exist
    public float cooldownDuration = 1f; // Cooldown duration in seconds

    private GameObject currentShield;
    private PlayerActions inputActions;
    private float nextSpawnTime = 0f; // Tracks the next time a shield can be spawned

    private void Awake()
    {
        inputActions = new PlayerActions();
    }

    private void OnEnable()
    {
        inputActions.GameControls.Enable();
        inputActions.GameControls.ShieldSpawner.performed += OnSpawnShield;
    }

    private void OnDisable()
    {
        inputActions.GameControls.ShieldSpawner.performed -= OnSpawnShield;
        inputActions.Disable();
    }

    private void OnSpawnShield(InputAction.CallbackContext context)
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnShield();
            nextSpawnTime = Time.time + cooldownDuration;
        }
    }

    private void SpawnShield()
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }

        if (shieldPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, shieldPrefabs.Length);
            currentShield = Instantiate(shieldPrefabs[randomIndex], transform.position, Quaternion.identity);
            Destroy(currentShield, shieldDuration);
        }
        else
        {
            Debug.LogWarning("No shield prefabs assigned.");
        }
    }
}
