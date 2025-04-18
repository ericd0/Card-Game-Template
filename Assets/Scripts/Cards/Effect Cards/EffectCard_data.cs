using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Card", menuName = "Cards/Buff Card")]
public class EffectCard_data : Card_data
{
    public Effect[] effects; // Change from BuffEffect to Effect[]
}