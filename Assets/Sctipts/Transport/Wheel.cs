using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Material _material;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Color _dirtColor;
    [SerializeField] private Color _defaultColor;

    private void OnEnable()
    {   if (_material != null)
            _material.color = _defaultColor;
    }

    public void StartRotate()
    {
        _animator.enabled = true;
        _animator.Play("Rotate");
        StartCoroutine(WheelContamination());

        StartCoroutine(ShowDirtEffect());
    }

    public void StopRotate()
    {
        _animator.enabled = false;
        _particle.Stop();
        _particle.gameObject.SetActive(false);
    }

    private IEnumerator WheelContamination()
    {
        yield return null;
    }


    private IEnumerator ShowDirtEffect()
    {
        yield return new WaitForSeconds(0.7f);
        _particle.gameObject.SetActive(true);
        _particle.Play();
    }

    private IEnumerator WheelColorChange()
    {
        float elapsedTime = 0;

        while(elapsedTime < 5)
        {
            elapsedTime += Time.deltaTime;

            _material.color = Color.Lerp(_material.color, _dirtColor, 0.0005f );
            yield return null;
        }
    }

    public void MoveOnDirt()
    {
        StartCoroutine(WheelColorChange());
    }
}
