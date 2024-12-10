using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    public static GameObject Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject;
        }
        else
        {
            Debug.LogError("Multiple Player instances detected! Ensure only one player exists.");
        }
    }

    private void OnDestroy()
    {
        if (Instance == gameObject)
        {
            Instance = null;
        }
    }
}
