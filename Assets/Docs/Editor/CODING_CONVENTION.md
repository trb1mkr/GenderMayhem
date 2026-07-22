# 👨‍💻 Правила разработки

Документ регламентирует правила написания кода и построения архитектуры.

## 🧩 Композиция и агрегация вместо наследования

В проекте предпочтительно использовать **агрегацию** и **композицию** вместо наследования.

- **Композиция**: объект состоит из других объектов, которые не существуют вне его.
- **Агрегация**: объект содержит ссылки на другие объекты, которые могут существовать независимо.
- Избегать глубоких иерархий наследования (более 2 уровней).

```csharp
// ❌ Наследование
public class Weapon : Item { }

// ✅ Композиция
public class Weapon
{
    public WeaponData WeaponData { get; }
    public ItemData ItemData { get; }
}
```

Logic-класс **не должен** дублировать ссылки на поля своего Data через публичные свойства, если прямо не приказано иное. Само поле `Data` должно быть публичным (`public`), чтобы внешние классы могли обращаться к данным напрямую:

```csharp
// ❌ Дублирование свойств Data в логике
public class Gun
{
    private GunData _data;
    public int Ammo => _data.Ammo;        // дублирование
    public FireMode Mode => _data.FireMode; // дублирование
}

// ✅ Публичный доступ к Data
public class Gun
{
    public GunData Data { get; }
    public Gun(GunData data) => Data = data;
}

// Использование: gun.Data.Ammo, gun.Data.FireMode
```

## 🔗 Data + SO паттерн

Когда данные должны настраиваться в инспекторе и сериализоваться как ассет, используется **Data + SO**:

- **Data** — чистый `[Serializable]` класс. Хранит **все** значимые поля (настройки + runtime). Не зависит от Unity.
- **SO** — `ScriptableObject`-обёртка. Содержит `public Data` (`[InlineProperty, HideLabel]`) + Odin Inspector UI (калькуляторы, превью, кнопки). **Не дублирует** поля Data.

```csharp
// ✅ Data — все значимые поля здесь
[Serializable]
public class GunData
{
    public int Ammo = 30;
    public float FireRate = 0.1f;
    public double RuntimeHeat;    // runtime тоже здесь
}

// ✅ SO — только UI обвязка
[CreateAssetMenu(menuName = "Scriptable Objects/Combat/GunData")]
public class GunDataSO : ScriptableObject
{
    [InlineProperty, HideLabel]
    public GunData Data = new();

    // Только UI-поля: калькуляторы, превью, кнопки
    [Button]
    public void ResetToDefault() => Data.Ammo = 30;
}

// Использование:
//   var gun = new Gun(gunDataSO.Data);
//   gun.Data.Ammo
```

**Правила:**
1. SO **не должен** содержать значимые данные вне `Data`.
2. Data **не должен** зависеть от Unity (нет `UnityEngine` using).
3. Runtime-поля тоже живут в Data (сериализуются для сейвов/сети).

## 🔄 Реактивное поведение с R3

Для реактивного поведения используется [R3](https://github.com/Cysharp/R3).

- Подписки оформлять через `Observable` / `Subject`.
- В View подписываться на изменения из Presenter.
- Управлять жизненным циклом подписок через `CompositeDisposable`.

```csharp
public class Gun
{
    public ReactiveProperty<int> Ammo { get; } = new();
}

public class GunView : MonoBehaviour
{
    [SerializeField] private Text _ammoText;
    private readonly CompositeDisposable _disposable = new();

    public void Bind(Gun gun)
    {
        gun.Ammo.Subscribe(ammo => _ammoText.text = ammo.ToString())
            .AddTo(_disposable);
    }

    private void OnDestroy() => _disposable.Dispose();
}
```

## 🔗 Слабая связность через события и делегаты

Для уменьшения жёсткой взаимосвязи между модулями и системами следует использовать:

1. **C# делегаты и события** (`event Action<T>`, `Action`, `Func<T>`) — приоритетный способ. Модули подписываются на события ядра/системы самостоятельно, ядро не знает о существовании модулей.
2. **UnityEvent / UnityAction** — менее приоритетный способ, использовать при необходимости инспекторной настройки.

Принцип: ядро публикует события, модули подписываются. Ядро не хранит ссылки на модули и не управляет их жизненным циклом напрямую.

```csharp
// Ядро — публикует
public class GameTimeCore
{
    public event Action<double, float> OnTick;
    public void Tick(float dt) => OnTick?.Invoke(totalSeconds, dt);
}

// Модуль — подписывается сам
public class GameTimeDayModule : GameTimeModuleBase
{
    public GameTimeDayModule(GameTimeCore core) : base(core) { }
    protected override void OnTick(double seconds, float dt) { /* ... */ }
}
```

## ⚡ Expression-bodied members и однострочные блоки

Для краткости и читаемости:

1. **Методы/свойства на 1 строку** — используйте expression-bodied syntax (`=>`).
   ```csharp
   public int GetValue() => _value;  // ✅
   public int GetValue() { return _value; }  // ❌
   ```

2. **Условия и циклы на 1 строке** — фигурные скобки не обязательны (C# 8+).
   ```csharp
   if (x > 0) { DoSomething(); }  // ❌ лишние скобки
   if (x > 0) DoSomething();  // ✅
   foreach (var item in list) Process(item);  // ✅
   foreach (var item in list) Process(item);  // ✅
        if (x > 0) 
            DoSomething();
   ```

3. **Многострочные** — всегда используйте блоки `{}`.

## 🧱 Принципы SOLID

Проект следует принципам SOLID:

| Принцип | Описание |
|---------|----------|
| **S**ingle Responsibility | Каждый класс отвечает за одну зону ответственности |
| **O**pen/Closed | Классы открыты для расширения, закрыты для модификации |
| **L**iskov Substitution | Производные классы должны быть взаимозаменяемы с базовыми |
| **I**nterface Segregation | Интерфейсы должны быть узкоспециализированными |
| **D**ependency Inversion | Зависимости направлены на абстракции, а не на конкретные реализации |
