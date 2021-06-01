using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : ScriptableObject
{
    [SerializeField] protected Texture2D icon;
    [TextArea]
    [SerializeField] protected string description;
    
    public virtual void OnUnlock(int currentLevel) { }
    public virtual Texture2D GetIcon() { return icon; }
    public virtual string GetDescription() { return description; }
    public virtual string GetEffect()
    {
        return "";
    }
}
