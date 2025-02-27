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
    public GameObject projectile;
    public Sprite sprite;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI castspeedText;
    public TextMeshProUGUI damageText;
    public Image spriteImage;
        

    // Start is called before the first frame update
    void Start()
    {
        card_name = data.card_name;
        description = data.description;
        castspeed = data.castspeed;
        damage = data.damage;
        sprite = data.sprite;
        nameText.text = card_name;
        descriptionText.text = description;
        //castspeedText.text = castspeed.ToString();
        damageText.text = damage.ToString();
        spriteImage.sprite = sprite;
    }
}
