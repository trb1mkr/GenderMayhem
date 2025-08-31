using UnityEngine;
using UnityEngine.Events;
using GenderMayhem.Actions;
using System.Collections;

public class AIWeaponController : MonoBehaviour
{
    #region Values
    public float MeleeAttackDelay;
    [HideInInspector] public UnityEvent Used, UseCanceled, AltUsed;
    private Coroutine _tryAttackCoroutine;
    #endregion

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        Used.AddListener(AI.Agent.StateController.OnUsed);

        AI.Detection.TargetGameObjectDetected += (targetType) => { if (targetType == AITarget.VisionPlayer) _tryAttackCoroutine = StartCoroutine(TryAttack()); if (targetType == AITarget.VisionWeapon) StartCoroutine(TryPickUp());};
        AI.Detection.TargetGameObjectLost += () => { if (_tryAttackCoroutine != null) StopCoroutine(_tryAttackCoroutine); };
        AI.Detection.TargetGameObjectLost += CancelAttack;

        AI.Agent.ItemManager.ItemPickedUp += () => { if (AI.Agent.ItemManager.Item is Gun gun)
            gun.AmmoIsOut += () => Reload(); };
        //неправильно
        //AI.Agent.ItemManager.ItemThrowed += () => { if (AI.Agent.ItemManager.Item is Gun gun) gun.AmmoIsOut -= () => AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(WeaponAction.Reload); };

        if (AI.Agent.ItemManager.Item is Gun gun)
            gun.AmmoIsOut += () => Reload();
    }

    private void Reload() =>
        AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(WeaponAction.Reload);

    private IEnumerator TryPickUp()
    {
        while (true)
        {
            AI.Agent.ItemManager.PickUp();
            if (AI.Agent.ItemManager.Item is not Fists) yield break;
            yield return null;
        }
    }

    private IEnumerator TryAttack()
    {
        while (true && AI.Agent.ItemManager.Item)
        {
            if (AI.Agent.ItemManager.Item is Gun && AI.Rotation.IsAimed)
                Attack();
            if (AI.Agent.ItemManager.Item is Melee && //ошибка
                Vector3.Distance(transform.position, AI.Detection.TargetGameObject.transform.position) < AI.NavMeshAgent.stoppingDistance + 5)
            {
                yield return new WaitForSeconds(MeleeAttackDelay);
                Attack();
            }
            yield return null;
        }
    }

    private void Attack()
    {
        if (AI.Agent.WeaponController.IsCooldown) return;
        if (AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(ItemAction.Use))
            Used.Invoke();
    }

    private void CancelAttack()
    {
        if (_tryAttackCoroutine != null) StopCoroutine(_tryAttackCoroutine);
        if (AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(ItemAction.UseCanceled))
            UseCanceled.Invoke();
    }
}
