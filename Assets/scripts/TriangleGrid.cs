

// public struct TriangleCoord
// {
//     public int x;
//     public int y;
//     public bool isUp; // true = ▲, false = ▼

//     public TriangleCoord(int x, int y, bool isUp)
//     {
//         this.x = x;
//         this.y = y;
//         this.isUp = isUp;
//     }

// }

// Vector2 GetWorldPosition(TriangleCoord coord, float size)
// {
//     float height = size * Mathf.Sqrt(3) / 2f;

//     float xPos = coord.x * size * 0.5f;
//     float yPos = coord.y * height;

//     return new Vector2(xPos, yPos);
// }

// public List<TriangleCoord> GetNeighbors(TriangleCoord t)
// {
//     var neighbors = new List<TriangleCoord>();

//     if (t.isUp)
//     {
//         neighbors.Add(new TriangleCoord(t.x - 1, t.y, false));
//         neighbors.Add(new TriangleCoord(t.x + 1, t.y, false));
//         neighbors.Add(new TriangleCoord(t.x, t.y - 1, false));
//     }
//     else
//     {
//         neighbors.Add(new TriangleCoord(t.x - 1, t.y, true));
//         neighbors.Add(new TriangleCoord(t.x + 1, t.y, true));
//         neighbors.Add(new TriangleCoord(t.x, t.y + 1, true));
//     }

//     return neighbors;
// }