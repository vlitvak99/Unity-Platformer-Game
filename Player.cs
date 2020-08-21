using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private float horizontalInput;
    private bool restartInput;
    private bool jumpInput;
    private bool isGrounded = true; 
    private bool canJump;

    private readonly float RUN_SPEED = 5;
    private readonly float JUMP_STRENGTH = 15;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        Restart();
        Death();
        Win();
        GroundCheck();
        Flip();
    }

    //use fixed update when manipulating physics objects like the rigidbody
    void FixedUpdate()
    {
        Run();
        Jump();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetKey(KeyCode.Space);
        restartInput = Input.GetKey(KeyCode.R);
    }

    private void Restart()
    {
        if (restartInput)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    //if a player's y position is below -10, they have fallen and died
    private void Death()
    {
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("Death Screen");
        }
    }

    //if the player passes the x position, 55, and is above y position, 6, they have won
    private void Win()
    {
        if(transform.position.x > 55 && transform.position.y > 6)
        {
            SceneManager.LoadScene("Win Scene");
        }
    }

    private void Run()
    {
        rb2d.velocity = new Vector2(horizontalInput * RUN_SPEED, rb2d.velocity.y);
    }

    private void GroundCheck()
    {
        //sets the layermask so that rays only detect the "Solids" layer
        LayerMask layermask = 1 << LayerMask.NameToLayer("Solids");

        RaycastHit2D[] rays = new RaycastHit2D[3];
        rays[0] = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, layermask);
        rays[1] = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.down, 1.1f, layermask);
        rays[2] = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.down, 1.1f, layermask);

        //check all rays to see if they touch the ground
        for (int i = 0; i < rays.Length; i++)
        {
            if (rays[i].collider)
            {
                //a reflected surface normal of 0.9 means the ground must be 90% level (not a wall).  
                if (rays[i].normal.y > 0.9f)
                {

                    isGrounded = true;

                    //only enable jumping if you touch the ground and let go of jump button
                    if (!jumpInput)
                        canJump = true;

                    return;
                }
            }
        }

        //if this code is reached, none of the surface normals were 0.9 long so none touch ground so we must be airborne
        isGrounded = false;
    }

    //flips the sprite left or right depending on your direction of movement
    private void Flip()
    {
        if (rb2d.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (rb2d.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Jump()
    {
        //you must be on the ground to jump
        if (!isGrounded)
            return;

        //you can't hold down the jump button and keep jumping
        if (!canJump)
            return;

        //otherwise, jump if you are pressing the jump button
        if (jumpInput)
        {
            //disables double jumping
            canJump = false;

            //adds vertical velocity
            rb2d.velocity = new Vector2(rb2d.velocity.x, JUMP_STRENGTH);
        }
    }
}
