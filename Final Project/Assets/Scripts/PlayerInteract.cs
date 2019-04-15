using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    public GameObject currentInterObj = null;
    public InteractionObject currentInterObjScript = null;
    public Inventory inventory;
    private Stats stat;

    void Start()
    {

        stat = GameObject.Find("Player").GetComponent<Stats>();
    }

    private void Update() {
        if (Input.GetButtonDown("Interact") && currentInterObj) {
            //Check if able to store object in inventory
            if (currentInterObjScript.inventory) {
                inventory.addItem(currentInterObj);
            }
            //Nulify the objects so the player cannot 
            //keep adding duplicates to inventory
            currentInterObj = null;
            currentInterObjScript = null;
        }

        // Use a health potion
        if (Input.GetButtonDown("Potion")) {
            //Check the inventory for a potion
            GameObject potion = inventory.FindItemByType("HealthPotion");
            if (potion != null)
            {
                //Use the potion - apply its effect
                //Remove the potion from inventory
                inventory.RemoveItem(potion);
                Debug.Log("Health potion was used");
                stat.addHealth(2);
    
            } else {
                Debug.Log("Cannot find potion in inventory");
            }
        }

        // Use a Power potion
        if (Input.GetButtonDown("PowerPotion")) {
            //Check the inventory for a potion
            GameObject potion1 = inventory.FindItemByType("PowerPotion");
            if (potion1 != null)
            {
                // Use the potion - apply it effects 
                //Remove the potion from inventory
                inventory.RemoveItem(potion1);
                Debug.Log("Power Potion was used");
                stat.addDamage(1);
            }
            else {
                Debug.Log("Cannot find power potion in inventory");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("interObject")) {
            Debug.Log(other.name);
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("interObject")) {

            if (other.gameObject == currentInterObj) {
                currentInterObj = null;
            }
        }

    }

}
