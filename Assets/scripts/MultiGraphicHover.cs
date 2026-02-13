using UnityEngine;
using UnityEngine.EventSystems;

public class TriangleTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SpriteRenderer[] triangles;

    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    void Awake()
    {
        triangles = GetComponentsInChildren<SpriteRenderer>();
        foreach (var t in triangles)
            t.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var t in triangles)
            t.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var t in triangles)
            t.color = normalColor;
    }
}