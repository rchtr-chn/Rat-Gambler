using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUIScript : MonoBehaviour
{
    public Text debt;
    public Text cookies;
    public Text day;

    void Update()
    {
        debt.text = "-" + GameManagerScript.instance.debtAmount.ToString();
        cookies.text = GameManagerScript.instance.CookieManagerScript.playerCookies.ToString();
        day.text = GameManagerScript.instance.turnsLeft.ToString();
    }
}
