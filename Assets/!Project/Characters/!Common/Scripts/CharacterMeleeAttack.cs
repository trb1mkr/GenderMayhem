using UnityEngine;

public class CharacterMeleeAttack : MonoBehaviour
{
    #region References
    [HideInInspector] public Character Character;
    #endregion

    private void OnTriggerStay2D(Collider2D other)
    {
        var victim = other.GetComponentInParent<Character>();
        if (victim != null && victim.Health.IsAlive && victim.StateController.TorsoCollider == other && Character.StateController.StateId == CharacterStateId.Attack)
        {
            if (Character.ItemManager.Item is Melee melee)
            {
                if (melee.IsLethal) victim.Health.Die(transform.position);
                else victim.Health.FallUnconscious(transform.position);
            }
        }
    }
}
