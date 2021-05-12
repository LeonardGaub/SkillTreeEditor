using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    [SerializeField] private Texture2D icon;

    public void onUnlock()
    {

    }
    public Texture2D GetIcon()
    {
        return icon;
    }
}
