using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dexterity+", menuName = "Skills/Dexterity")]
public class Dexterity : SkillBase
{
    [SerializeField] int valueToIncrease = 0;

    public override void OnUnlock(int currentLevel)
    {
        onSkillUnlock?.Invoke(this);
    }
    public override void ActivateEffect(PlayerDemo player)
    {
        player.IncreaseDex(valueToIncrease);
    } 

    public override string GetEffect()
    {
        return "Dexterity + " + valueToIncrease.ToString();
    }
}
