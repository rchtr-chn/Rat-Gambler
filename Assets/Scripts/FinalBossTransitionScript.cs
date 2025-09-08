using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossTransitionScript : MonoBehaviour
{
    public GameObject obj;
    public Image background;
    public Text text;
    void Start()
    {
        StartCoroutine(fadeTransition());
    }

    IEnumerator fadeTransition()
    {
        yield return new WaitForSeconds(3f);
        //fade to black
        float timer = 1f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            background.color = new Color(0f, 0f, 0f, timer);
            text.color = new Color(1f, 1f, 1f, timer);
            yield return null;
        }
        obj = gameObject;
        obj.SetActive(false);
    }
}
