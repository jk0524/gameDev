using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	private Animator animator;
	private Rigidbody2D rigidbody2D;
	
	public float speed = 3.5f; //speed of the player
	
	private bool facingRight = true; //for Flip the player
	private bool blockState = false; //for blocking movement of the player
	private bool attack1 = false; //to identify player's attack

//if you need to seek a GameController script uncomment the code below and code in "void Start()"
	//private GameController gameController;


	void Start () 
	{
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();

//		//Seek GameController
//		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
//		if (gameControllerObject != null)
//		{
//			gameController = gameControllerObject.GetComponent <GameController>();
//		}
//		if (gameController == null)
//		{
//			Debug.Log ("Cannot find 'GameController' script");
//		}
	}
	
//_______________________________________________________________
//_______________________________________________________________

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		//Player Movement
		if (blockState == false) 
			rigidbody2D.velocity = movement * speed;
		else
			rigidbody2D.velocity = movement * 0;

		//Player Run
		if (movement != Vector2.zero)
			animator.SetBool("Run", true);
		else 
			animator.SetBool("Run", false);

		//Player Flip Call
		if (moveHorizontal > 0 && !facingRight)
			Flip ();
		else if (moveHorizontal < 0 && facingRight)
			Flip ();
	}
//_______________________________________________________________
//_______________________________________________________________

	void Update ()
	{
		//Player Attack1 Call
		if (Input.GetButtonDown ("Fire1") && attack1 == false && animator.GetBool ("Death_bool") == false)
			StartCoroutine("Attack1");

		//Player Hit Call
		if (Input.GetKeyDown (KeyCode.H) && animator.GetBool ("Death_bool") == false)
			StartCoroutine("Hit");

		//Player Death Call
		if (Input.GetKeyDown (KeyCode.G) && animator.GetBool ("Death_bool") == false)
			StartCoroutine("Death");
	}
//_______________________________________________________________
//_______________________________________________________________

	//Player Flip
	void Flip()
	{
		if (blockState == false) 
		{
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}
//_______________________________________________________________
//_______________________________________________________________

	//Player Attack1
	IEnumerator Attack1()
	{
		if (blockState == false) 
		{
			animator.SetTrigger ("Attack_01");
			animator.SetBool ("Idle_in_Fight", true);
			StartCoroutine ("BlockstateTimer", 2.4f);

			yield return new WaitForSeconds (0.1f);
			attack1 = true;
			yield return null;
			attack1 = false;
			yield return new WaitForSeconds (0.4f);
		}
	}
	

	//Player Death
	IEnumerator Death()
	{
		animator.SetBool ("Death_bool",true);
		animator.SetTrigger ("Death_01");
		blockState = true;
		yield return null;
	}


	//Player Hit
	IEnumerator Hit()
	{
		animator.SetTrigger("Hit");
		StartCoroutine ("BlockstateTimer", 2.4f);
		yield return null;
	}


	//Timer which blocking movement of the player
	IEnumerator BlockstateTimer(float timer)
	{
		float bstimer = 0f;
		if (animator.GetBool ("Death_bool") == false) 
		{
			for (bstimer = timer; bstimer >= 0f; bstimer -= 0.1f) 
			{
				blockState = true;
				yield return new WaitForSeconds(0.01f);
			}
		}
		if(animator.GetBool ("Death_bool") == false)
			blockState = false;
	}
}