using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName ="Player/new Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player stats")]
    [SerializeField]
    private float movementSpeed;

    [Header("Player play Data")]
    [SerializeField]
    private uint highScore;
    [SerializeField]
    private uint enemiesEaten;
    [SerializeField]
    private uint dotsEaten;
    [SerializeField]
    private uint powerUpsEaten;

    [SerializeField]
    private uint[] ScoreToLevelUp = new uint[10];
    [SerializeField]
    private uint playerLevel =1;
    [SerializeField]
    private uint extraMovementSpeedPerLevel = 2;
    private void IncreaseMovementSpeed(float value)
    {
        movementSpeed += value;
    }

    public float MovementSpeed { get => movementSpeed; }
    public uint HighScore { get => highScore; set {
            highScore = value;
            if (playerLevel >= ScoreToLevelUp.Length)
                return;

            if (highScore > ScoreToLevelUp[playerLevel])
            {
                IncreaseMovementSpeed(extraMovementSpeedPerLevel);
                playerLevel++;

            } 
        }  
    }
    public uint EnemiesEaten { get => enemiesEaten; set => enemiesEaten = value; }
    public uint DotsEaten { get => dotsEaten; set => dotsEaten = value; }
    public uint PowerUpsEaten { get => powerUpsEaten; set => powerUpsEaten = value; }
}
