using UnityEngine;
using System.Collections;



public class WPivotMover : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private AnimationCurve rotationCurve;

    public SpriteRenderer sprite;
    public Color normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color invis = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    private bool isRotating = false;

    public float targetRotationx;
    public float targetRotationy;
    public float targetRotationz;

    private bool hasPlayed = false;

    private void Awake()
    {
        sprite.color = invis;
    }

    public void PlayFlip()
    {
        sprite.color = normalColor;

        if (hasPlayed) return;
        hasPlayed = true;

        if (!isRotating)
            StartCoroutine(RotateToTarget());
    }


    private IEnumerator RotateToTarget()
    {
        isRotating = true;

        Quaternion fromRot = transform.localRotation;
        Quaternion toRot = Quaternion.Euler(
            targetRotationx,
            targetRotationy,
            targetRotationz
        );

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            float eased = rotationCurve.Evaluate(t);

            transform.localRotation = Quaternion.Slerp(
                fromRot,
                toRot,
                eased
            );

            yield return null;
        }

        transform.localRotation = toRot;
        isRotating = false;
    }

}