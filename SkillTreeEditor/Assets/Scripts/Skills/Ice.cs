using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ice", menuName = "Skills/Abilites/Ice")]
public class Ice : SkillBase
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
        return "Unlocks new Ability: Ice";
    }
}
