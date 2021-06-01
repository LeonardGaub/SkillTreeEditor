using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeLines : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public bool mirrorZ = true;
    
    public void Setup(Vector2 startPos, Vector2 endPos)
    {
        Strech(gameObject, startPos, endPos, mirrorZ);
    }


    public void Strech(GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ)
    {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        
        _sprite.GetComponent<RectTransform>().localPosition = centerPos;
        _sprite.transform.right = direction;

        if (_mirrorZ) { _sprite.transform.right *= -1f; }
        Vector3 scale = new Vector3(1, 1, 1);
        GetComponent<RectTransform>().sizeDelta = new Vector2(Vector2.Distance(_initialPosition, _finalPosition), 15);
        _sprite.transform.localScale = scale;
    }
}
