using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TriangleEdgeDrawer : MonoBehaviour
{
    public Vector3[] vertices; // assign your 3 triangle points
    public Color edgeColor = Color.red;
    public float width = 0.05f;

    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = vertices.Length + 1; // close the loop
        lr.loop = true;
        lr.startWidth = lr.endWidth = width;
        lr.startColor = lr.endColor = edgeColor;

        lr.SetPositions(new Vector3[] { vertices[0], vertices[1], vertices[2], vertices[0] });
    }
}


//////////////////// Different Colors per Edge
// using UnityEngine;

// public class MultiColorTriangle : MonoBehaviour
// {
//     public Vector3[] vertices; // 3 points
//     public Color[] edgeColors = new Color[3]; // color per edge
//     public float width = 0.05f;

//     void Start()
//     {
//         for (int i = 0; i < 3; i++)
//         {
//             GameObject edgeObj = new GameObject("Edge" + i);
//             edgeObj.transform.parent = transform;
//             LineRenderer lr = edgeObj.AddComponent<LineRenderer>();

//             lr.positionCount = 2;
//             lr.startWidth = lr.endWidth = width;
//             lr.startColor = lr.endColor = edgeColors[i];

//             Vector3 start = vertices[i];
//             Vector3 end = vertices[(i + 1) % 3];
//             lr.SetPositions(new Vector3[] { start, end });
//         }
//     }
// }