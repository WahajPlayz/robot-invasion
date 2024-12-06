using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject torsoPrefab; // The main torso prefab
    public GameObject headPrefab;  // Head prefab
    public GameObject leftArmPrefab; // Left arm prefab
    public GameObject rightArmPrefab; // Right arm prefab
    public GameObject legsPrefab;  // Legs prefab
    public Transform spawnPoint;   // The spawn point for the mech

    void Start()
    {
        SpawnMech();
    }

    public void SpawnMech()
    {
        // Instantiate the torso at the spawn point
        GameObject torso = Instantiate(torsoPrefab, spawnPoint.position, spawnPoint.rotation);
        torso.transform.parent = spawnPoint; // Set the spawn point as the parent for organization

        // Find attachment points in the torso prefab
        Transform headAttachPoint = torso.transform.Find("HeadAttachPoint");
        Transform leftArmAttachPoint = torso.transform.Find("LeftArmAttachPoint");
        Transform rightArmAttachPoint = torso.transform.Find("RightArmAttachPoint");
        Transform legsAttachPoint = torso.transform.Find("LegsAttachPoint");

        // Instantiate the head at the head attachment point if it exists
        if (headAttachPoint != null)
        {
            GameObject head = Instantiate(headPrefab, headAttachPoint.position, Quaternion.identity);
            head.transform.parent = torso.transform; // Attach to the torso
        }

        // Instantiate the left arm at the left arm attachment point if it exists
        if (leftArmAttachPoint != null && leftArmPrefab != null)
        {
            GameObject leftArm = Instantiate(leftArmPrefab, leftArmAttachPoint.position, Quaternion.identity);
            leftArm.transform.parent = torso.transform; // Attach to the torso
        }

        // Instantiate the right arm at the right arm attachment point if it exists
        if (rightArmAttachPoint != null && rightArmPrefab != null)
        {
            GameObject rightArm = Instantiate(rightArmPrefab, rightArmAttachPoint.position, Quaternion.identity);
            rightArm.transform.parent = torso.transform; // Attach to the torso
        }

        // Instantiate the legs at the legs attachment point if it exists
        if (legsAttachPoint != null)
        {
            GameObject legs = Instantiate(legsPrefab, legsAttachPoint.position, Quaternion.identity);
            legs.transform.parent = torso.transform; // Attach to the torso
        }
    }
}
