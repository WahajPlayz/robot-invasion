using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Cowsins AI Data", menuName = "COWSINS/COWSINS AI/Saved Data", order = 1)]
public class SettingSaveSO : ScriptableObject
{
    public enum MoveMode
    {
        Waypoints,
        Random,
        Idle
    }

    public enum EnemyType
    {
        Shooter,
        Melee
    }

    public enum Type
    {
        Hitscan,
        Projectile
    }

    public enum AIType
    {
        Enemy,
        NPC
    }

    public AIType aiType;
    
    public bool useRagdoll;
    public bool dumbAI;
    public MoveMode moveMode;

    public float searchRadius;
    public bool increaseSightOnAttack;

    [Range(0, 360)] public float attackSearchAngle;

    [Range(0, 360)] public float searchAngle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public float searchWaitTime;
    
    // COMBAT SETTINGS
    public EnemyType enemyType;

    public Type shooterType;

    public float spreadAmount;

    public LayerMask hitMask;

    public float shootDistance;

    public bool inShootingDistance;

    public float timeBetweenShots;

    public float damageAmount;
}
