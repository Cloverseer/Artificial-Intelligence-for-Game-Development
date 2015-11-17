using UnityEngine;

public class MovementWASD : MonoBehaviour
{
	public float speed;
	public float jumpPower;
	public Transform feet;

	private bool canJump = true;

	void Update()
	{
		bool thrust = Input.GetButtonDown ("Fire1");

		GetComponent<Animator> ().SetBool ("Thrust", thrust);

		float x = Input.GetAxis("Horizontal");

		if (x != 0)
		{
			GetComponent<Animator>().SetBool("Run", true);
		}
		else
		{
			GetComponent<Animator>().SetBool("Run", false);
		}

		rigidbody.velocity = new Vector2 (x * speed, rigidbody.velocity.y);

		if (x < 0 && transform.localScale.x > 0)
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		else if (x > 0 && transform.localScale.x < 0)
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		//canJump = Physics2D.Linecast(transform.position, feet.position, 1 << LayerMask.NameToLayer("StandingLayer"));
		if (!canJump)
		{
			canJump = Physics.Linecast(transform.position, feet.position - new Vector3(0, 0.15f), ~(1 << LayerMask.NameToLayer("EntityLayer")));
		}
	}

	void FixedUpdate()
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EntityLayer"), LayerMask.NameToLayer("EntityLayer"), true);

		if (Input.GetButton("Jump") && canJump)
		{
			rigidbody.velocity = new Vector3 (rigidbody.velocity.x, jumpPower);
			canJump = false;
		}
	}
}
