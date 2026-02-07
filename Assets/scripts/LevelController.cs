// using System.Collections.Generic;
// using UnityEngine;

// public class LevelController : MonoBehaviour
// {
//     // for levelS manager    vvv
//     // public GameObject[] levelPrefabs; // assign your level prefabs in the inspector
//     // private GameObject currentLevel;

//     [SerializeField] private List<TriangleStateControl> triangles;

//     private void Awake()
//     {
//         foreach (var tri in triangles)
//         {
//             tri.OnRotationChanged += OnTriangleRotated;
//         }
//     }

//     private void OnDestroy()
//     {
//         foreach (var tri in triangles) { tri.OnRotationChanged -= OnTriangleRotated; }
//     }

//     private void OnTriangleRotated(TriangleStateControl changed)
//     {
//         EvaluateLevel();
//     }

//     private void EvaluateLevel()
//     {
//         // Put logic here


//         // all triangles at goal
//         foreach (var tri in triangles)
//         {
//             if (!tri.IsAtGoal())
//             {
//                 tri.SetState(TriangleState.Active);
//                 return;
//             }
//         }

//         // All satisfied
//         foreach (var tri in triangles)
//             tri.SetState(TriangleState.Done);
//     }

// }










    // Call this to load a level by index
//     public void LoadLevel(int index)
//     {
//         // Remove the old level if it exists
//         if (currentLevel != null)
//         {
//             Destroy(currentLevel);
//         }

//         // Instantiate the new level
//         if (index >= 0 && index < levelPrefabs.Length)
//         {
//             currentLevel = Instantiate(levelPrefabs[index]);
//         }
//         else
//         {
//             Debug.LogError("Level index out of range!");
//         }
//     }




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