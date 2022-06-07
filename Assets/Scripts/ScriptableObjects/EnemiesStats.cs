using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "enemyStats", menuName ="Enemies/new enemy")]
public class EnemiesStats : ScriptableObject
{
 
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float walkTimePerDirection;
    [SerializeField]
    private bool canMoveHorizontally;
    [SerializeField]
    private bool canMoveVertically;
    [SerializeField]
    private uint scoreWhenEaten;
    [Header("On eated configuration")]
    [SerializeField]
    private Ease textAnimation;
    [SerializeField]
    private TextDir textDirection;


    public Ease TextAnimation { get => textAnimation; }
    public TextDir TextDirection { get => textDirection; }
    public float WalkTime { get => walkTimePerDirection; }
    public float MovementSpeed { get => movementSpeed; }
    public bool CanMoveHorizontally { get => canMoveHorizontally; }
    public bool CanMoveVertically { get => canMoveVertically; }
    public uint ScoreWhenEaten { get => scoreWhenEaten; }
}
