using UnityEngine;

public class InventoryGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Inventory InventoryData;


    void Start()
    {
        InventoryData = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
