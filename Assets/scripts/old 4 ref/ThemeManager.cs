using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public List<Image> uiImagesToRecolor;
    public List<SpriteRenderer> sprites;

    public Color targetColor;
    public Color[] themeColors;
    private int index = 0;
    private Color CurrentColor;

    


    void Start()
    {
        index = PlayerPrefs.GetInt("ThemeIndex", 0);
        CurrentColor = themeColors[index];
        ApplyTheme();
    }



    public void ApplyTheme()
    {
        foreach (Image img in uiImagesToRecolor)
            img.color = targetColor;

        foreach (SpriteRenderer s in sprites)
            s.color = targetColor;
        

        // choice persists
        // PlayerPrefs.SetString("ThemeColor", ColorUtility.ToHtmlStringRGBA(targetColor));
    }
    public void NextTheme()
    {
        index++;
        if (index >= themeColors.Length) index = 0;

        foreach (Image img in uiImagesToRecolor)
            img.color = themeColors[index];

        foreach (SpriteRenderer s in sprites)
            s.color = themeColors[index];


        // choice persists
        // PlayerPrefs.SetInt("ThemeIndex", index);
    }
}
