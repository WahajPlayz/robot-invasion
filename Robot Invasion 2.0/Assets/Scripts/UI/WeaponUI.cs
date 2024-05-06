using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text magazineSizeText;
    [SerializeField] private Text magazineCountText;

    public void UpdateInfo(Sprite weaponIcon, int magazineSize, int MagazineCount)
    {
       icon.sprite = weaponIcon;
       magazineSizeText.text = magazineSize.ToString();
       int magazineCountAmount = magazineSize * MagazineCount;
       magazineCountText.text = magazineCountAmount.ToString();
    }
}
