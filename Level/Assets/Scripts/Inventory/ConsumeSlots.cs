using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsumeSlots : MonoBehaviour
{
    Inventory inventory;
    ConsumableInventory consumeInventory;

    public Image icon;
    public Button buy;
    public Consumable item;
    public TextMeshProUGUI price;

    bool canBuy;

    private void Start()
    {
        inventory = Inventory.instance;
        consumeInventory = ConsumableInventory.instance;
    }
    private void Update()
    {
        BuyCheck();

        if (!canBuy)
        {
            buy.interactable = false;
        }
        else
            buy.interactable = true;
    }
    public void Buy()
    {
        if (canBuy)
        {
            if(inventory.items.Contains(item))
            {
                item.numOfItems++;
            }
            else
            {
                item.numOfItems = 1;
                inventory.Add(item);
            }

        }
        gameManager.instance.currencyNumber -= item.buyPrice;
        gameManager.instance.playerScript.updatePlayerHUD();
        Inventory.instance.onItemChangedCallback.Invoke();
    }

    public void AddItem(Consumable newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        price.text = item.buyPrice.ToString();

    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void BuyCheck()
    {
        if (gameManager.instance.currencyNumber >= item.buyPrice)
        {
            canBuy = true;
        }
        else
            canBuy = false;
    }
}