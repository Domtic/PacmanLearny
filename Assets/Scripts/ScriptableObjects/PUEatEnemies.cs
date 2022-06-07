using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "new PU", menuName = "PowerUps/PU EatEnemies")]
public class PUEatEnemies : ScriptableObject
{
    [SerializeField]
    private float timeToEat;
    [SerializeField]
    private uint scoreOnEated;

    [Header("On eated configuration")]
    [SerializeField]
    private Ease textAnimation;
    [SerializeField]
    private TextDir textDirection;

    public Ease TextAnimation { get => textAnimation; }
    public TextDir TextDirection { get => textDirection; }
    public uint ScoreOnEated { get => scoreOnEated; }
    public float TimeToEat { get => timeToEat; }
}
