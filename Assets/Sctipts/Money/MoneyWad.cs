using System.Collections;
using UnityEngine;

public class MoneyWad : MonoBehaviour
{
    [SerializeField] private float _appearDuration;
    [SerializeField] private float _disappearDuration;
    [SerializeField] private AnimationCurve _curve;

    public void ShowAppear(Transform appearPoint)
    {
        if (isActiveAndEnabled)
            StartCoroutine(ShowAppearAnimation(appearPoint));
    }

    public void ShowDisappear(Transform disappearPoint)
    {
        if (disappearPoint == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.parent = null;
            StartCoroutine(ShowDisappearAnimation(disappearPoint.position));
        }
    }

    private IEnumerator ShowAppearAnimation(Transform startPoint)
    {
        float timer = 0;

        Vector3 target = transform.localPosition;

        while (timer < _appearDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _appearDuration;
            
            transform.localPosition = Vector3.Lerp(startPoint.localPosition, target, progress) +
                                      Vector3.up * _curve.Evaluate(progress);

            yield return null;
        }

        transform.localPosition = target;
    }

    private IEnumerator ShowDisappearAnimation(Vector3 disappearPoint)
    {
        Vector3 startPoint = transform.position;
        
        float timer = 0;

        while (timer < _disappearDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _disappearDuration;

            transform.position = Vector3.Lerp(startPoint, disappearPoint, progress) +
                                 Vector3.up * _curve.Evaluate(progress);
            
            yield return null;
        }

        Destroy(gameObject);
    }
}