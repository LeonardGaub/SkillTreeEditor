using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkillView))]
public class SkillTooltipSpawner : TooltipSpawner
{
    public override bool CanCreateTooltip()
    {
        var item = GetComponent<SkillView>().GetSkill();

        if (!item) return false;

        return true;
    }

    public override void UpdateTooltip(GameObject tooltip)
    {
        var itemTooltip = tooltip.GetComponent<SkillTooltip>();
        if (!itemTooltip) return;

        var item = GetComponent<SkillView>().GetSkill();

        itemTooltip.SetUp(item);
    }
}
