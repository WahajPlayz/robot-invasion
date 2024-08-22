using UnityEngine;
using cowsins;
using UnityEditor;
public class UIChangeWeapon : MonoBehaviour
{
    [SerializeField] private Weapon_SO weapon;
    [SerializeField] private WeaponController weaponController;

    private AttachmentIdentifier_SO Barrel,
            Scope,
            Stock,
            Grip,
            Magazine,
            Flashlight,
            Laser;

    [HideInInspector] public int currentBullets, totalBullets;
    public void Interact()
    {
        currentBullets = weapon.magazineSize + SetDefaultAttachments();
        totalBullets = weapon.totalMagazines * currentBullets;
        if (!CheckIfInventoryFull(weaponController))
        {
            
        }

        SwapWeapons(weaponController);
    }

    private bool CheckIfInventoryFull(WeaponController weaponController)
    {
        for (int i = 0; i < weaponController.inventorySize; i++)
        {
            if (weaponController.inventory[i] == null) // Inventory has room for a new weapon.
            {
                AddWeaponToInventory(weaponController, i);
                return false;
            }
        }
        // Inventory is full
        return true;
    }

    private void AddWeaponToInventory(WeaponController weaponController, int slot)
    {
        var weaponPicked = Instantiate(weapon.weaponObject, weaponController.weaponHolder);
        weaponPicked.transform.localPosition = weapon.weaponObject.transform.localPosition;

        weaponController.inventory[slot] = weaponPicked;

        if (weaponController.currentWeapon == slot)
        {
            weaponController.weapon = weapon;
            ApplyAttachments(weaponController);
            weaponController.UnHolster(weaponPicked.gameObject, true);
            weaponPicked.gameObject.SetActive(true);
        }
        else
        {
            weaponPicked.gameObject.SetActive(false);
        }

        UpdateWeaponBullets(weaponController.inventory[slot].GetComponent<WeaponIdentification>());

        UpdateWeaponUI(weaponController, slot);

#if UNITY_EDITOR
        UpdateCrosshair(weaponController);
#endif
    }

    private void SwapWeapons(WeaponController weaponController)
    {
        Weapon_SO oldWeapon = weaponController.weapon;
        int savedBulletsLeftInMagazine = weaponController.id.bulletsLeftInMagazine;
        int savedTotalBullets = weaponController.id.totalBullets;
        weaponController.ReleaseCurrentWeapon();

        AddWeaponToInventory(weaponController, weaponController.currentWeapon);

        weaponController.weapon = weapon;

        UpdateWeaponBullets(weaponController.inventory[weaponController.currentWeapon].GetComponent<WeaponIdentification>());

        UpdateWeaponUI(weaponController, weaponController.currentWeapon);

        currentBullets = savedBulletsLeftInMagazine;
        totalBullets = savedTotalBullets;

        weapon = oldWeapon;
    }

    private void UpdateWeaponBullets(WeaponIdentification weaponIdentification)
    {
        weaponIdentification.bulletsLeftInMagazine = currentBullets;
        weaponIdentification.totalBullets = totalBullets;
    }

    private void UpdateWeaponUI(WeaponController weaponController, int slot)
    {
        weaponController.slots[slot].weapon = weapon;
        weaponController.slots[slot].GetImage();
    }

#if UNITY_EDITOR
    private void UpdateCrosshair(WeaponController weaponController)
    {
        if (weaponController.weapon != null)
        {
            var crosshairShape = UIController.instance.crosshair.GetComponent<CrosshairShape>();
            crosshairShape.currentPreset = weaponController.weapon.crosshairPreset;
            CowsinsUtilities.ApplyPreset(crosshairShape.currentPreset, crosshairShape);
        }
    }
#endif


    public void Drop(WeaponController wcon, Transform orientation)
    {
        currentBullets = wcon.id.bulletsLeftInMagazine;
        totalBullets = wcon.id.totalBullets;
        weapon = wcon.weapon;
    }

