
/// <summary>
/// A list of helpful functions 
/// </summary>
/// 

// recursively set all childrens layers in a game object
using UnityEngine;

public class CODINGTOOLS
{
    public void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);

        }
    }
}

