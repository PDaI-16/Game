using UnityEngine;

public class WeaponPickup : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player is on weapon");
    }


}
