using UnityEngine;
using System.Collections;

public class DigitController : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private TriNumCell[] cells;
    [SerializeField] private float staggerDelay = 0.02f;
    private Coroutine updateRoutine;

    // randomizer
    private bool flickerMode = false;
    private int frameCounter = 0;
    [SerializeField] private int framesPerUpdate = 3;  // how many frames between the flicker


    [SerializeField] public bool isStatic = false;
    [SerializeField] public int staticDigit = 0;

    // 0–9 triangle masks
    // true = triangle ON
    private static readonly bool[][] DIGIT_MASKS =
    {
        // 0
        new bool[]
        {
            true,true,
            true,true,true,true,
            true,false,false,true,
            true,false,false,true,
            true,false,false,true,
            true,false,false,true,
            true,false,false,true,
            true,true,true,true,
            true,true
        },

        // 1
        new bool[]
        {
            true,true,
            true,true,true,false,
            false,false,true,false,
            false,false,true,false,
            false,false,true,false,
            false,false,true,false,
            false,false,true,false,
            false,false,true,false,
            false,true
        },

        // 2
        new bool[]
        {
            true,true,
            true,true,true,true,
            false,false,false,true,
            false,false,true,true,
            false,true,true,false,
            false,true,false,false,
            false,false,false,false,
            true,true,true,true,
            true,true
        },

        // 3
        new bool[]
        {
            true,true,
            true,true,true,true,
            false,false,false,true,
            false,false,true,true,
            false,true,true,false,
            false,false,true,true,
            false,false,false,true,
            true,true,true,true,
            true,true
        },

        // 4
        new bool[]
        {
            true,true,
            true,true,true,false,
            true,false,true,false,
            true,false,true,false,
            true,false,true,true,
            true,true,true,true,
            false,true,true,false,
            false,false,true,false,
            false,true
        },

        // 5
        new bool[]
        {
            false,false,
            true,true,true,true,
            true,false,false,false,
            true,false,false,false,
            false,true,true,false,
            false,false,true,true,
            false,false,false,true,
            true,true,true,true,
            true,true
        },

        // 6
        new bool[]
        {
            true,true,
            true,true,true,true,
            true,false,false,false,
            true,false,false,false,
            true,true,true,false,
            true,true,true,true,
            true,false,false,true,
            true,true,true,true,
            true,true
        },

        // 7
        new bool[]
        {
            true,true,
            true,true,true,false,
            true,false,true,true,
            false,false,true,true,
            false,true,true,false,
            false,true,true,false,
            false,false,true,false,
            false,false,true,false,
            false,true
        },

        // 8
        new bool[]
        {
            true,true,
            true,true,true,true,
            true,false,false,true,
            true,true,true,true,
            false,true,true,false,
            true,true,true,true,
            true,false,false,true,
            true,true,true,true,
            true,true
        },

        // 9
        new bool[]
        {
            true,true,
            true,true,true,true,
            true,false,false,true,
            true,true,true,true,
            true,false,false,false,
            true,false,false,false,
            true,false,false,false,
            true,true,true,true,
            true,true
        },
    };


    #if UNITY_EDITOR
    [Header("Play Mode Preview")]
    [SerializeField] private bool animateInPlayMode = true;
    [SerializeField] private int playModePreviewDigit = 0;

    private int lastPreviewDigit = -1;

    private void Start()
    {
        if (isStatic)
        {
            ApplyStaticDigit();
            return;
        }
    }
    private void ApplyStaticDigit()
    {
        ApplyMaskInstant(DIGIT_MASKS[staticDigit]);
    }


    // ======================== UPDATE ====================================
    private void Update()
    {
        if (!Application.isPlaying)
            return;

        

        if (playModePreviewDigit != lastPreviewDigit)
        {
            lastPreviewDigit = playModePreviewDigit;

            if (animateInPlayMode)
                SetDigitStaggered(playModePreviewDigit);
            else
                SetDigit(playModePreviewDigit);
        }

         // RANDOMIZER
        if (flickerMode)
        {
            frameCounter++;

            if (frameCounter >= framesPerUpdate)
            {
                frameCounter = 0;
                int randomDigit = Random.Range(0, 10); // 0-9
                ApplyMaskInstant(DIGIT_MASKS[randomDigit]);
            }
        }
    }

    // ======================== SET STUFF ====================================
    public void SetDigit(int digit)
    {
        ApplyMaskInstant(DIGIT_MASKS[digit]);
        // if (digit < 0 || digit > 9)
        // {
        //     Clear();
        //     return;
        // }

        // var mask = DIGIT_MASKS[digit];

        // for (int i = 0; i < cells.Length; i++)
        // {
        //     cells[i].SetVisible(mask[i]);
        // }
    }

    public void SetDigitStaggered(int digit)
    {
        if (updateRoutine != null)
            StopCoroutine(updateRoutine);
        updateRoutine = StartCoroutine(ApplyDigit(digit));
    }

       // for milliseconds, im randomizing the visibillity 
    public void SetFlickerMode(bool enable)
    {
        flickerMode = enable;
        frameCounter = 0; // resets the counter
    }

    // ======================== APPLY STUFF ====================================
    private void ApplyMaskInstant(bool[] mask)
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].SetVisible(mask[i]);
    }

    // private IEnumerator ApplyDigit(int digit)
    // {
    //     if (digit < 0 || digit > 9)
    //     {
    //         ClearInstant();
    //         yield break;
    //     }

    //     var mask = DIGIT_MASKS[digit];

    //     for (int i = 0; i < cells.Length; i++)
    //     {
    //         cells[i].SetVisible(mask[i]);
    //         yield return new WaitForSeconds(staggerDelay);
    //     }
    // }

    private IEnumerator ApplyDigit(int digit)
    {
        if (digit < 0 || digit > 9)
        {
            ClearInstant();
            yield break;
        }

        var mask = DIGIT_MASKS[digit];

        // Create an index list
        int[] indices = new int[cells.Length];
        for (int i = 0; i < indices.Length; i++)
            indices[i] = i;

        // Fisher–Yates shuffle
        for (int i = indices.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        // Apply in randomized order
        foreach (int i in indices)
        {
            cells[i].SetVisible(mask[i]);
            
            // yield return new WaitForSeconds(staggerDelay);
            yield return new WaitForSeconds(
                staggerDelay + Random.Range(-0.01f, 0.015f)
            );
        }
    }

    // private void RandomizeCells()
    // {
    //     foreach (var cell in cells)
    //     {
    //         // 50% chance to turn on or off
    //         bool randomVisible = Random.value > 0.5f;
    //         cell.SetVisible(randomVisible);
    //     }
    // }

    // ================================== CLEAR =======================================
    public void ClearInstant()
    {
        foreach (var cell in cells)
            cell.SetVisible(false);
    }

#endif
}