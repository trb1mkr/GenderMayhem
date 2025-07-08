using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class CharacterWeaponController : MonoBehaviour
{
    #region Values
    [ReadOnly] public bool IsCooldown;
    public float CooldownTime;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    #endregion

    private void Start()
    {
        Character.ItemManager.ItemChanged.AddListener(() => IsCooldown = false);
        Character.ItemManager.ItemChanged.AddListener(SubscribeOnAttack);
    }

    void SubscribeOnAttack()
    {
        if (Character.ItemManager.Item is Melee melee)
        {
            melee.Attacked.RemoveListener(StartCooldown);
            melee.Attacked.AddListener(StartCooldown);
            CooldownTime = melee.CooldownTime;
        }
        else CooldownTime = 0;
    }

    void StartCooldown()
    {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        IsCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        IsCooldown = false;
    }
}
