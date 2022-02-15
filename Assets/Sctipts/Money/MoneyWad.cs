using System.Collections;
using UnityEngine;

public class MoneyWad : MonoBehaviour
{
    [SerializeField] private float _appearDuration;
    [SerializeField] private AnimationCurve _curve;

    public void StartAppear(Transform startPoint)
    {
        if (isActiveAndEnabled)
            StartCoroutine(ShowAppearAnimation(startPoint));
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
}