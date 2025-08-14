using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceEmitter : MonoBehaviour
{
    [ReadOnly] private SoundEmitData _currentEmitData;
    [SerializeField] private bool _debugDrawRadius = true;

    public void NotifyListeners(SoundEmitData soundEmitData)
    {
        _currentEmitData = soundEmitData;
        // Находим все коллайдеры в радиусе
        Collider2D[] colliders  = Physics2D.OverlapCircleAll(transform.position, soundEmitData.SoundRadius, soundEmitData.ListenerMask);

        // Перебираем найденные объекты
        for (int i = 0; i < colliders.Length; i++)
        {
            var listener = colliders[i].GetComponentInParent<IAudioSourceListener>();
            listener?.OnSoundDetected(gameObject, soundEmitData.SoundType);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_currentEmitData == null) return;
        if (_debugDrawRadius)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _currentEmitData.SoundRadius);
        }
    }
}