using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class GunshopMouseShow : MonoBehaviour
{
    public void show()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}