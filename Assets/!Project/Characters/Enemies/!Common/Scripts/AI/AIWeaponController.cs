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

        AI.Navigation.Pursuit.Started += () => StartCoroutine(TryUse());
        AI.Navigation.Pursuit.Ended += CancelUse;

        ((Gun)AI.Agent.ItemManager.Item).AmmoIsOut += () => AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(WeaponAction.Reload); // переписать,чтобы оно подписывалось
    }

    private IEnumerator TryUse()
    {
        while (true)
        {
            if (AI.Rotation.IsAimed)
            {
                if ((Weapon)AI.Agent.ItemManager.Item is Gun)
                    Use();
                if ((Weapon)AI.Agent.ItemManager.Item is Melee &&
                    Vector3.Distance(transform.position, AI.Detection.TargetGameObject.transform.position) + 5 < AI.NavMeshAgent.stoppingDistance)
                    Use();
            }
            yield return null;
        }
    }

    private void Use()
    {
        if (AI.Agent.WeaponController.IsCooldown) return;
        if (AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(ItemAction.Use))
            Used.Invoke();
    }

    private void CancelUse()
    {
        StopAllCoroutines();
        if (AI.Agent.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(ItemAction.UseCanceled))
            UseCanceled.Invoke();
    }
}
