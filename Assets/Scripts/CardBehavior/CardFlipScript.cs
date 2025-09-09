using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipScript : MonoBehaviour
{
    [SerializeField] GameObject frontView;
    [SerializeField] GameObject backView;
    [SerializeField] float duration = 0.6f;
    public bool showFront = true;
    Coroutine flipCoroutine;

    private void Start()
    {
        if (showFront)
        {
            frontView.SetActive(true);
            backView.SetActive(false);
        }
        else
        {
            frontView.SetActive(false);
            backView.SetActive(true);
        }
    }

    IEnumerator FlipCardAnimation(bool isFront)
    {
        float timer = 0f;

        float startAngle = isFront ? 0f : 180f;
        float endAngle = isFront ? 180f : 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float angle = Mathf.Lerp(startAngle, endAngle, timer / duration);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            if ((angle >= 90f && isFront) || (angle <= 90f && !isFront))
            {
                frontView.SetActive(!isFront);
                backView.SetActive(isFront);
            }

            yield return null;
        }

        showFront = !showFront;
        flipCoroutine = null;
    }

    public void FlipCard()
    {
        if (flipCoroutine != null)
        {
            return;
        }
        flipCoroutine = StartCoroutine(FlipCardAnimation(showFront));
    }

    public void FlipCardInstant()
    {
        backView.SetActive(!showFront);
        frontView.SetActive(showFront);
        showFront = !showFront;
    }
}
