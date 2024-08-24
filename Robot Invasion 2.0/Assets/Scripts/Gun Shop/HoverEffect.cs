using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using cowsins;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject objToEnable;
    public GameObject[] objsToDisable;

    // pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
    foreach(var item in objsToDisable)
{ item.SetActive(false);
}

objToEnable.SetActive(true); 
    }

    //  pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach(var item in objsToDisable)
{ item.SetActive(false);
}

objToEnable.SetActive(false); 
    }

// Clicked
    public void OnPointerClick(PointerEventData eventData)
    {
  if (!CoinManager.Instance.CheckIfEnoughCoins(100))
        {
            Debug.Log("Not enough coins");
            return;
        }

CoinManager.Instance.RemoveCoins(100);
// Buy the weapon here, the coins have already been reduced
    }
}
