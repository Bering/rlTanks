using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] float movementForce = 100;
    [SerializeField] float reloadDelay = 2;
    [SerializeField] Shell shell = null;
    [SerializeField] float knockoutVelocity = 25;
    [SerializeField] float recoveryDelay = 3;

    Rigidbody rb;
    Transform shootingPoint = null;
    float nextShootTime;
    float recoveryTime;
    float cameraRotationY;
    Settings settings;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shootingPoint = transform.Find("Tank/Turret/Canon/ShootingPoint");
        Debug.Assert(shootingPoint != null);

        if (isLocalPlayer) {
            Follow f = Camera.main.GetComponent<Follow>();
            f.SetTarget(gameObject);
        }
        else {
            // Destroy onboard camera and audio listener of other players, just keep ours
            Destroy(transform.Find("Camera").gameObject);
        }

        cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;

        settings = GameObject.Find("GameManager").GetComponent<Settings>();
    }

    void Update()
    {
        if (!hasAuthority) return;

        CmdMoveAndRotate(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButton("Fire1")) {
            CmdFire();
        }
    }

    [Command]
    void CmdMoveAndRotate(float horizontal, float vertical)
    {
        if (!CanMove()) return;
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (settings.controlsOrientation == 1) {
            movement = Quaternion.Euler(0, cameraRotationY, 0) * movement;
        }

        // TODO: project movement on terrain's slope
        rb.AddForce(movement * movementForce);
        
        if (movement.magnitude != 0) rb.rotation = Quaternion.LookRotation(movement);
    }

    private bool CanMove()
    {
        return (Time.time > recoveryTime);
    }

    [Command]
    void CmdFire()
    {
        if (!CanFire()) return;

        nextShootTime = Time.time + reloadDelay;

        Shell newShell = Instantiate(shell, shootingPoint.position, shootingPoint.rotation);
        NetworkServer.Spawn(newShell.gameObject);
    }

    // only called by server but returns a value so can't be made a command
    private bool CanFire()
    {
        return (Time.time > nextShootTime);
    }

    void OnCollisionEnter(Collision c)
    {
        if (isServer == false) return;

        Shell shell = c.gameObject.GetComponent<Shell>();
        if (shell == null) return;
        
        Knockout(c.relativeVelocity.magnitude);
    }

    void Knockout(float velocity)
    {
        if (velocity >= knockoutVelocity) {
            recoveryTime = Time.time + recoveryDelay;
        }
    }
}
