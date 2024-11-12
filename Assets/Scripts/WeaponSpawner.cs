using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/*https://docs.unity3d.com/6000.0/Documentation/Manual/instantiating-prefabs-intro.html
https://discussions.unity.com/t/best-way-to-reference-a-prefab-game-object/935673/7
https://discussions.unity.com/t/instantiating-gameobjects-at-random-screen-positions/633835 
https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/
 */

public class WeaponSpawner : MonoBehaviour
{

    [SerializeField] Sprite[] weaponSprites;

    [SerializeField] GameObject weapon;

    private SpriteRenderer weaponSpriteRenderer;

    [SerializeField] Sprite chosenSprite;

    [SerializeField] GameObject background;

    private float _randomX;
    private float _randomY;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //weapon = GameObject.Find("Weapon");

        int randomWeaponIndex = Random.Range(0, weaponSprites.Length);
        chosenSprite = weaponSprites[randomWeaponIndex];


        weaponSpriteRenderer = weapon.GetComponentInChildren<SpriteRenderer>();
        weaponSpriteRenderer.sprite = chosenSprite;


        Renderer renderer = background.GetComponent<SpriteRenderer>();
        _randomX = Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        _randomY = Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        Instantiate(weapon, new Vector3(_randomX, _randomY, 0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
