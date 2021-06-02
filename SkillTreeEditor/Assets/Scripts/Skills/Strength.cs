using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strength+", menuName = "Skills/Strength")]
public class Strength : SkillBase
{
    [SerializeField] int valueToIncrease = 0;

    public override void OnUnlock(int currentLevel)
    {
        onSkillUnlock?.Invoke(this);
    }

    public override void ActivateEffect(PlayerDemo player)
    {
        player.IncreaseStr(valueToIncrease);
    }

    public override string GetEffect()
    {
        return "Strength + " + valueToIncrease.ToString();
    }
}
