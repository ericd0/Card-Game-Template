using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Card_data data;

    public string card_name;
    public string description;
    public float castspeed;
    public float damage;
    public float projectileSpeed;
    public int type;
    //0 = projectile, 1 = buff,...
    public int piercing;
    public GameObject projectile;
    public BuffEffect buffEffect;
    public Sprite sprite;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI castspeedText;
    public TextMeshProUGUI damageText;
    public Image spriteImage;
        

    // Start is called before the first frame update
    void Start()
    {
        // Set common properties
        card_name = data.card_name;
        description = data.description;
        castspeed = data.castspeed;
        sprite = data.sprite;

        // Set type-specific properties
        if (data is ProjectileCard_data projectileData)
        {
            damage = projectileData.damage;
            projectileSpeed = projectileData.velocity;
            piercing = projectileData.piercing;
            projectile = projectileData.projectile;
        }
        else if (data is BuffCard_data buffData)
        {
            buffEffect = buffData.buffEffect;
        }

        // Update UI
        UpdateUI();
    }

    void UpdateUI()
    {
        nameText.text = card_name;
        descriptionText.text = description;
        spriteImage.sprite = sprite;
        
        if (data is ProjectileCard_data)
        {
            damageText.text = damage.ToString();
        }
        else
        {
            damageText.gameObject.SetActive(false);
        }
    }
}