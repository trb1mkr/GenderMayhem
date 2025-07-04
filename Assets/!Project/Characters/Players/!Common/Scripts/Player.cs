using UnityEngine;
using UnityEngine.Events;

public class Player : Character
{
    public UnityEvent PlayerSpawned;
    public PlayerControls Controls { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public CameraAimController Camera { get; private set; }
    //public FinishOff FinishOff { get; private set; }

    new void Awake()
    {
        base.Awake();
        Controls = GetComponent<PlayerControls>();
        Movement = GetComponent<PlayerMovement>();
        Camera = transform.root.GetComponentInChildren<CameraAimController>();

        Movement.Player = Controls.Player = Camera.Player = this;

        //Hands.Weapon = Fists;

        PlayerSpawned.Invoke();

        Controls.AltUsed.AddListener(StateController.HandleAltUsed);
        Controls.Used.AddListener(StateController.HandleUsed);
    }
}