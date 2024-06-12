using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab; // Assign your shield prefab in the Unity Editor
    public float shieldDuration = 5f; // Duration in seconds the shield will exist

    private GameObject currentShield;
    private PlayerActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerActions();
    }

    private void OnEnable()
    {
        inputActions.SheildSpawner.Enable();
        inputActions.SheildSpawner.SpawnShield.performed += OnSpawnShield;
    }

    private void OnDisable()
    {
        inputActions.SheildSpawner.SpawnShield.performed -= OnSpawnShield;
        inputActions.SheildSpawner.Disable();
    }

    private void OnSpawnShield(InputAction.CallbackContext context)
    {
        SpawnShield();
    }

    private void SpawnShield()
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }

        currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        Destroy(currentShield, shieldDuration);
    }
}
