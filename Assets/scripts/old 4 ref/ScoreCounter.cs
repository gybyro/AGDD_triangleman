using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreLabel;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = GameManager.instance.score.ToString();
    }
}
