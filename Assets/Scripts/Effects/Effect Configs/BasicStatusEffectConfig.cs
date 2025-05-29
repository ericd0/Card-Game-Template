using UnityEngine;
[CreateAssetMenu(fileName = "New Effect Config", menuName = "Effects/Basic Status Effect Configuration")]

public class BasicStatusEffectConfig : EffectConfig
{
    [SerializeField] private string[] statModifiersName = new string[0];
    public string[] StatModifiersName => statModifiersName;
}