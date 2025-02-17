using UnityEngine;

public class CharacterAnimate : MonoBehaviour
{
    #region References
    [HideInInspector] public Character Character;
    #endregion

    public void Animate()
    {
        // string weaponName = weapon.GetType().Name;
        // if (weaponName == null) weaponName = "";
        // string animationName = weaponName + state;
        // //Debug.Log(animationName);
        // var animationFrame = animationFrames.Find(x => x.name == animationName);
        // if (animationFrame == null) { Debug.Log("No Such Animation"); return; }
        // if (spriteRenderer.sprite == animationFrame.sprite) return;
        // currentAnimationFrame = animationName;
        // spriteRenderer.sprite = animationFrame.sprite;
    }

    void Update()
    {
        Animate();
    }
}
