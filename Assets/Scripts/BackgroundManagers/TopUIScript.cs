using UnityEngine;
using UnityEngine.UI;

public class TopUIScript : MonoBehaviour
{
    public Text Debt;
    public Text Cookies;
    public Text Day;

    void Update()
    {
        Debt.text = "-" + GameManagerScript.Instance.DebtAmount.ToString();
        Cookies.text = GameManagerScript.Instance.CookieManagerScript.PlayerCookies.ToString();
        Day.text = GameManagerScript.Instance.TurnsLeft.ToString();
    }
}
