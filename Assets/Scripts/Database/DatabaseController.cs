using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DatabaseController : MonoBehaviour
{
    public static DatabaseController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of DatabaseController found!");
            return;
        }
        instance = this;
    }

    [SerializeField] private GameObject connectionButton;
    [SerializeField] private GameObject marketButton;
    [SerializeField] private GameObject inventoryButton;

    public int AccountID;
    public string AccountFirstName;
    public string AccountLastName;
    public int AccountCurrentCoin;

    public TextMeshProUGUI textFirstName;
    public TextMeshProUGUI textLastName;
    public TextMeshProUGUI textCurrentCoin;

    [SerializeField] private GameObject m_MarketItemPrefab;
    [SerializeField] private GameObject m_BuyItemPrefab;
    [SerializeField] private GameObject m_SellItemPrefab;
    [SerializeField] private Transform m_Container;

    public void EnableButton()
    {
        // set interactable
        connectionButton.GetComponent<Button>().interactable = false;
        CallInfoAccount();
        marketButton.GetComponent<Button>().interactable = true;
        inventoryButton.GetComponent<Button>().interactable = true;
    }

    public void CallInfoAccount()
    {
        StartCoroutine(InfoAccount());
    }

    IEnumerator InfoAccount()
    {

        UnityWebRequest www = UnityWebRequest.Get("https://www.game-camtcmu.com/project/getAccount.php");
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Connection Error!");
        }
        else
        {
            string s = www.downloadHandler.text;
            List<string> information = s.ToString().Split('-').ToList();
            AccountID = int.Parse(information[0]);
            AccountFirstName = information[1];
            AccountLastName = information[2];
            AccountCurrentCoin = int.Parse(information[3]);
            textFirstName.text = "Firstname: " + AccountFirstName;
            textLastName.text = "Lastname: " + AccountLastName;
            textCurrentCoin.text = "Coin: " + AccountCurrentCoin.ToString();
            www.Dispose();
        }
    }

    public void CallMarket()
    {
        StartCoroutine(Market());
    }

    IEnumerator Market()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.game-camtcmu.com/project/getMarket.php");
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Connection Error!");
        }
        else
        {
            string s = www.downloadHandler.text;
            List<string> marketList = s.ToString().Split(',').ToList();
            // Debug.Log(marketList.Count);

            marketList.RemoveAt(marketList.Count - 1);
            // clear all child
            foreach (Transform child in m_Container)
            {
                Destroy(child.gameObject);
            }
            foreach (string market in marketList)
            {
                List<string> information = market.Split('-').ToList();
                var marketItem = Instantiate(m_MarketItemPrefab);
                // keep information[0] in marketItem
                marketItem.GetComponent<MarketValue>().MarketID = int.Parse(information[0]);
                marketItem.GetComponentInChildren<TextMeshProUGUI>().text = information[1];
                marketItem.transform.SetParent(m_Container);
            }
        }
    }

    public void CallGetItemByMarketID(int marketID)
    {
        StartCoroutine(getItemByMarketID(marketID));
    }

    IEnumerator getItemByMarketID(int marketID)
    {
        WWWForm form = new WWWForm();
        form.AddField("Market_ID", marketID);
        WWW www = new WWW("https://www.game-camtcmu.com/project/getItem.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("Connection Error!");
        }
        else
        {
            string s = www.text;
            List<string> itemList = s.ToString().Split(',').ToList();
            // Debug.Log(itemList.Count);

            itemList.RemoveAt(itemList.Count - 1);
            // clear all child
            foreach (Transform child in m_Container)
            {
                Destroy(child.gameObject);
            }
            foreach (string item in itemList)
            {
                List<string> information = item.Split('-').ToList();
                var itemObject = Instantiate(m_BuyItemPrefab);
                itemObject.GetComponent<ItemValue>().ItemID = int.Parse(information[0]);
                itemObject.GetComponent<ItemValue>().ItemName = information[1];
                itemObject.GetComponent<ItemValue>().ItemDescription = information[2];
                itemObject.GetComponent<ItemValue>().ItemPrice = int.Parse(information[3]);
                // get Child TextMeshProUGUI with name "Name"
                itemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = information[1];
                // get Child TextMeshProUGUI with name "Description"
                itemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = information[2];
                // get Child TextMeshProUGUI with name "Price"
                itemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "$ " + information[3];
                itemObject.transform.SetParent(m_Container);
            }
        }
    }

    public void CallBuyItem(int Item_ID, int Item_Price)
    {
        StartCoroutine(buyItem(AccountID, Item_ID, Item_Price));
    }

    IEnumerator buyItem(int Account_ID, int Item_ID, int Item_Price)
    {
        WWWForm form = new WWWForm();
        form.AddField("Account_ID", Account_ID);
        form.AddField("Item_ID", Item_ID);
        form.AddField("Item_Price", Item_Price);
        WWW www = new WWW("https://www.game-camtcmu.com/project/buyItem.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("Connection Error!");
        }
        else
        {
            Debug.Log("Buy Item Success!");
            string s = www.text;
            Debug.Log(s);
            int coin = int.Parse(s);
            AccountCurrentCoin = coin;
            textCurrentCoin.text = "Coin: " + AccountCurrentCoin.ToString();
        }
    }

    public void CallInventory()
    {
        StartCoroutine(Inventory());
    }

    IEnumerator Inventory()
    {
        WWWForm form = new WWWForm();
        form.AddField("Account_ID", AccountID);
        WWW www = new WWW("https://www.game-camtcmu.com/project/getInventory.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("Connection Error!");
        }
        else
        {
            string s = www.text;
            List<string> inventoryList = s.ToString().Split(',').ToList();
            inventoryList.RemoveAt(inventoryList.Count - 1);
            foreach (Transform child in m_Container)
            {
                Destroy(child.gameObject);
            }
            foreach (string inventory in inventoryList)
            {
                List<string> information = inventory.Split('-').ToList();
                var itemObject = Instantiate(m_SellItemPrefab);
                itemObject.GetComponent<ItemValue>().ItemID = int.Parse(information[0]);
                itemObject.GetComponent<ItemValue>().ItemName = information[1];
                itemObject.GetComponent<ItemValue>().ItemDescription = information[2];
                itemObject.GetComponent<ItemValue>().ItemPiece = int.Parse(information[3]);
                itemObject.GetComponent<ItemValue>().ItemPrice = int.Parse(information[4]) / 2;
                // get Child TextMeshProUGUI with name "Name"
                itemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = information[1];
                // get Child TextMeshProUGUI with name "Description"
                itemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = information[2];
                // get Child TextMeshProUGUI with name "Piece"
                itemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "piece: " + information[3];
                // get Child TextMeshProUGUI with name "Price"
                itemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "$ " + (int.Parse(information[4]) / 2).ToString();
                itemObject.transform.SetParent(m_Container);
            }
        }
    }

    public void CallSellItem(int Item_ID, int Item_Price)
    {
        StartCoroutine(sellItem(AccountID, Item_ID, Item_Price));
    }

    IEnumerator sellItem(int Account_ID, int Item_ID, int Item_Price)
    {
        WWWForm form = new WWWForm();
        form.AddField("Account_ID", Account_ID);
        form.AddField("Item_ID", Item_ID);
        form.AddField("Item_Price", Item_Price);
        WWW www = new WWW("https://www.game-camtcmu.com/project/sellItem.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("Connection Error!");
        }
        else
        {
            Debug.Log("Sell Item Success!");
            string s = www.text;
            int coin = int.Parse(s);
            AccountCurrentCoin = coin;
            textCurrentCoin.text = "Coin: " + AccountCurrentCoin.ToString();
        }
    }
}
