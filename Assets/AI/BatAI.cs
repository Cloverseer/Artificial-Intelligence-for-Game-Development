using UnityEngine;
using AStar;

[RequireComponent (typeof(Entity))]
[RequireComponent (typeof(Rigidbody))]
public class BatAI : MonoBehaviour
{
	public float wanderRadius;

	public enum State
	{
		Idle,
		Wandering,
		Attacking,
		Dead
	};

	[HideInInspector]
	public State state;

	private Vector3 wanderTarget;
	private bool hasWanderTarget;
	private Path path;
	private Entity entity;

	// Use this for initialization
	void Start ()
	{
		entity = GetComponent<Entity> ();
		state = State.Wandering;
		hasWanderTarget = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void FixedUpdate()
	{
		// Entities don't collide with each other's hitboxes.
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("EntityLayer"), LayerMask.NameToLayer ("EntityLayer"), true);

		switch(state)
		{
			case State.Idle:
			{

			} break;

			case State.Wandering:
			{
				if (!hasWanderTarget)
				{
					wanderTarget = new Vector3 (Random.Range (-wanderRadius, wanderRadius) * Pathfinding.grid.nodeDiameter,
					                            Random.Range (-wanderRadius, wanderRadius) * Pathfinding.grid.nodeDiameter,
					                            0);
					path = Pathfinding.GetPath (transform.position, transform.position + wanderTarget);
					if (path != null)
					{
						if( path.Count > 0) hasWanderTarget = true;
					}
				}
				else
				{
					hasWanderTarget = !Pathfinding.FollowFlyingPath (transform, entity, path, entity.speed);
				}
			} break;

			case State.Attacking:
			{
				
			} break;

			case State.Dead:
			{
				
			} break;

			default:
				break;
		}
	}
}
