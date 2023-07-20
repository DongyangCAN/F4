using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialogue
{
    [TextArea(2, 2)]
    public string[] sentenes;
    public Sprite[] sprites;
    public Sprite[] dialogueWindows;
}