    // Applied the default attachments to the weapon
    private int SetDefaultAttachments()
    {
        DefaultAttachment defaultAttachments = weapon.weaponObject.defaultAttachments;
        Barrel = defaultAttachments.defaultBarrel?.attachmentIdentifier;
        Scope = defaultAttachments.defaultScope?.attachmentIdentifier;
        Stock = defaultAttachments.defaultStock?.attachmentIdentifier;
        Grip = defaultAttachments.defaultGrip?.attachmentIdentifier;
        Flashlight = defaultAttachments.defaultFlashlight?.attachmentIdentifier;
        Magazine = defaultAttachments.defaultMagazine?.attachmentIdentifier;
        Laser = defaultAttachments.defaultLaser?.attachmentIdentifier;

        if (defaultAttachments.defaultMagazine is Magazine magazine)
        {
            return magazine.magazineCapacityAdded;
        }

        return 0;
    }
    /// <summary>
    /// Stores the attachments on the WeaponPickeable so they can be accessed later in case the weapon is picked up.
    /// </summary>
    public void SetPickeableAttachments(Attachment b, Attachment sc, Attachment st, Attachment gr, Attachment mag, Attachment fl, Attachment ls)
    {
        Barrel = b?.attachmentIdentifier;
        Scope = sc?.attachmentIdentifier;
        Stock = st?.attachmentIdentifier;
        Grip = gr?.attachmentIdentifier;
        Magazine = mag?.attachmentIdentifier;
        Flashlight = fl?.attachmentIdentifier;
        Laser = ls?.attachmentIdentifier;
    }
    // Equips all the appropriate attachyments on pick up
    public void ApplyAttachments(WeaponController weaponController)
    {
        WeaponIdentification wp = weaponController.inventory[weaponController.currentWeapon];

        var attachments = new[] { Barrel, Scope, Stock, Grip, Magazine, Flashlight, Laser };
        foreach (var attachment in attachments)
        {
            (Attachment atc, int id) = GetAttachmentID(attachment, wp);
            weaponController.AssignNewAttachment(atc, id);
        }
    }

    /// <summary>
    /// Grabs the attachment object and the id given an attachment identifier
    /// </summary>
    /// <param name="atcToCheck">Attachment object to get information about returned.</param>
    /// <param name="wID">Weapon Identification that holds the attachments</param>
    /// <returns></returns>
    private (Attachment, int) GetAttachmentID(AttachmentIdentifier_SO atcToCheck, WeaponIdentification wID)
    {
        // Check for compatibility
        for (int i = 0; i < wID.compatibleAttachments.barrels.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.barrels[i].attachmentIdentifier) return (wID.compatibleAttachments.barrels[i], i);
        }
        for (int i = 0; i < wID.compatibleAttachments.scopes.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.scopes[i].attachmentIdentifier) return (wID.compatibleAttachments.scopes[i], i);
        }
        for (int i = 0; i < wID.compatibleAttachments.stocks.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.stocks[i].attachmentIdentifier) return (wID.compatibleAttachments.stocks[i], i);
        }
        for (int i = 0; i < wID.compatibleAttachments.grips.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.grips[i].attachmentIdentifier) return (wID.compatibleAttachments.grips[i], i);
        }
        for (int i = 0; i < wID.compatibleAttachments.magazines.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.magazines[i].attachmentIdentifier) return (wID.compatibleAttachments.magazines[i], i);
        }
        for (int i = 0; i < wID.compatibleAttachments.flashlights.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.flashlights[i].attachmentIdentifier) return (wID.compatibleAttachments.flashlights[i], i);
        }
        for (int i = 0; i < wID.compatibleAttachments.lasers.Length; i++)
        {
            if (atcToCheck == wID.compatibleAttachments.lasers[i].attachmentIdentifier) return (wID.compatibleAttachments.lasers[i], i);
        }

        // Return an error
        return (null, -1);
    }
}