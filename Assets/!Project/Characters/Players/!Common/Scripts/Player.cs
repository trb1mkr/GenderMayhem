public class Player : Character
{
    //public UnityEvent PlayerSpawned;
    public PlayerControls Controls { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public CameraAimController CamAimController { get; private set; }

    new void Awake()
    {
        base.Awake();
        Controls = GetComponent<PlayerControls>();
        Movement = GetComponent<PlayerMovement>();
        CamAimController = transform.root.GetComponentInChildren<CameraAimController>();

        Movement.Player = Controls.Player = this;

        //Hands.Weapon = Fists;
        //PlayerSpawned.Invoke();
    }
}