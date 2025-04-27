using System.Collections;
using UnityEngine;

[System.Serializable]
abstract public class Weapon : Item, IUsable
{
    #region Values
    public AudioClip AttackSound;
    public AudioClip PickUpSound;
    #endregion

    public virtual IEnumerator Use()
    {
        Attack();
        yield return null;
        //yield return new WaitForSeconds(UsageTime);
    }

    virtual public void Attack() 
        { GetComponent<CameraShakeSource>().Shake(); }

    virtual public void AltAttack() { return; }


}