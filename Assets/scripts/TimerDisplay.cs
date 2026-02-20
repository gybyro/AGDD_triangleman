using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public event System.Action OnTimerExpired;

    [SerializeField] private DigitController minTens;
    [SerializeField] private DigitController minOnes;
    [SerializeField] private DigitController secTens;
    [SerializeField] private DigitController secOnes;
    [SerializeField] private DigitController milOne;
    [SerializeField] private DigitController milTwo;
    [SerializeField] private DigitController milThree;


    
        [Header("Play Mode Preview")]
        [SerializeField] private bool previewCountdown = true;
        [SerializeField] private int previewStartSeconds = 90;
        [Tooltip("6000 seconds are 99 minutes and 60 seconds which is the max")]
        [SerializeField] private float expireDelay = 1f;
  

    private float timeRemaining;
    private int lastShownTime = -1;
    private bool timerReachedZero = false;
    private float zeroReachedTime;


    private void Start()
    {
        timeRemaining = previewStartSeconds;

    #if UNITY_EDITOR
        if (!previewCountdown)
            enabled = false;
    #endif
    }
    

    // updates every second
    private void Update()
    {
        #if UNITY_EDITOR
            if (!previewCountdown)
                return;
        #endif

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(0, timeRemaining);

        if (!timerReachedZero && timeRemaining <= 0f)
        {
            timerReachedZero = true;
            zeroReachedTime = Time.time;
            timeRemaining = 0f; // clamp visually
        }

        // wait before firing event
        if (timerReachedZero && Time.time >= zeroReachedTime + expireDelay)
        {
            OnTimerExpired?.Invoke();
            enabled = false;
        }

        int secondsInt = Mathf.CeilToInt(timeRemaining);

        if (secondsInt != lastShownTime)
        {
            lastShownTime = secondsInt;
            UpdateDigits(secondsInt);
        }

     
        int fullMilliseconds = Mathf.FloorToInt((timeRemaining * 1000f) % 1000); // 0–999
        int hundreds = fullMilliseconds / 100;
        int tens = (fullMilliseconds / 10) % 10;
        int ones = fullMilliseconds % 10;

        milOne.SetDigit(hundreds);
        milTwo.SetDigit(tens);
        milThree.SetDigit(ones);
    }


    private void UpdateDigits(int totalTime)
    {
        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime % 60f);

        

        minTens.SetDigitStaggered(minutes / 10);
        minOnes.SetDigitStaggered(minutes % 10);
        secTens.SetDigitStaggered(seconds / 10);
        secOnes.SetDigitStaggered(seconds % 10);

        
        // milTwo.SetFlickerMode(true);
        // milThree.SetFlickerMode(true);
    }
}