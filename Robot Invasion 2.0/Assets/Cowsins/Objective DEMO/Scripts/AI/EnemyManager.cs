using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.AI
{
    public class EnemyManager : MonoBehaviour
    {
        public List<cowsins.EnemyHealth> Enemies { get; private set; }
        public int NumberOfEnemiesTotal { get; private set; }
        public int NumberOfEnemiesRemaining => Enemies.Count;

        void Awake()
        {
            Enemies = new List<cowsins.EnemyHealth>();
        }

        public void RegisterEnemy(cowsins.EnemyHealth enemy)
        {
            Enemies.Add(enemy);

            NumberOfEnemiesTotal++;
        }

        public void UnregisterEnemy(cowsins.EnemyHealth enemyKilled)
        {
            int enemiesRemainingNotification = NumberOfEnemiesRemaining - 1;

            EnemyKillEvent evt = Unity.FPS.Game.Events.EnemyKillEvent;
            evt.Enemy = enemyKilled.gameObject;
            evt.RemainingEnemyCount = enemiesRemainingNotification;
            EventManager.Broadcast(evt);

            // removes the enemy from the list, so that we can keep track of how many are left on the map
            Enemies.Remove(enemyKilled);
        }
    }
}