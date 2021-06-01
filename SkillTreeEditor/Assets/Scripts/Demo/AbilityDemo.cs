using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityDemo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilities;

    private void Awake()
    {
        PlayerDemo.OnAbilitiesUpdated += UpdateAbilities;
    }

    private void UpdateAbilities(List<SkillBase> skills) 
    {
        abilities.text = "";
        foreach(var skill in skills)
        {
            abilities.text += skill.name + "\n";
        }
    }
}
