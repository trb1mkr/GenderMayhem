using UnityEngine;
using UnityEngine.Events;

public class Player : Character
{
    public UnityEvent PlayerSpawned;
    public PlayerControls Controls { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerCamera Camera { get; private set; }
    //public FinishOff FinishOff { get; private set; }

    new void Awake()
	{
        base.Awake();
        Controls = GetComponent<PlayerControls>();
        Movement = GetComponent<PlayerMovement>();
        Camera = GetComponentInChildren<PlayerCamera>();

        Movement.Player = Controls.Player = Camera.Player = this;

        State = "Idle";
        Weapon = Fists;

        PlayerSpawned.Invoke();
	}
}