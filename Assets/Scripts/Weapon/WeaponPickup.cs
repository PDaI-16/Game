using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("Weapon Pickup start");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player is on weapon");
    }


}
