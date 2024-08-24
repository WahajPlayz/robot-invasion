using cowsins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "COWSINS/Reward")]
public class Reward : ScriptableObject
{
    [Tooltip("Set the min and max of coins you want to drop")]
    [SerializeField] Vector2 coinAmount;

    [Tooltip("Set the min and max of cexp you want to drop")]
    [SerializeField] Vector2 expAmount;

    [Tooltip("If you want a sound to be played when you give reward you can assign any clip")]
    [SerializeField] AudioClip rewardSFX;

    public void GiveReward()
    {
        if (coinAmount != Vector2.zero)
        {
            int amountOfCoins = Mathf.RoundToInt(Random.Range(coinAmount.x, coinAmount.y));
            CoinManager.Instance.AddCoins(amountOfCoins);
            UIController.instance.UpdateCoinsPanel();
            UIEvents.onCoinsChange?.Invoke(CoinManager.Instance.coins);
        }

        if (expAmount != Vector2.zero)
        {
            int amountOfExp = Mathf.RoundToInt(Random.Range(expAmount.x, expAmount.y));
            ExperienceManager.instance.AddExperience(amountOfExp);
            UIController.instance.UpdateXPPanel();
            UIController.addXP?.Invoke();
        }

        SoundManager.Instance.PlaySound(rewardSFX, 0, 1, false, 0);
    }
}
