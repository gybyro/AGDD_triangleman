using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levelPrefabs; // assign your level prefabs in the inspector
    private GameObject currentLevel;

    // Call this to load a level by index
    public void LoadLevel(int index)
    {
        // Remove the old level if it exists
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }

        // Instantiate the new level
        if (index >= 0 && index < levelPrefabs.Length)
        {
            currentLevel = Instantiate(levelPrefabs[index]);
        }
        else
        {
            Debug.LogError("Level index out of range!");
        }
    }
}



// using UnityEngine;
// using UnityEngine.UI;

// public class LevelSelector : MonoBehaviour
// {
//     public LevelManager levelManager;

//     public void SelectLevel(int index)
//     {
//         levelManager.LoadLevel(index);
//     }
// }