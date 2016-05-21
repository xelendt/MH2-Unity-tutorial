using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jumpForce = 500.0f;

	public AudioClip winSound;
	AudioSource audio;

	private int score = 0;

	SpriteRenderer sr;
	private Animator animator;

	Rigidbody2D rb;

	void Awake(){
		//Get references to our components
		rb = GetComponent<Rigidbody2D>();
	}

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		MoveHorizontal(Input.GetAxis("Horizontal"));

		if(Input.GetButtonDown("Jump")){ //If we pushed the jump button down this frame...
			Jump(); //Lets jump!
		}

		if (Input.GetAxis ("Horizontal") > 0) {
			this.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
			animator.SetInteger ("linkstate", 2);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			this.transform.localScale = new Vector3 (-0.1f, 0.1f, 1);
			animator.SetInteger ("linkstate", 2);
		} else {
			animator.SetInteger ("linkstate", 0);
		}
		if (!isGrounded()) {
			animator.SetInteger ("linkstate", 1);
		}

		transform.eulerAngles = new Vector3 (0, 0, 0);
	}

	void MoveHorizontal( float input ){
		Vector2 moveVel = rb.velocity;

		moveVel.x = input * speed;

		rb.velocity = moveVel;
	}

	void Jump(){
		if( isGrounded() ){
			rb.AddForce(Vector2.up * jumpForce); //Add a upward force to our rigidbody
		}
	}

	void OnCollisionEnter2D(Collision2D coll) { //On the frame this object's Collider collides with another collider...
		if (coll.gameObject.name == "Coin"){  //Check if we've hit a speed powerup
			Debug.Log("Coin picked up!"); //Lets let the console know we've hit a powerup
			Destroy(coll.gameObject); //Destroy the powerup
			Debug.Log("Score:\t" + (++score));
		}
		if (coll.gameObject.name == "Fairy") {
			Debug.Log ("YOU WIN");
			audio.PlayOneShot(winSound, 0.7F);
		}
	}

	bool isGrounded(){

		float spriteRange = 0.75f;	// how big our sprite is
		float raycastRange = spriteRange + 0.05f; //How far should we do the linecast

		RaycastHit2D hit = Physics2D.Linecast(transform.position - new Vector3(0, spriteRange, 0), 
			transform.position - new Vector3(0, raycastRange, 0));

		if(hit.collider == null){ //If the raycast didn't hit anything
			Debug.Log("not hitting anything");
			return false;
		} else if(hit.collider.tag == "Platform"){ //If it hit a ground's collider
			return true;
		} else { //If it hit anything else
			Debug.Log( hit.collider.tag );
			return false;
		}		

	}
}
