using System.Collections.Generic;
using UnityEngine;

public class MechCustomizationMaster : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    public List<GameObject> torsoPrefabs; // List of available torso prefabs
    public List<GameObject> legPrefabs; // List of available leg prefabs
    public List<ArmPair> arms; // List of available arm pairs
    public List<GameObject> headPrefabs; // List of available head prefabs

    public int selectedTorsoIndex = 0; // Index to determine the selected torso
    public int selectedLegIndex = 0; // Index to determine the selected legs
    public int selectedArmIndex = 0; // Index to determine the selected arm pair
    public int selectedHeadIndex = 0; // Index to determine the selected head

    void Start()
    {
        if (gameManager != null)
        {
            // Select and pass the prefabs to the GameManager based on selected indexes
            gameManager.torsoPrefab = (selectedTorsoIndex >= 0 && selectedTorsoIndex < torsoPrefabs.Count) ? torsoPrefabs[selectedTorsoIndex] : null;
            gameManager.headPrefab = (selectedHeadIndex >= 0 && selectedHeadIndex < headPrefabs.Count) ? headPrefabs[selectedHeadIndex] : null;
            gameManager.legsPrefab = (selectedLegIndex >= 0 && selectedLegIndex < legPrefabs.Count) ? legPrefabs[selectedLegIndex] : null;

            if (selectedArmIndex >= 0 && selectedArmIndex < arms.Count)
            {
                gameManager.leftArmPrefab = arms[selectedArmIndex].leftArmPrefab;
                gameManager.rightArmPrefab = arms[selectedArmIndex].rightArmPrefab;
            }

           
        }
    }
}
