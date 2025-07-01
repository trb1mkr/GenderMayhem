using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Level level = new Level();
    public Characters characters = new Characters();
    public Items items = new Items();

    public struct Level
    {
        public static GameObject Objects;
        public static GameObject Floor;
        public static Obstacles Obstacles;
    }

    public struct Obstacles
    {
        public static GameObject Doors;
        public static GameObject Walls;
        public static GameObject Glass;
    }

    public struct Characters
    {
        public static GameObject Enemies;
        public static GameObject Paths;
    }

    public struct Items
    {
        public static GameObject Weapons;
        public static GameObject Bullets;
        public static GameObject Shells;
        public static GameObject Magazines;
    }
        
    void Awake()
    {
        SceneManager.LoadScene(1);
    }
}
