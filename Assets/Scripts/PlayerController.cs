using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour 
{
	public static PlayerController instance;

	//variable for player speed
	public float speed = 10;

	//varaible for player jump heigth
	public float jumpVelocity = 10;

	//Variables to tie to in-game objects in the Start()
	Transform myTrans, tagGround;
	Rigidbody2D myBody;

	//To allow player to ground
	public bool isGrounded = false; 

	//To seperate player from other layers
	public LayerMask playerMask;

	float hInput = 0;

	AnimatorController myAnim;

	//Combat
	public int health = 3;
	public float invincibleTimeAfterHurt = 2;

	[HideInInspector]
	public Collider2D[] myColls;

	// Use this for initialization
	void Start () 
	{
		//initialize variables with game objects
		instance = this;
		myColls = this.GetComponents<Collider2D>();
		myBody = this.GetComponent < Rigidbody2D >();
		myTrans = this.transform;
		tagGround = GameObject.Find (this.name + "/tag_ground").transform;
		myAnim = AnimatorController.instance;
	}

	void FixedUpdate () 
	{
		//To know when player is touching the ground
		isGrounded = Physics2D.Linecast (myTrans.position, tagGround.position, playerMask);

		myAnim.UpdateIsGrounded(isGrounded);

		#if !UNITY_ANDROID && !UNITY_IPHONE
		//Will alow us to move with keyboard. Since Unity predefinges keyboard & touch as Horizontal
		hInput = Input.GetAxisRaw("Horizontal");
		myAnim.UpdateSpeed(hInput);

		//Jump button
		if(Input.GetButtonDown("Jump"))
			{
				Jump();
			}
		#endif
		Move (hInput);

	}

	//Controls horizontal movement for player
	public void Move(float horizonalInput)
	{
		Vector2 moveVel = myBody.velocity;
		moveVel.x = horizonalInput * speed;
		myBody.velocity = moveVel;
	}

	public void Jump()
	{
		//Will only jump is tounching ground * No Double Jumps
		if (isGrounded) 
		{
			//Vector2 is short for a vector with x = 0 and y = 1
			myBody.velocity += jumpVelocity * Vector2.up;
		}
	}

	public void StartMoving(float horizonalInput)
	{
		hInput = horizonalInput;
		myAnim.UpdateSpeed(horizonalInput);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.collider.GetComponent<Enemy> ();
		if (enemy != null) 
		{
			foreach(ContactPoint2D point in collision.contacts)
			{
				Debug.Log (point.normal);
				Debug.DrawLine (point.point, point.point + point.normal, Color.red, 10);
				if (point.normal.y >= 0.9f) 
				{
					Vector2 velocity = myBody.velocity;
					velocity.y = jumpVelocity;
					myBody.velocity = velocity;
					enemy.Hurt ();
				} else 
				{
					Hurt ();
				}
			}
		}
	}

	void Hurt()
	{
		health--;
		if (health <= 0) 
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			//Application.LoadLevel (Application.loadedLevel);
		} else 
		{
			myAnim.TriggerHurt(invincibleTimeAfterHurt);
		}
	}

}
