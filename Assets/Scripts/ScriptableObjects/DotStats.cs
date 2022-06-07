using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "dot Data", menuName ="Dots/new Dot")]
public class DotStats :ScriptableObject
{
    [SerializeField]
    private uint scoreOnEated;
    [SerializeField]
    private Color dotColor;

   [Header("On eated configuration")]
    [SerializeField]
    private Ease textAnimation;
    [SerializeField]
    private TextDir textDirection;
   
    public uint ScoreOnEated { get => scoreOnEated; }
    public Color DotColor { get => dotColor; }

    public Ease TextAnimation { get => textAnimation; }
    public TextDir TextDirection { get => textDirection; }
}

public enum TextDir
{
    Right,
    Left,
    Up,
    Down,
    Noone
}