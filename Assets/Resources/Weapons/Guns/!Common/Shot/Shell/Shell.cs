using UnityEngine;

public class Shell : MonoBehaviour
{
    // Приватные поля для настройки силы и крутящего момента
    [SerializeField] private float _force;
    [SerializeField] private float _forceDeviation; // Отклонение силы
    [SerializeField] private float _torque;
    [SerializeField] private float _torqueDeviation; // Отклонение крутящего момента
    [SerializeField] private float _directionDeviation; // Отклонение направления

    private Rigidbody2D _rb;
    private float _sleepVelocity = 0.1f;

    void Update()
    {
        if (_rb.linearVelocity.magnitude <= _sleepVelocity) 
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore");
            _rb.simulated = false;
        }
    }

    void Start()
    {
        // Получаем компонент Rigidbody2D
        _rb = GetComponent<Rigidbody2D>();

        // Применяем силу и крутящий момент
        ApplyForceAndTorque();
    }

    void ApplyForceAndTorque()
    {
        // Преобразуем отклонение направления из градусов в радианы
        float deviationInRadians = _directionDeviation * Mathf.Deg2Rad;

        // Вычисляем случайный угол отклонения в пределах directionDeviation
        float randomAngle = Random.Range(-deviationInRadians, deviationInRadians);

        // Создаем направление с учетом отклонения
        Vector2 direction = RotateVector(-1 * transform.up, randomAngle);

        // Вычисляем случайное значение силы и крутящего момента с учетом отклонения
        float finalForce = _force + Random.Range(-_forceDeviation, _forceDeviation);
        float finalTorque = _torque + Random.Range(-_torqueDeviation, _torqueDeviation);

        // Применяем силу в направлении transform.down с учетом отклонения
        _rb.AddForce(direction * finalForce, ForceMode2D.Impulse);

        // Применяем крутящий момент
        _rb.AddTorque(finalTorque, ForceMode2D.Impulse);
    }

    Vector2 RotateVector(Vector2 vector, float angle)
    {
        return new Vector2(
            vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle),
            vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle)
        );
    }
}