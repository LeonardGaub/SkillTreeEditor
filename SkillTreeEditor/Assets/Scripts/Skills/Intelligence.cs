using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Intelligence+", menuName = "Skills/Intelligence")]
public class Intelligence : SkillBase
{
    [SerializeField] int valueToIncrease = 0;

    public override void OnUnlock(int currentLevel)
    {
        onSkillUnlock?.Invoke(this);
    }

    public override void ActivateEffect(PlayerDemo player)
    {
        player.IncreaseInt(valueToIncrease);
    }

    public override string GetEffect()
    {
        return "Intelligence + " + valueToIncrease.ToString();
    }
}
