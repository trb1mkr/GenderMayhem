public class Player : Character
{
    public PlayerControls Controls { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public CameraAimController CamAimController { get; private set; }
    public CameraShakeController CamShakeController { get; private set; }

    new void Awake()
    {
        base.Awake();
        Controls = GetComponent<PlayerControls>();
        Movement = GetComponent<PlayerMovement>();
        CamAimController = transform.root.GetComponentInChildren<CameraAimController>();
        CamShakeController = transform.root.GetComponentInChildren<CameraShakeController>();

        Movement.Player = Controls.Player = this;
        Health.Character = this;
    }
}