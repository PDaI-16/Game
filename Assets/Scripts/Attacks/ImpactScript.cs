using UnityEngine;

public class ImpactScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Destroy()
    {
        Destroy(gameObject);
    }

}