using UnityEngine;
using UnityEngine.Events;
using GenderMayhem.Actions;
using System.Collections;

public class AIWeaponController : MonoBehaviour
{
    [HideInInspector] public UnityEvent Used, UseCanceled, AltUsed;

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion

    private void Start()
    {
        Used.AddListener(AI.Agent.StateController.HandleUsed);

        AI.Detection.TargetGameObjectDetected += () => StartCoroutine(TryAttack());
        AI.Detection.TargetGameObjectLost += () => StopCoroutine(TryAttack());
        AI.Detection.TargetGameObjectLost += CancelAttack;

        AI.Agent.ItemManager.ItemPickedUp += () => { if (AI.Agent.ItemManager.Item is Gun gun) gun.AmmoIsOut += () => AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(WeaponAction.Reload); };
        //неправильно
        //AI.Agent.ItemManager.ItemThrowed += () => { if (AI.Agent.ItemManager.Item is Gun gun) gun.AmmoIsOut -= () => AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(WeaponAction.Reload); };

        if (AI.Agent.ItemManager.Item is Gun gun) gun.AmmoIsOut += () => AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(WeaponAction.Reload);
    }

    private IEnumerator TryAttack()
    {
        while (true && AI.Agent.ItemManager.Item)
        {
            if (AI.Agent.ItemManager.Item is Gun && AI.Rotation.IsAimed)
                Attack();
            if (AI.Agent.ItemManager.Item is Melee &&
                Vector3.Distance(transform.position, AI.Detection.TargetGameObject.transform.position) < AI.NavMeshAgent.stoppingDistance + 5)
            {
                yield return new WaitForSeconds(0.1f);
                Debug.Log("Melee use");
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
        StopCoroutine(TryAttack());
        if (AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(ItemAction.UseCanceled))
            UseCanceled.Invoke();
    }
}
