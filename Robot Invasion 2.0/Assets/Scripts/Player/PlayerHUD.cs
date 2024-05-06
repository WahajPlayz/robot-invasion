using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private ProgressBar healthbar;
    [SerializeField] private WeaponUI weaponUI;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthbar.Setvalues(currentHealth, maxHealth);
    }


    public void UpdateWeaponUI(Weapon newWeapon)
    {
        weaponUI.UpdateInfo(newWeapon.icon, newWeapon.MagazineSize, newWeapon.MagazineCount);
    }
}