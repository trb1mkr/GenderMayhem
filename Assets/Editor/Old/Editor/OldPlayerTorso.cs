//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;
//using System.IO;

//public class OldPlayerTorso : MonoBehaviour
//{
//    Vector2 mousePosition;
//    private Rigidbody2D Rigidbody;
//    SpriteRenderer SpriteRenderer;
//    Camera Cam;

//    Dictionary<string,Dictionary<string, Sprite>> CharacterAnimations = new Dictionary<string, Dictionary<string, Sprite>>();
//    Weapon Weapon;
//    string State = "Idle";
    
//    void Start()
//    {
//        Weapon = gameObject.GetComponent<Weapon>();
//        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
//        Cam = Camera.main;
//        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

//        GetAllAnimations();
//    }

//    void Update()
//    {
//        Animate();
//        Rotate();
//    }

//    void GetAllAnimations()
//    {
//        var path = "Graphics/Characters/Player/" + SceneManager.GetActiveScene().name;
//        var fullPath = Application.dataPath + "/Resources/" + path;
//        DirectoryInfo[] directories = (new DirectoryInfo(fullPath)).GetDirectories();
//        foreach (DirectoryInfo directory in directories)
//        {
//            var Buffer = Resources.LoadAll<Sprite>(path + "/" + directory.Name);
//            Dictionary<string, Sprite> AnimationDict = new Dictionary<string, Sprite>();
//            foreach (var StateSprite in Buffer)
//            {
//                AnimationDict.Add(StateSprite.name, StateSprite);
//            }
//            CharacterAnimations.Add(directory.Name, AnimationDict);
//        }
//    }

//    void Animate()
//    {
//        Dictionary<string, Sprite> WeaponAnimations;
//        CharacterAnimations.TryGetValue(Weapon.Prefab.name, out WeaponAnimations);
//        Sprite AnimationFrame;
//        WeaponAnimations.TryGetValue(State, out AnimationFrame);
//        SpriteRenderer.sprite = AnimationFrame;
//        gameObject.GetComponent<PixelCollider2D>().Regenerate();
//    }

//    void Rotate()
//    {
//        var targetPosition = Cam.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y - Cam.transform.position.z));
//        Rigidbody.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((targetPosition.y - transform.position.y), (targetPosition.x - transform.position.x)) * Mathf.Rad2Deg);
//    }

//    void OnPickUp()
//    {

//    }

//    void OnAttack()
//    {
//        var BulletPrefab = Resources.Load("Graphics/Weapons/Prefabs/Bullet") as GameObject;
//        var BulletObject = Instantiate(BulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
//        BulletObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(mousePosition.x, mousePosition.y), ForceMode2D.Impulse);
//    }

//    void OnThrow()
//    {
//        var WeaponPrefab = Resources.Load("Graphics/Weapons/Prefabs/" + Weapon.Prefab.name) as GameObject;
//        var WeaponObject = Instantiate(WeaponPrefab, gameObject.transform.position, gameObject.transform.rotation);
//        WeaponObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(mousePosition.x, mousePosition.y), ForceMode2D.Impulse);
//        WeaponObject.GetComponent<Rigidbody2D>().AddTorque(5);
//        State = "Idle";
//    }

//    void OnStockAttack()
//    {

//    }

//    void DropMagazine()
//    {
//        if (((Gun)Weapon).Bullets == 0)
//        {
//            var Magazine = Resources.Load("Graphics/Weapons/Firearms/Magazines/" + Weapon.Prefab.name) as GameObject;
//            Instantiate(Magazine,gameObject.transform.position,gameObject.transform.rotation);
//        }
//    }

//    void OnCrosshair(InputValue position)
//    {
//        mousePosition = position.Get<Vector2>();
//    }
//}