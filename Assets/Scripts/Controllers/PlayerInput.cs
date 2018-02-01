using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController),typeof(ProjectileShoot))]
public class PlayerInput : MonoBehaviour
{
    [Header("Keyboard Input")]
    public KeyCode kbJump = KeyCode.Space;
    public KeyCode kbAbility1 = KeyCode.Q;
    public KeyCode kbAbility2 = KeyCode.E;
    public KeyCode kbAbility3 = KeyCode.R;
    public KeyCode kbAbility4 = KeyCode.F;
    public KeyCode kbPause = KeyCode.Escape;
    public KeyCode kbInteract = KeyCode.Return;

    [Header("Controller Input")]
    public KeyCode ctJump = KeyCode.Joystick1Button0;
    public KeyCode ctAbility1 = KeyCode.Joystick1Button5;
    public string ctAbility2 = "RightTrigger";
    public KeyCode ctAbility3 = KeyCode.Joystick1Button4;
    public string ctAbility4 = "LeftTrigger";
    public KeyCode ctPause = KeyCode.Joystick1Button7;
    public KeyCode ctInteract = KeyCode.Joystick1Button1;

    private PlayerController player;
    private ProjectileShoot projectile;

    //Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerController>();
        projectile = GetComponent<ProjectileShoot>();
    }

    //Update is called once per frame
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float leftTrigger = Input.GetAxisRaw(ctAbility4);
        float rightTrigger = Input.GetAxisRaw(ctAbility2);

        if(directionalInput.y >= 0.5f)
        {
            directionalInput.y = 1;
        }
        else if(directionalInput.y <= -0.5f)
        {
            directionalInput.y = -1;
        }
        else
        {
            directionalInput.y = 0;
        }

        player.SetDirectionalInput(directionalInput);

        //Jump input
        if (Input.GetKeyDown(kbJump) || Input.GetKeyDown(ctJump))
        {
            player.OnJumpInputDown();
        }

        //Jump input
        if (Input.GetKeyUp(kbJump) || Input.GetKeyUp(ctJump))
        {
            player.OnJumpInputUp();
        }

        if(Input.GetKeyDown(kbAbility1)|| Input.GetKeyDown(ctAbility1))
        {
            projectile.ShootProjectile();
        }

        if (Input.GetKeyDown(kbAbility2) || rightTrigger > 0)
        {
            Debug.Log("Fire Ability 2");
        }
        if (Input.GetKeyDown(kbAbility3) || Input.GetKeyDown(ctAbility3))
        {
            Debug.Log("Fire Ability 3");
        }
        if (Input.GetKeyDown(kbAbility4) || leftTrigger > 0)
        {
            Debug.Log("Fire Ability 4");
        }
    }
}