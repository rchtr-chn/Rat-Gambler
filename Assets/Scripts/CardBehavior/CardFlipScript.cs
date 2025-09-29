using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipScript : MonoBehaviour
{
    [SerializeField] private GameObject _frontView;
    [SerializeField] private GameObject _backView;
    [SerializeField] private float _duration = 0.6f;
    public bool ShowFront = true;
    private Coroutine _flipCoroutine;

    private void Start()
    {
        if (ShowFront)
        {
            _frontView.SetActive(true);
            _backView.SetActive(false);
        }
        else
        {
            _frontView.SetActive(false);
            _backView.SetActive(true);
        }
    }

    IEnumerator FlipCardAnimation(bool isFront)
    {
        float timer = 0f;

        float startAngle = isFront ? 0f : 180f;
        float endAngle = isFront ? 180f : 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float angle = Mathf.Lerp(startAngle, endAngle, timer / _duration);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            if ((angle >= 90f && isFront) || (angle <= 90f && !isFront))
            {
                _frontView.SetActive(!isFront);
                _backView.SetActive(isFront);
            }

            yield return null;
        }

        ShowFront = !ShowFront;
        _flipCoroutine = null;
    }

    public void FlipCard()
    {
        if (_flipCoroutine != null)
        {
            return;
        }
        _flipCoroutine = StartCoroutine(FlipCardAnimation(ShowFront));
    }

    public void FlipCardInstant()
    {
        _backView.SetActive(!ShowFront);
        _frontView.SetActive(ShowFront);
        ShowFront = !ShowFront;
    }
}
