#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Events;

namespace cowsins
{
    public class EnemyAI : MonoBehaviour, IDamageable
    {
        [System.Serializable]
        public class Events
        {
            public UnityEvent OnSpawn, OnShoot, OnDamaged, OnDeath;
        }

        [Tooltip("Name of the enemy. This will appear on the killfeed"), SerializeField]
        public string _name;

        public float health;

        [Tooltip("initial enemy health "), SerializeField]
        public float maxHealth;

        public float shield;

        [Tooltip("initial enemy shield"), SerializeField]
        public float maxShield;

        [Tooltip("display enemy status via UI"), SerializeField]
        protected Slider healthSlider, shieldSlider;

        [Tooltip(
             "If true, it will display the UI with the shield and health sliders, disabling this will disable pop ups."),
         SerializeField]
        public bool showUI;

        [Tooltip(
             "Add a pop up showing the damage that has been dealt. Recommendation: use the already made pop up included in this package. "),
         SerializeField]
        private GameObject damagePopUp;

        [Tooltip("Colour for the specific status to be displayed in the slider"), SerializeField]
        private Color shieldColor, healthColor;

        [Tooltip("Horizontal randomness variation"), SerializeField]
        private float xVariation;

        [HideInInspector] public Transform player;
        protected Transform UI;

        public Events events;

        private CowsinsAI cai;
        private Animator animator;
        private NavMeshAgent agent;


        // Start is called before the first frame update"
        public virtual void Start()
        {
            // Status initial settings
            health = maxHealth;
            shield = maxShield;

            // Spawn
            events.OnSpawn.Invoke();

            // Initial settings 
            player = GameObject.FindGameObjectWithTag("Player").transform;
            cai = GetComponent<CowsinsAI>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();


            // UI 
            // Determine max values
            if (healthSlider != null) healthSlider.maxValue = maxHealth;
            if (shieldSlider != null) shieldSlider.maxValue = maxShield;
            if (!showUI) // Destroy the enemy UI if we do not want to display it
            {
                Destroy(healthSlider);
                Destroy(shieldSlider);
            }

            player = GameObject.FindGameObjectWithTag("Player").transform;
            
            // Rigidbody Search
            if (cai.useRagdoll)
            {
                animator = gameObject.GetComponent<Animator>();
                agent = gameObject.GetComponent<NavMeshAgent>();
                Rigidbody[] rigidBodies = gameObject.GetComponentsInChildren<Rigidbody>();

                foreach (Rigidbody rigidBody in rigidBodies)
                { 
                    rigidBody.isKinematic = true;
                }
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {
            //Handle UI 
            if (healthSlider != null) healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * 6);
            if (shieldSlider != null) shieldSlider.value = Mathf.Lerp(shieldSlider.value, shield, Time.deltaTime * 4);

            // Manage health
            if (health <= 0) Die();
        }

        /// <summary>
        /// Since it is IDamageable, it can take damage, if a shot is landed, damage the enemy
        /// </summary>
        public virtual void Damage(float _damage)
        {
            float damage = Mathf.Abs(_damage);
            float oldDmg = damage;
            if (damage <= shield) // Shield will be damaged
            {
                shield -= damage;
                if (shieldSlider != null) shieldSlider.GetComponent<Animator>().Play("UIDamageShieldEnemy");
            }
            else
            {
                damage = damage - shield;
                shield = 0;
                health -= damage;
                if (shieldSlider != null && health >= 0) shieldSlider.GetComponent<Animator>().Play("UIDamageShieldEnemy");
                if (shieldSlider != null && shield >= 0) healthSlider.GetComponent<Animator>().Play("UIDamageEnemy");
            }

            // Custom event on damaged
            events.OnDamaged.Invoke();
            UIEvents.onEnemyHit.Invoke();
            // If you do not want to show a damage pop up, stop, do not continue
            if (!showUI) return;
            GameObject popup = Instantiate(damagePopUp, transform.position, Quaternion.identity) as GameObject;
            if (oldDmg / Mathf.FloorToInt(oldDmg) == 1)
                popup.transform.GetChild(0).GetComponent<TMP_Text>().text = oldDmg.ToString("F0");
            else
                popup.transform.GetChild(0).GetComponent<TMP_Text>().text = oldDmg.ToString("F1");
            float xRand = Random.Range(-xVariation, xVariation);
            popup.transform.position = popup.transform.position + new Vector3(xRand, 0, 0);
        }

        public virtual void Die()
        {
            // Custom event on damaged
            events.OnDeath.Invoke();

            // Does it display killfeed on death? 
            UIEvents.onEnemyKilled.Invoke(_name);

            // Custom Inputs
            if (cai.useRagdoll)
            {
                RagdollDeath();
            }

            if (cai.useDeathAnimation)
            {
                AnimationDeath();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void RagdollDeath()
        {
            Rigidbody[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
            
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
            }

            cai.enabled = false;

            animator.enabled = false;

            agent.enabled = false;
        }

        void AnimationDeath()
        {
            cai.enabled = false;
            agent.enabled = false;

            if (cai._enemyType == CowsinsAI.EnemyType.Melee)
            {
                if (showUI)
                {
                    healthSlider.gameObject.SetActive(false);
                    shieldSlider.gameObject.SetActive(false);   
                }
                GetComponent<Collider>().enabled = false;
                
                animator.SetBool("isWalking", false);
                animator.SetBool("attacking", false);
                animator.SetBool("dead", true);
            }

            if (cai._enemyType == CowsinsAI.EnemyType.Shooter)
            {
                if (showUI)
                {
                    healthSlider.gameObject.SetActive(false);
                    shieldSlider.gameObject.SetActive(false);
                }

                GetComponent<Collider>().enabled = false;
                animator.SetBool("isWalking", false);
                animator.SetBool("combatWalk", false);
                animator.SetBool("combatIdle", false);
                animator.SetBool("firing", false);
                animator.SetBool("dead", true);
            }
        }

        void AnimationDeathDeactivateAnimator()
        {
            animator.enabled = false;

            if (cai.destroyAfterTime)
            {
                StartCoroutine(DestroyPlayer(cai.destroyTimer));
            }
        }

        IEnumerator DestroyPlayer(float destroyAfterTime)
        {
            float timer = destroyAfterTime;

            while (timer > 0f)
            {
                yield return new WaitForSeconds(1f);
                timer--;
            }
            
            Destroy(gameObject);
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
    [CustomEditor(typeof(EnemyAI))]
    public class EnemyEditorAI : Editor
    {

        override public void OnInspectorGUI()
        {
            serializedObject.Update();
            EnemyAI myScript = target as EnemyAI;

            EditorGUILayout.LabelField("IDENTITY", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("STATS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHealth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxShield"));

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showUI"));
            if (myScript.showUI)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("healthSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shieldSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("healthColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shieldColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damagePopUp"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("xVariation"));
            }

            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}