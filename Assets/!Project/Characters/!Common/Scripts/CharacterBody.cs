using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    public Transform Head;
    public Transform Torso;
    public Transform Legs;

    [HideInInspector] public Character Character;
}
