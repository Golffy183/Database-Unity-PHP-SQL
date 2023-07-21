using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketValue : MonoBehaviour
{
    [SerializeField] private int marketID;
    public int MarketID
    {
        get { return marketID; }
        set { marketID = value; }
    }

    public void onClickSelectMarket()
    {
        DatabaseController.instance.CallGetItemByMarketID(marketID);
    }
}
