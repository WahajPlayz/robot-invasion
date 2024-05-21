using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(CowsinsAI))]
public class CowsinsAIEditor : Editor
{
    private string[] _combatTabs = {"Variables", "Combat", "Debug"};
    private int _combatTab;

    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        CowsinsAI cai = target as CowsinsAI;

        GUILayout.BeginHorizontal();
        Texture2D myTexture = Resources.Load<Texture2D>("CustomEditor/Cowsins AI Logo");
        GUILayout.Label(myTexture, GUILayout.Width(150), GUILayout.Height(150));
        GUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Cowsins AI", EditorStyles.whiteLargeLabel);
        EditorGUILayout.Space(5);
        _combatTab = GUILayout.Toolbar(_combatTab, _combatTabs);
        EditorGUILayout.Space(10f);
        EditorGUILayout.EndVertical();

        if (_combatTab >= 0 || _combatTab < _combatTabs.Length)
        {
            switch (_combatTabs[_combatTab])
            {
                case "Variables":
                    EditorGUILayout.Space(5);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("BASIC VARIABLES", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentState"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("aiType"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("useRagdoll"));
                    
                    if (!cai.useRagdoll)
                    {
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("useDeathAnimation"));
                        EditorGUILayout.Space(5);
                        if (cai.useDeathAnimation)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyAfterTime"));
                            EditorGUILayout.Space(5);
                            if (cai.destroyAfterTime)
                            {
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyTimer"));
                            }   
                        }
                    }
                    
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("dumbAI"));
                    EditorGUILayout.Space(5);

