using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    
    private PlayerHUD hud;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void GetReferences()
    {
        hud = GetComponent<PlayerHUD>();
    }


    public override void CheakHealth()
    {
        base.CheakHealth();
        hud.UpdateHealth(health, maxHealth);
    }
}
