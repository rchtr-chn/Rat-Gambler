using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossTransitionScript : MonoBehaviour
{
    public GameObject Obj;
    public Image Background;
    public Text Text;
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
            Background.color = new Color(0f, 0f, 0f, timer);
            Text.color = new Color(1f, 1f, 1f, timer);
            yield return null;
        }
        Obj = gameObject;
        Obj.SetActive(false);
    }
}
