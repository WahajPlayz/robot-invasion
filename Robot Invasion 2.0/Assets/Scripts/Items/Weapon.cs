using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Items/Weapon")]
public class Weapon : Item
{
    public GameObject prefab;
    public int MagazineSize;
    public int MagazineCount;
    public float range;
    public WeaponType weaponType;
    public WeaponStyle weaponStyle;
}

public enum WeaponType {Melee, EnergyPistol, EnergyAR, EnergyShotgun, EnergySniper}
public enum WeaponStyle { Primary, Secondary, Melee}

