using UnityEngine;

public class TriNumCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetVisible(bool visible)
    {
        if (!Application.isPlaying)
            return; // editor preview does NOT toggle renderer

        spriteRenderer.enabled = visible;
    }
        // if (!Application.isPlaying)
        // {
        //     gameObject.SetActive(visible);
        // }
        // else
        // {
        //     spriteRenderer.enabled = visible;
        // }
    
}

// using UnityEngine;
// using System.Collections;

// public class TriNumCell : MonoBehaviour
// {
//     [SerializeField] private SpriteRenderer spriteRenderer;
//     [SerializeField] private float animDuration = 0.12f;
//     Coroutine animRoutine;


//     public void SetVisible(bool visible)
//     {
//         spriteRenderer.enabled = visible;
//     }

//      private void Awake()
//     {
//         spriteRenderer.enabled = false;
//         transform.localScale = Vector3.zero;
//     }

//     public void AnimateVisible(bool visible, float delay = 0f)
//     {
//         if (animRoutine != null)
//             StopCoroutine(animRoutine);

//         animRoutine = StartCoroutine(Animate(visible, delay));
//     }

//     private IEnumerator Animate(bool visible, float delay)
//     {
//         if (delay > 0f)
//             yield return new WaitForSeconds(delay);

//         spriteRenderer.enabled = true;

//         float t = 0f;
//         float start = transform.localScale.x;
//         float end = visible ? 1f : 0f;

//         while (t < animDuration)
//         {
//             t += Time.deltaTime;
//             float v = Mathf.Lerp(start, end, t / animDuration);
//             transform.localScale = Vector3.one * v;
//             yield return null;
//         }

//         transform.localScale = Vector3.one * end;

//         if (!visible)
//             spriteRenderer.enabled = false;
//     }
// }
