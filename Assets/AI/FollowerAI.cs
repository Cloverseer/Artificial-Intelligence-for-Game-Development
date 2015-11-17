using UnityEngine;
using System.Collections.Generic;
using AStar;

[RequireComponent (typeof(Entity))]
[RequireComponent (typeof(Rigidbody))]
public class FollowerAI : MonoBehaviour
{
	public Transform target;
	public float wanderRange;

	public enum State
	{
		Idle,
		Wandering,
		Attacking,
		Defending,
		Following, // Arguably the most important one.
		Dead
	};
	
	[HideInInspector]
	public State state;
	
	private float idleTimer;
	private float wanderTimer;
	private Vector3 oldTargetPos;
	private Vector3 wanderTarget;
	private bool canJump;
	private bool hasEnemyTarget;
	private bool hasWanderTarget;
	private State oldState; // For debugging.
	private Path path;
	private Entity entity;
	private Animator animator;

	void Awake ()
	{
		idleTimer = 0.0f;
		wanderTimer = 0.0f;
	}
	
	// Use this for initialization
	void Start ()
	{
		entity = GetComponent<Entity> ();
		animator = transform.GetComponentInChildren<Animator>();
		state = State.Idle;
		oldTargetPos = target.position;
		hasEnemyTarget = false;
		hasWanderTarget = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (oldTargetPos != target.position) state = State.Following;
	
		if (state.Equals (State.Idle))
		{
			wanderTimer += Time.deltaTime;
		}

		// If following, and the player hasn't changed positions, we start the idleTimer.
		if (state.Equals (State.Following) && oldTargetPos == target.position)
		{
			idleTimer += Time.deltaTime;
		}
		else
		{
			idleTimer = 0.0f;
		}

		if (rigidbody.velocity.x != 0)
		{
			animator.SetBool("Run", true);
		}
		else
		{
			animator.SetBool("Run", false);
		}

		oldTargetPos = target.position;
		oldState = state;
	}

	void FixedUpdate ()
	{
		// Entities don't collide with each other's hitboxes.
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("EntityLayer"), LayerMask.NameToLayer ("EntityLayer"), true);
	
		if (!state.Equals (State.Idle))
		{
			wanderTimer = 0.0f;
		}
	
		// FSM
		switch (state)
		{
			case State.Idle:
			{
				if (wanderTimer > 3.0f)
				{
					state = State.Wandering;
				}
				
				else if (Vector3.Distance (transform.position, target.position) > 10 * Pathfinding.grid.nodeRadius)
				{
					state = State.Following;
				}
			} break;

			case State.Wandering:
			{
				if (!hasWanderTarget)
				{
					wanderTarget = new Vector3 (Random.Range (-wanderRange, wanderRange) * Pathfinding.grid.nodeRadius * 2, target.position.y, target.position.z);
					path = Pathfinding.GetPath (transform.position, wanderTarget);
					if (path != null)
					{
						if( path.Count > 0) hasWanderTarget = true;
					}
				}
				else
				{
					hasWanderTarget = !Pathfinding.FollowPath (transform, entity, path, entity.speed / 4);
				}
			} break;
				
			case State.Attacking:
			{
				
			} break;
				
			case State.Defending:
			{
				
			} break;
				
			case State.Following: // seek, jumping, ladder
			{
				if (idleTimer >= 3.0f)
				{
					state = State.Idle;
				}

				// If the follower gets too far, teleport to target;
				if (Vector3.Distance (transform.position, target.position) > 15)
				{
					transform.position = target.position;
				}
				
				path = Pathfinding.GetPath (transform.position, target.position - Vector3.up);
				if (path != null)
				{
					Pathfinding.Seek (transform, entity, path);
				//Debug.Log (transform.rigidbody.velocity + " " + target.rigidbody.velocity);
				}
			} break;
				
			default:
				break;
		}
	}
}
