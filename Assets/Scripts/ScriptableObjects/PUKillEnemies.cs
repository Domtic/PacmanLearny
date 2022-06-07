using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "new PU", menuName = "PowerUps/PU KillEnemies")]
public class PUKillEnemies : ScriptableObject
{
    [SerializeField]
    private bool killAllEnemies;
    [SerializeField]
    private uint scoreOnEated;

    [Header("Only if killallenemies is false")]
    [SerializeField]
    private int enemiesToKILL;
   

    [Header("On eated configuration")]
    [SerializeField]
    private Ease textAnimation;
    [SerializeField]
    private TextDir textDirection;

    public Ease TextAnimation { get => textAnimation; }
    public TextDir TextDirection { get => textDirection; }
    public bool KillAllEnemies { get => killAllEnemies; }
    public int EnemiesToKill { get => enemiesToKILL; }
    public uint ScoreOnEATED { get => scoreOnEated; }
}
