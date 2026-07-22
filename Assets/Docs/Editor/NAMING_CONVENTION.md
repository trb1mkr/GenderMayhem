# 🏷️ Правила именования

Правила именования папок и файлов со своей спецификой.

## 🗂️ Имена папок

- Папки именуются в **CamelCase**.
- Все ассеты, которые являются общими для группы префабов стоит хранить в папке !Common. 
- Список специальных имён папок Unity: [Special Folders](https://docs.unity3d.com/6000.0/Documentation/Manual/SpecialFolders.html).

## 📂 Специальные имена папок

- Префикс `!` в начале имени папки используется как компактный аналог `_` для перемещения папки в самый верх иерархии, например: `!Common`.


## 🧩 Имена префабов

Префабы **не должны** содержать в названии суффикс `Prefab`.

```
❌ GunPrefab.prefab
✅ Gun.prefab
```

## 📜 Имена скриптов

Скрипты **не должны** содержать в названии суффикс `Script`.

```
❌ ShootingScript.cs
✅ Shooting.cs
```

## 💾 ScriptableObject

Экземпляры `ScriptableObject` **могут** содержать в названии суффикс `Data`.

```
✅ GunData.asset
✅ PlayerConfig.asset
```

**Menu Path:** Атрибут `CreateAssetMenu` должен использовать `menuName` в формате `Scriptable Objects/{НазваниеСистемы}/{НазваниеSO}`.

```
✅ [CreateAssetMenu(menuName = "Scriptable Objects/GameTime/GameTimeConfig")]
❌ [CreateAssetMenu(menuName = "MAD MARKET/GameTime/Config")]
```