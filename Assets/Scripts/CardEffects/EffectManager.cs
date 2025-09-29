using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectManager : MonoBehaviour
{
    private GameManagerScript _gameManager;
    public GameObject SelectTableSidePrompt;
    public GameObject CopyCardPrompt;
    public GameObject PromptParent;
    public IEnumerator Add3PointsAnyCoroutine()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        GameObject promptObj = Instantiate(SelectTableSidePrompt, Vector3.zero, Quaternion.identity, PromptParent.transform);
        promptObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        bool mouseClicked = false;
        while (!mouseClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseClicked = true;
                if(Input.mousePosition.y > Screen.height / 2)
                {
                    _gameManager.AdditionalEnemyPoints += 3;
                    Debug.Log("Top side clicked");
                }
                else
                {
                    _gameManager.AdditionalPlayerPoints += 3;
                    Debug.Log("Bottom side clicked");
                }
                Destroy(promptObj);
            }
            yield return null;
        }

        _gameManager.EndPlayerTurn();
    }

    public IEnumerator Minus3PointsAnyCoroutine()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        GameObject promptObj = Instantiate(SelectTableSidePrompt, Vector3.zero, Quaternion.identity, PromptParent.transform);
        promptObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        bool mouseClicked = false;
        while (!mouseClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseClicked = true;
                if (Input.mousePosition.y > Screen.height / 2)
                {
                    _gameManager.AdditionalEnemyPoints -= 3;
                    Debug.Log("Top side clicked");
                }
                else
                {
                    _gameManager.AdditionalPlayerPoints -= 3;
                    Debug.Log("Bottom side clicked");
                }
                Destroy(promptObj);
            }
            yield return null;
        }

        _gameManager.EndPlayerTurn();
    }

    public IEnumerator CopyCardEffect()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        GameObject promptObj = Instantiate(CopyCardPrompt, Vector3.zero, Quaternion.identity, PromptParent.transform);
        promptObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject playerField = GameObject.Find("PlayerField");
        GameObject enemyField = GameObject.Find("EnemyField");

        List<RaycastResult> results = new List<RaycastResult>();

        bool cardSelected = false;
        while (!cardSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    Transform parentTransform = result.gameObject.transform.parent;
                    Transform grandParentTransform = parentTransform.parent;
                    GameObject obj = grandParentTransform.gameObject;

                    Card card = obj.GetComponent<CardDisplay>().CardData;
                    if ((result.gameObject.transform.IsChildOf(playerField.transform) || result.gameObject.transform.IsChildOf(enemyField.transform)) && card != null)
                    {
                        GameObject handManagerObj = GameObject.Find("PlayerHandManager");
                        HandManagerScript handManager = handManagerObj.GetComponent<HandManagerScript>();
                        handManager.AddCopiedCardToHand(card);
                        cardSelected = true;
                        Destroy(promptObj);
                        break;
                    }
                }

            }
            yield return null;
        }
        
        _gameManager.EndPlayerTurn();
    }

    //public IEnumerator SwapCards()
    //{
    //    gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    //    GameObject playerField = GameObject.Find("PlayerField");
    //    GameObject enemyField = GameObject.Find("EnemyField");
    //    DeckManagerScript playerDeck = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
    //    DeckManagerScript enemyDeck = GameObject.FindGameObjectWithTag("EnemyDeckManager").GetComponent<DeckManagerScript>();
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    Card firstCard = null;
    //    Card secondCard = null;

    //    bool bothCardsSelected = false;
    //    while (!bothCardsSelected)
    //    {
    //        if(Input.GetMouseButtonDown(0))
    //        {
    //            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
    //            {
    //                position = Input.mousePosition
    //            };
    //            EventSystem.current.RaycastAll(pointerEventData, results);
    //            foreach (RaycastResult result in results)
    //            {
    //                Transform parentTransform = result.gameObject.transform.parent;
    //                Transform grandParentTransform = parentTransform.parent;
    //                GameObject obj = grandParentTransform.gameObject;
    //                Card card = obj.GetComponent<CardDisplay>().cardData;

    //                if (card != null)
    //                {
    //                    if (result.gameObject.transform.IsChildOf(playerField.transform))
    //                    {
    //                        firstCard = card;
    //                        if(secondCard != null)
    //                        {
    //                            bothCardsSelected = true;
    //                        }
    //                    }
    //                    else if (result.gameObject.transform.IsChildOf(enemyField.transform))
    //                    {
    //                        secondCard = card;
    //                        Debug.Log(secondCard);
    //                        if (firstCard != null)
    //                        {
    //                            bothCardsSelected = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        yield return null;
    //    }

    //    if(firstCard != null && secondCard != null)
    //    {
    //        FieldManagerScript playerFieldScript = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
    //        FieldManagerScript enemyFieldScript = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>();

    //        for (int i=0; i < playerFieldScript.fieldCards.Count - 1; i++)
    //        {
    //            if (playerFieldScript.fieldCards[i].GetComponent<CardDisplay>().cardData == firstCard)
    //            {
    //                playerFieldScript.fieldCards.RemoveAt(i);
    //                Destroy(playerFieldScript.fieldCards[i]);
    //                break;
    //            }
    //        }
    //        for (int i = 0; i < enemyFieldScript.fieldCards.Count - 1; i++)
    //        {
    //            if (enemyFieldScript.fieldCards[i].GetComponent<CardDisplay>().cardData == secondCard)
    //            {
    //                enemyFieldScript.fieldCards.RemoveAt(i);
    //                Destroy(enemyFieldScript.fieldCards[i]);
    //                break;
    //            }
    //        }

    //        //playerFieldScript.AddCopiedCardToField(secondCard);
    //        //enemyFieldScript.AddCopiedCardToField(firstCard);


    //        gameManager.EndPlayerTurn();
    //    }
    //}
}
