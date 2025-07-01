using UnityEngine;
using System.Collections;

public static class MonoBehaviourExtensions
{
    public static void Invoke(this MonoBehaviour monoBehaviour, System.Action action, float delay) =>
        monoBehaviour.StartCoroutine(InvokeCoroutine(action, delay));

    private static IEnumerator InvokeCoroutine(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
