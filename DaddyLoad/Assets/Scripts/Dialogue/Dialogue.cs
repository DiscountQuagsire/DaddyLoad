using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable][CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public string characterName;

    public Sprite sprite;

    [TextArea (3,10)]
    public string[] sentences;

}
