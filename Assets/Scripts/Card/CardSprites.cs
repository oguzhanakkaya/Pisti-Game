using System;
using UnityEngine;

[Serializable]
public class CardSprites
{
    [Header("The card suit and number. 0:S, 1:H, 2:C, 3:D")]
    public int suit;
    public Sprite jackCardSprites;
    public Sprite queenCardSprites;
    public Sprite kingCardSprites;
    public Sprite otherCardSprites;
}
