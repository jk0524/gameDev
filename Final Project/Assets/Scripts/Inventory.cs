using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventory = new GameObject[10];
    public Button[] InventoryButtons = new Button[10];

    public void addItem(GameObject item)
    {

        bool itemAdded = false;

        // Find the first open slot in our inventory
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                InventoryButtons[i].image.overrideSprite = item.GetComponent<SpriteRenderer>().sprite;
                Debug.Log(item.name + " was added");
                itemAdded = true;
                //Do something with the object
                item.SendMessage("DoInteraction");
                break;
            }
        }
        //Inventory was full
        if (!itemAdded)
        {
            Debug.Log("The inventory is full. Item not added");
        }
    }

    public bool FindItem(GameObject item) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == item) {
                return true;
            }
        }
        return false;
    }

    public GameObject FindItemByType(string itemType) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] != null) {
                if (inventory[i].GetComponent<InteractionObject>().itemType == itemType) {
                    return inventory[i];
                }
            }
        }
        return null;
    }

    public void RemoveItem(GameObject item) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == item) {
                inventory[i] = null;
                InventoryButtons[i].image.overrideSprite = null;
                break;
            }
        }
    }
}
