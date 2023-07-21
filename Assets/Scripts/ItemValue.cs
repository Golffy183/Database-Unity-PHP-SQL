using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemValue : MonoBehaviour
{
    [SerializeField] private int itemID;
    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }

    [SerializeField] private string itemName;
    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    [SerializeField] private string itemDescription;
    public string ItemDescription
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }

    [SerializeField] private int itemPiece;
    public int ItemPiece
    {
        get { return itemPiece; }
        set { itemPiece = value; }
    }

    [SerializeField] private int itemPrice;
    public int ItemPrice
    {
        get { return itemPrice; }
        set { itemPrice = value; }
    }

    public void onClickBuyItem()
    {
        if (DatabaseController.instance.AccountCurrentCoin < ItemPrice)
        {
            Debug.Log("Not enough money!");
            return;
        }
        DatabaseController.instance.CallBuyItem(ItemID, ItemPrice);
    }

    public void onClickSellItem()
    {
        if (ItemPiece <= 0)
        {
            Debug.Log("Not enough item!");
            return;
        }
        DatabaseController.instance.CallSellItem(ItemID, ItemPrice);

        // update item piece
        ItemPiece--;
        this.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "piece: " + ItemPiece.ToString();
    }
}
