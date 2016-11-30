using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour 
{

	public static AnimatorController instance;

	Transform myTrans;
	//Call animations
	Animator myAnim;
	//To rotate player direction
	Vector3 artScaleCache;


	// Use this for initialization
	void Start () 
	{
		myTrans = this.transform;
		myAnim = this.gameObject.GetComponent<Animator> ();
		instance = this;

		artScaleCache = myTrans.localScale;
	}

	void FlipArt(float currentSpeed)
	{
		//Going left and facing right or going right and facing left 
		if((currentSpeed < 0 && artScaleCache.x == 1)||(currentSpeed > 0 && artScaleCache.x == -1))
		{
			//flip the art
			artScaleCache.x *= -1;
			myTrans.localScale = artScaleCache;
		}
	}

	// Update is called once per frame
	public void UpdateSpeed (float currentSpeed) 
	{
		myAnim.SetFloat ("Speed", currentSpeed);
		FlipArt (currentSpeed);

	}

	public void UpdateIsGrounded(bool isGrounded)
	{
		myAnim.SetBool ("isGrounded", isGrounded);
	}

	public void TriggerHurt(float hurtTime)
	{
		StartCoroutine (HurtBlinker (hurtTime));
	}

	IEnumerator HurtBlinker(float hurtTime)
	{
		//Ignore collisiong with Enemies
		int enemyLayer = LayerMask.NameToLayer("Enemy");
		int playerLayer = LayerMask.NameToLayer("Player");
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer);
		foreach(Collider2D collider in PlayerController.instance.myColls)
		{
			collider.enabled = false;
			collider.enabled = true;
		}

		//Start looping blink anim
		myAnim.SetLayerWeight(1,1);
		//Wait for invincibility to end
		yield return new WaitForSeconds(hurtTime);

		//Stop blinking animaiton and re-enable collision
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer, false);
		myAnim.SetLayerWeight(1,0);
	}
}
