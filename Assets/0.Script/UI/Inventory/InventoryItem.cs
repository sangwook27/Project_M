using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryItem : MonoBehaviour
{
    public Image iconImg;

    //[SerializeField] private Image iconImg;
    [SerializeField] private TMP_Text countTxt;

    private int itemCount = 0;

    public string SpriteName
    {
        get{ return iconImg.sprite.name; }
    }
    public Sprite Icon
    {
        get{ return iconImg.sprite; }
        set
        {
            iconImg.sprite = value;
        }
    }

    public int Count
    {
       get{ return itemCount; }
       set
       {
          itemCount = value;
          countTxt.text = $"{itemCount}";
       }
    }


    public void ItemIcon(Sprite sprite)
    {
        Icon = sprite;
    }

    public void ItemCount(int i)
    {
        // æ∆¿Ã≈€ ∞πºˆ
        Count = i;
    }
}
