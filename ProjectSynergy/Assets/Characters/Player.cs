using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public AudioClip jumpSFX;

    public float speed = 6.0F;
    public float airSpeed = 1.0F;
    public float jumpForce = 10.0F;
    public float gravityJump = 15.0f;
    public float gravityFall = 20.0f;
    public float pushPower = 1.0F;

    [HideInInspector]
    public string pollenatedPlantName = null;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;
    [HideInInspector]
    public bool shroomJump = false;
    
    public bool freezePlayer = false;
    [HideInInspector]
    public bool freezeJump = false;

    private int downwardCeilingForce = 3;
    private float gravity = 20.0f;
    private float startPos;
    private AnimateSprite animateSprite;
    private bool isJumping = false;
	
	//int currentScene = 0; // Current scene/level number.

    private Vector3 originalVelocity;

    private void Awake()
    {
        animateSprite = gameObject.GetComponent<AnimateSprite>();
	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.attachedRigidbody != null)								//only things woith ridgid bodies need/can be pushed
        {
            pushObject(hit);
        }
    }

    private void Update()
    {
        if (freezePlayer == false)
        {
            Movement();
        }
        PlayerAnimate();
		
		// @todo: Start music in level 00 once the player starts moving.
		// But we only want to do this in the very beginning of the game, so probably should go into Game or Music Manager.
		//currentScene = Application.loadedLevel;
		//if (velocity.x > 0 && currentScene == 0)
        //{
        //}
    }

    private void pushObject(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        //Vector3 pushDir = new Vector3(hit.moveDirection.x, rigidbody.velocity.y, 0);
        //rigidbody.velocity = pushDir * pushPower;
        rigidbody.velocity = new Vector3(hit.moveDirection.x * pushPower, rigidbody.velocity.y, 0);
        //Nut nut = hit.collider.
        gameObject.GetComponent<Nut>();

        //if (!(this.gameObject.transform.position.y  > hit.collider.transform.position.y + this.gameObject.transform.lossyScale.y))   //checks if player above nut
        //{
        //    nut.CallAnimation();
        //}   
    }

    private void SetFacingDirection()
    {
        //aniamtion for direction
        if (velocity.x < 0 && this.transform.localScale.x > 0)
        {
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);//times it by direction to get - or positive and apply that to scale.
        }
        else if (velocity.x > 0 && this.transform.localScale.x < 0)
        {
            this.transform.localScale = new Vector3(-1 * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    private void Movement()
    {
        CharacterController controller = gameObject.GetComponent<CharacterController>();
        //originalVelocity = velocity;
        velocity = new Vector3(Input.GetAxis("Horizontal"), velocity.y, 0);//Input.GetAxis("Horizontal") will be 1,-1 or 0

        SetFacingDirection();

        if (controller.isGrounded)// && (this.velocity.y < 0.01f || this.velocity.y > -0.01f))
        {
            GroundBehaviour();
        }

        if (!controller.isGrounded)// && (this.velocity.y > 0.01f || this.velocity.y <  -0.01f))
        {
            AirBehaviour();
        }

        if (controller.collisionFlags == CollisionFlags.Above)
        {
            velocity.y = 0;
            velocity.y -= downwardCeilingForce;
        }
        //Debug.Log("after " + velocity.x);
        controller.Move(velocity * Time.deltaTime);
        LevelManager.levelManager.LogHeatMap(controller.transform.position.x, controller.transform.position.y);
    }

    private void GroundBehaviour()
    {
        velocity.x *= speed; //converts the direction (1,-1,0) into the speed in that direction

        isJumping = false;
        //This may be eneded for nut problems or such
        // This seems to solve issue with unknown cause that involves falling super fast sometimes.
        if (velocity.y < 0)                                         //if grounded then set downward to 0. Allow psotive force for jumps and bounce.
        {
            velocity.y = 0;
        }

        if (Input.GetButton("Jump"))
        {
            if (freezeJump == false)
            {
                isJumping = true;
                velocity.y = jumpForce;
				
				// AD: Use Fabric.
                //audio.PlayOneShot(jumpSFX);
				Fabric.EventManager.Instance.PostEvent("Jump");
            }
        }
        shroomJump = false;                                         //turns it off if its on
    }

    private void AirBehaviour()
    {
        velocity.x *= airSpeed;

        if (Input.GetButton("Jump") && shroomJump == false) //Special gravity for hodling jump and not using a shroom
        {
            //gravity code
            gravity = gravityJump;
        }

        if (Input.GetButtonUp("Jump") || velocity.y < 0) //get button UP. or velocity is negative, switch to normal gravity
        {
            gravity = gravityFall;
        }

        velocity.y -= gravity * Time.deltaTime;             //Gravity
    }

    private void PlayerAnimate()
    {
        //animateSprite.PlayAnimation("DiagonalJump");
        ////Animate  idle
        if (velocity.x == 0 || freezePlayer == true)
        {
            if (pollenatedPlantName == "")
            {
                animateSprite.PlayAnimation("Idle");
            }
            else
            {
                animateSprite.PlayAnimation("IdlePollenated");
            }
        }
        else
        {
			//Debug.Log ("Walking");
			//Fabric.EventManager.Instance.PostEvent ("Footsteps");
            if (pollenatedPlantName == "")
            {
                animateSprite.PlayAnimation("Walk");
            }
            else
            {
                animateSprite.PlayAnimation("WalkPollenated");
            }
        }

        if (isJumping == true)
        {
            if (pollenatedPlantName == "")
            {
                animateSprite.PlayAnimation("DiagonalJump");
            }
            else
            {
                animateSprite.PlayAnimation("DiagonalJumpPollenated");
            }
        }
    }
}
