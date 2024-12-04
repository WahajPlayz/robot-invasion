using UnityEngine;

public class JetpackController : MonoBehaviour
{
    public float jetpackForce = 10f; // The force applied when jetpack is activated
    public float moveSpeed = 5f; // Speed of movement while flying
    public float maxHeight = 20f; // Maximum height you can fly up to
    public float jetpackFuel = 100f; // Amount of fuel available for jetpack
    public float fuelConsumptionRate = 10f; // Rate at which fuel is consumed per second

    private Rigidbody rb;
    private bool isJetpacking = false;
    private float currentFuel;

    void Start()
    {
        // Get the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();
        currentFuel = jetpackFuel; // Initialize fuel
    }

    void Update()
    {
        // Handle Jetpack activation and flying logic
        HandleJetpack();
    }

    void HandleJetpack()
    {
        // Check if player is pressing the "Space" key and fuel is available
        if (Input.GetKey(KeyCode.Space) && currentFuel > 0)
        {
            if (!isJetpacking)
            {
                isJetpacking = true; // Start jetpacking
            }

            // Apply upward force to simulate jetpack
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Acceleration);

            // Decrease fuel while flying
            currentFuel -= fuelConsumptionRate * Time.deltaTime;

            // Limit max height for the player
            if (transform.position.y > maxHeight)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Stop upward movement when max height is reached
            }
        }
        else
        {
            if (isJetpacking)
            {
                isJetpacking = false; // Stop jetpacking
            }

            // If the jetpack is off, slowly refill fuel
            if (currentFuel < jetpackFuel)
            {
                currentFuel += 5f * Time.deltaTime; // Refill fuel over time
            }
        }

        // Allow forward movement while jetpacking
        if (isJetpacking && currentFuel > 0)
        {
            float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
            float vertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrows

            // Move the player in the direction of input (left/right and forward/backward)
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // Display fuel (optional: for debugging)
        Debug.Log("Current Fuel: " + currentFuel);
    }
}
