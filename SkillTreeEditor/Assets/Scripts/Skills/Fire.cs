using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Fire", menuName = "Skills/Abilites/Fire")]
public class Fire : SkillBase
{
    public override void OnUnlock(int currentLevel)
    {
        onSkillUnlock?.Invoke(this);
    }

    public override void ActivateEffect(PlayerDemo player)
    {
        player.AddAbility(this);
    }

    public override string GetEffect()
    {
        return "Unlocks new Ability: Fire"; 
    }
}
