using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemo : MonoBehaviour
{
    public static PlayerDemo instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public int strength = 0;
    public int dexterity = 0;
    public int intelligence = 0;
    private List<SkillBase> unlockedAbilities = new List<SkillBase>();

    public static Action<int, int, int> OnStatsUpdated;
    public static Action<List<SkillBase>> OnAbilitiesUpdated;

    private void Start()
    {
        OnStatsUpdated?.Invoke(strength, dexterity, intelligence);
        OnAbilitiesUpdated?.Invoke(unlockedAbilities);
    }
    public void IncreaseStr(int value)
    {
        strength += value;
        OnStatsUpdated?.Invoke(strength, dexterity, intelligence);
    }

    public void IncreaseInt(int value)
    {
        intelligence += value;
        OnStatsUpdated?.Invoke(strength, dexterity, intelligence);
    }

    public void IncreaseDex(int value)
    {
        dexterity += value;
        OnStatsUpdated?.Invoke(strength, dexterity, intelligence);
    }

    public void AddAbility(SkillBase newAbility)
    {
        unlockedAbilities.Add(newAbility);
        OnAbilitiesUpdated?.Invoke(unlockedAbilities);
    }

}
