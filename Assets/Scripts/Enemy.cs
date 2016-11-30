using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{

	public LayerMask enemyMask;
	public float speed = 1;
	Rigidbody2D myBody;
	Transform myTrans;
	float myWidth, myHeight;


	void Start () 
	{
		myTrans = this.transform;
		myBody = this.GetComponent<Rigidbody2D> ();
		SpriteRenderer mySprite = this.GetComponent<SpriteRenderer> ();
		myWidth = mySprite.bounds.extents.x;
		myHeight = mySprite.bounds.extents.y;

	}

	void FixedUpdate () 
	{
		//check for edge of platform
		Vector2 lineCastPosition = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + Vector2.up * myHeight;
		Debug.DrawLine (lineCastPosition, lineCastPosition + Vector2.down);
		bool isGrounded = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down, enemyMask);
		Debug.DrawLine (lineCastPosition,lineCastPosition - myTrans.right.toVector2() * .05f);
		bool isBlocked = Physics2D.Linecast(lineCastPosition, lineCastPosition - myTrans.right.toVector2() * .05f, enemyMask);

		if (!isGrounded || isBlocked) 
		{
			Vector3 currRot = myTrans.eulerAngles;
			currRot.y += 180;
			myTrans.eulerAngles = currRot;
		}

		//Always move foward
		Vector2 myVel = myBody.velocity;
		myVel.x = -myTrans.right.x * speed;
		myBody.velocity = myVel;
	}

	public void Hurt()
	{
		Destroy (this.gameObject);

	}
}