using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public Card_data data;
    public string card_name;
    public float castspeed;
    public float damage;
    public float projectileSpeed;
    public int piercing;
    public GameObject projectile;
    public BuffEffect buffEffect;
    public Sprite sprite;
    
    // Only keep needed UI elements
    public TextMeshProUGUI nameText;
    public Image spriteImage;

    void Start()
    {
        // Set common properties
        card_name = data.card_name;
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

        UpdateUI();
    }

    void UpdateUI()
    {
        nameText.text = card_name;
        spriteImage.sprite = sprite;
    }
}