// using UnityEngine;
// using UnityEngine.Tilemaps;

// public class HexCenters : MonoBehaviour
// {
//     public Grid grid;
//     public Tilemap tilemap;

//     void Start()
//     {
//         foreach (var cellPos in tilemap.cellBounds.allPositionsWithin)
//         {
//             if (!tilemap.HasTile(cellPos)) continue;

//             // Get the world center of this hex cell
//             Vector3 worldPos = grid.GetCellCenterWorld(cellPos);
//             Debug.Log($"Hex center at {worldPos}");
//         }
//     }
// }
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class HexTriangleSpawner : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;
    public GameObject trianglePrefab; // assign your triangle sprite prefab
    public float hexRadius = 1f; // distance from hex center to corner

    void Start()
    {
        SpawnTrianglesInCameraView();
    }

    void SpawnTrianglesInCameraView()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Get camera bounds in world space
        Vector3 bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Convert bounds to tilemap cell positions
        Vector3Int minCell = tilemap.WorldToCell(bottomLeft);
        Vector3Int maxCell = tilemap.WorldToCell(topRight);

        List<Vector3> triangleCenters = new List<Vector3>();

        // Iterate over all visible hex cells
        for (int x = minCell.x - 1; x <= maxCell.x + 1; x++)
        {
            for (int y = minCell.y - 1; y <= maxCell.y + 1; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(cellPos)) continue;

                Vector3 hexCenter = grid.GetCellCenterWorld(cellPos);

                // Compute 6 triangle centers per hex
                for (int i = 0; i < 6; i++)
                {
                    float angle1 = Mathf.Deg2Rad * (60 * i);
                    float angle2 = Mathf.Deg2Rad * (60 * (i + 1));

                    Vector3 corner1 = hexCenter + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0) * hexRadius;
                    Vector3 corner2 = hexCenter + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0) * hexRadius;

                    Vector3 triangleCenter = (hexCenter + corner1 + corner2) / 3f;

                    triangleCenters.Add(triangleCenter);

                    // Spawn your triangle prefab at this center
                    if (trianglePrefab != null)
                    {
                        Instantiate(trianglePrefab, triangleCenter, Quaternion.identity);
                    }
                }
            }
        }

        Debug.Log($"Spawned {triangleCenters.Count} triangle centers in camera view.");
    }
}