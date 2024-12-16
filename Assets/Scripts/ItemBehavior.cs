using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public InventoryItem itemData;
    private Rigidbody rb;
    private void Start()
    {
        itemData = new InventoryItem
        {
            itemName = gameObject.name,
            id = Random.Range(1, 1000),
            weight = gameObject.GetComponent<Rigidbody>().mass,
            type = gameObject.tag.ToString(),
        };
    }
}
