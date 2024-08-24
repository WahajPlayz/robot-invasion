using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.AI
{
    public class EnemyManager : MonoBehaviour
    {
        public List<EnemyAI> Enemies { get; private set; }
        public int NumberOfEnemiesTotal { get; private set; }
        public int NumberOfEnemiesRemaining => Enemies.Count;
        public List<cowsins.EnemyHealth> TargetEnemies { get; private set; }

        void Awake()
        {
            Enemies = new List<EnemyAI>();
            TargetEnemies = new List<cowsins.EnemyHealth>();
        }

        public void RegisterEnemy(EnemyAI enemy)
        {
            if (!Enemies.Contains(enemy))
                Enemies.Add(enemy);
            else
                Debug.LogWarning("The Enemy Manager already contain this enemy in it's list");

            NumberOfEnemiesTotal++;
        }
        
        public void RegisterTarget(cowsins.EnemyHealth Target)
        {
            if (!TargetEnemies.Contains(Target))
                TargetEnemies.Add(Target);
            else
                Debug.LogWarning("The Enemy Manager already contain this enemy in it's list");

            NumberOfEnemiesTotal++;
        }
        public void UnregisterTarget(cowsins.EnemyHealth TargetKilled)
        {
            if (TargetEnemies.Contains(TargetKilled))
            {
                int enemiesRemainingNotification = NumberOfEnemiesRemaining - 1;

                EnemyKillEvent evt = Unity.FPS.Game.Events.EnemyKillEvent;
                evt.Enemy = TargetKilled.gameObject;
                evt.RemainingEnemyCount = enemiesRemainingNotification;
                EventManager.Broadcast(evt);

                // removes the enemy from the list, so that we can keep track of how many are left on the map
                TargetEnemies.Remove(TargetKilled);
            }
        }

        public void UnregisterEnemy(EnemyAI enemyKilled)
        {
            if (Enemies.Contains(enemyKilled))
            {
                int enemiesRemainingNotification = NumberOfEnemiesRemaining - 1;

                EnemyKillEvent evt = Unity.FPS.Game.Events.EnemyKillEvent;
                evt.Enemy = enemyKilled.gameObject;
                evt.RemainingEnemyCount = enemiesRemainingNotification;
                EventManager.Broadcast(evt);

                // removes the enemy from the list, so that we can keep track of how many are left on the map
                Enemies.Remove(enemyKilled);
            }
            else
                Debug.LogWarning("The enemy you want to unregister is not present in the enemy list, make sure to register it before");
        }
    }
}