                    EditorGUI.indentLevel++;
                    if (!cai.dumbAI)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("moveMode"));
                        EditorGUILayout.Space(5);

                        if (cai.moveMode == CowsinsAI.MoveMode.Waypoints)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_waypoints"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("waypointWaitTime"));
                            EditorGUILayout.Space(5);
                        }
                        else if (cai.moveMode == CowsinsAI.MoveMode.Random)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("wanderRadius"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("minWanderDistance"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxWanderDistance"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("waitTimeBetweenWander"));
                            EditorGUILayout.Space(5);
                        }
                    }
                    else
                    {
                        return;
                    }

                    EditorGUI.indentLevel--;

                    if (cai.aiType == CowsinsAI.AIType.NPC)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("NPCAnimator"));
                    }

                    if (cai.aiType == CowsinsAI.AIType.Enemy)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("searchRadius"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("increaseSightOnAttack"));
                        EditorGUILayout.Space(5);
                        if (cai.increaseSightOnAttack)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSearchAngle"));
                            EditorGUILayout.Space(5);
                        }

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("searchAngle"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("obstructionMask"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("searchWaitTime"));
                        EditorGUILayout.Space(5);
                    }

                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space(10);
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Save Settings"))
                    {
                        SaveSettings();
                    }
                    break;
                case "Combat":
                    if (cai.aiType == CowsinsAI.AIType.Enemy)
                    {
                        EditorGUILayout.LabelField("COMBAT OPTIONS", EditorStyles.boldLabel);
                        EditorGUILayout.LabelField(
                            "Note: Both Shooter AND Melee cannot be enabled at the same time or there will be an error!",
                            EditorStyles.helpBox);
                        EditorGUILayout.Space(10);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_enemyType"));
                        if (cai._enemyType == CowsinsAI.EnemyType.Shooter)
                        {
                            EditorGUILayout.LabelField("SHOOTER OPTIONS", EditorStyles.boldLabel);
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
                            EditorGUILayout.Space(5);
                            if (cai.type == CowsinsAI.ShooterType.Projectile)
                            {
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("projectile"));
                                EditorGUILayout.Space(10);
                            }
                            else if (cai.type == CowsinsAI.ShooterType.Hitscan)
                            {
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletTrail"));
                                EditorGUILayout.Space(5);
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("spreadAmount"));
                                EditorGUILayout.Space(5);
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("hitMask"));
                                EditorGUILayout.Space(10);
                            }

                            EditorGUILayout.LabelField("UNIVERSAL COMBAT VARIABLES", EditorStyles.boldLabel);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("firePoint"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("shooterAnimator"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("shootDistance"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("inShootingDistance"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeBetweenShots"));
                            EditorGUILayout.Space(5);
                            EditorGUI.indentLevel--;
                        }
                        
                        if (cai._enemyType == CowsinsAI.EnemyType.Melee)
                        {
                            EditorGUILayout.Space(5);
                            EditorGUILayout.LabelField("MELEE OPTIONS", EditorStyles.boldLabel);
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeDistance"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeAnimator"));
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("waitBetweenAttack"));
                            EditorGUILayout.Space(5);
                            EditorGUI.indentLevel--;
                        }

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageAmount"));
                        EditorGUILayout.Space(5);
                        
                        EditorGUILayout.Space(5);
                    }
                    else
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField(
                            "Shooter Tab Disabled Due to AI Type Settings!",
                            EditorStyles.helpBox);
                    }

                    EditorGUILayout.Space(10);
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Save Settings"))
                    {
                        SaveSettings();
                    }
                    
                    break;

                case "Debug":
                    EditorGUILayout.LabelField("DEBUG VARIABLES", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("shootingDistanceDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeDistanceDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("canSeePlayerDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("searchRadiusDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRadiusDebug"));
                    EditorGUILayout.Space(10);
                    
                    EditorGUILayout.LabelField("RUNTIME DEBUG VARIABLES", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("canSeePlayer"));

                    EditorGUILayout.Space(10);
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Save Settings"))
                    {
                        SaveSettings();
                    }
                    
                    break;
                
                    void SaveSettings()
                    {
                        SettingSaveSO saveSO = CreateInstance<SettingSaveSO>();

                        if (cai.aiType == CowsinsAI.AIType.Enemy)
                        {
                            saveSO.aiType = SettingSaveSO.AIType.Enemy;
                        }

                        if (cai.aiType == CowsinsAI.AIType.NPC)
                        {
                            saveSO.aiType = SettingSaveSO.AIType.NPC;
                        }

                        saveSO.useRagdoll = cai.useRagdoll;

                        saveSO.dumbAI = cai.dumbAI;

                        saveSO.searchRadius = cai.searchRadius;

                        saveSO.increaseSightOnAttack = cai.increaseSightOnAttack;

                        saveSO.attackSearchAngle = cai.attackSearchAngle;

                        saveSO.searchAngle = cai.searchAngle;

                        saveSO.targetMask = cai.targetMask;

                        saveSO.obstructionMask = cai.obstructionMask;

                        saveSO.searchWaitTime = cai.searchWaitTime;
                        
                        AssetDatabase.CreateAsset(saveSO, "Assets/CowsinsAIData.asset");
                        AssetDatabase.SaveAssets();
                        
                        EditorUtility.FocusProjectWindow();

                        Selection.activeObject = saveSO;
                    }

            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        CowsinsAI cai = (CowsinsAI) target;

        if (cai.aiType == CowsinsAI.AIType.Enemy)
        {
            if (cai.searchRadiusDebug)
            {
                Handles.color = Color.white;
                Handles.DrawWireArc(cai.transform.position, Vector3.up, Vector3.forward, 360, cai.searchRadius);

                Vector3 searchViewAngle01 = DirectionFromAngle(cai.transform.eulerAngles.y, -cai.searchAngle / 2);
                Vector3 searchViewAngle02 = DirectionFromAngle(cai.transform.eulerAngles.y, cai.searchAngle / 2);

                Handles.color = Color.yellow;
                Handles.DrawLine(cai.transform.position, cai.transform.position + searchViewAngle01 * cai.searchRadius);
                Handles.DrawLine(cai.transform.position, cai.transform.position + searchViewAngle02 * cai.searchRadius);
            }

            if (cai.attackRadiusDebug)
            {
                Vector3 attackViewAngle01 = DirectionFromAngle(cai.transform.eulerAngles.y, -cai.searchAngle / 2);
                Vector3 attackViewAngle02 = DirectionFromAngle(cai.transform.eulerAngles.y, cai.searchAngle / 2);
            }

            if (cai.canSeePlayerDebug)
            {
                if (cai.canSeePlayer)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(cai.transform.position, cai.player.transform.position);
                }
            }

            if (cai.shootingDistanceDebug)
            {
                Handles.color = Color.red;
                Handles.DrawWireArc(cai.transform.position, Vector3.up, Vector3.forward, 360, cai.shootDistance);

                if (cai.inShootingDistance)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(cai.transform.position, cai.player.transform.position);
                }
            }

            if (cai.meleeDistanceDebug)
            {
                if (cai.inMeleeDistance)
                {
                    Handles.color = Color.red;
                    Handles.DrawWireArc(cai.transform.position, Vector3.up, Vector3.forward, 360, cai.meleeDistance);
                    Handles.DrawLine(cai.transform.position, cai.player.transform.position);
                }
            }
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
#endif