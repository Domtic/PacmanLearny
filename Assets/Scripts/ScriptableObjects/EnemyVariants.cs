using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new enemy bank", menuName = "Enemies/new bank")]
public class EnemyVariants : ScriptableObject
{
    [SerializeField]
    private List<EnemiesStats> variants;

    public List<EnemiesStats> Variants { get => variants; }
}
