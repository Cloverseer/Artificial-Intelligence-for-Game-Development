namespace AStar
{
	using UnityEngine;
	using System.Collections.Generic;

	public class Pathfinding : MonoBehaviour
	{
		public static Grid grid;
	
		public void Awake()
		{
			if (grid == null)
			{
				grid = GetComponent<Grid>();
			}
		}
		
		public static Path GetPath(Vector3 startPos, Vector3 targetPos)
		{
			Node startNode = grid.NodeFromWorldPoint(startPos);
			Node targetNode = grid.NodeFromWorldPoint(targetPos);

			if (targetNode == null) return null;
			if (!targetNode.walkable) return null;
			
			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();
			
			openSet.Add (startNode);
			
			while(openSet.Count > 0)
			{
				Node currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i++)
				{
					// lowest cost
					if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
					{
						currentNode = openSet[i];
					}
				}
				
				openSet.Remove(currentNode);
				closedSet.Add (currentNode);
				
				if(currentNode == targetNode)
				{
					return TracePath(startNode, targetNode);
				}
				
				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					if(!neighbour.walkable || closedSet.Contains(neighbour)) continue;
					int newMovementCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);
					
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = getDistance(neighbour, targetNode);
						
						neighbour.parent = currentNode;
						if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
					}
				}	
			}
			return null;
		}
		
		public static Path TracePath(Node startNode, Node endNode)
		{
			Path path = new Path();
			Node currentNode = endNode;
			
			while(currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			path.Reverse();
			
			grid.path = path;
			return path;
		}

		// returns True if they arrive close enough to the end of path
		public static bool Seek(Transform t, Entity e, Path p)
		{
			Node tNode = grid.NodeFromWorldPoint(t.position);
						
			// If the path exists, and the next node has a higher y value, jump;
			if (p.Count > 0)
			{
				if(e.jumpable)
				{
					if (p[0].position.y > tNode.position.y)
					{
						e.Jump();
					}
				}
				else
				{
					if (p[0].position.y > tNode.position.y)
					{
						Vector2 newV = t.rigidbody.velocity;
						newV.y = e.speed;
						
						t.rigidbody.velocity = newV;
					}

					else if (p[0].position.y < tNode.position.y)
					{
						Vector2 newV = t.rigidbody.velocity;
						newV.y = -e.speed;
						
						t.rigidbody.velocity = newV;
					}
				}
			}
			else
			{
				t.rigidbody.velocity = new Vector2(0, t.rigidbody.velocity.y);
				return true;
			}
			
			// If we are within 3 nodes distance, we are close enough to the target;
			if (p.Count <= 3)
			{
				float slowerX = Mathf.Lerp(t.rigidbody.velocity.x, 0, Time.fixedDeltaTime * 10);
				t.rigidbody.velocity = new Vector2(slowerX, t.rigidbody.velocity.y);
				return t.rigidbody.velocity.x == 0;
			}
			
			if (tNode.position == p[0].position)
			{
				p.RemoveAt(0);
				p.RemoveAt(p.Count-1);
			}
			
			Vector3 direction = p[0].position - tNode.position;
			float newX = Mathf.Lerp(t.rigidbody.velocity.x, direction.x * e.speed, Time.deltaTime * 50);
			Vector2 oldV = t.rigidbody.velocity;
			oldV.x = newX;
			
			t.rigidbody.velocity = oldV;
			return false;
		}

		// returns true if we reach the end of the path
		public static bool FollowPath(Transform t, Entity e, Path p)
		{
			return FollowPath(t, e, p, e.speed);
		}
		
		public static bool FollowPath(Transform t, Entity e, Path p, float speed)
		{
			Node tNode = grid.NodeFromWorldPoint(t.position);
			
			if (p.Count > 0)
			{
				if (e.jumpable)
				{
					if (p[0].position.y > tNode.position.y)
					{
						e.Jump();
					}
				}
				else
				{
					return FollowFlyingPath(t, e, p, speed);
				}
				
				if (tNode.position == p[0].position && p.Count > 1)
				{
					p.RemoveAt(0);
				}
				
				if (p.Count > 1)
				{
					Vector3 direction = p[0].position - tNode.position;
					float newX = Mathf.Lerp(t.rigidbody.velocity.x, direction.x * speed, Time.deltaTime * 50);
					
					Vector2 oldV = t.rigidbody.velocity;
					oldV.x = newX;
					
					t.rigidbody.velocity = oldV;
					return false;
				}
				else if (p.Count == 1)
				{
					float slowerX = Mathf.Lerp(t.rigidbody.velocity.x, 0, Time.fixedDeltaTime * 2);
					t.rigidbody.velocity = new Vector2(slowerX, t.rigidbody.velocity.y);
					return t.rigidbody.velocity.x == 0;
				}
			}
			return true;
		}

		public static bool FollowFlyingPath(Transform t, Entity e, Path p, float speed)
		{
			Node tNode = grid.NodeFromWorldPoint(t.position);
			if (p.Count > 0)
			{
				Vector3 direction = p[0].position - tNode.position;
				if (p.Count > 1)
				{
					GameObject.CreatePrimitive(PrimitiveType.Cube);
					Vector3 newPos = t.rigidbody.velocity;
					newPos.x = Mathf.Lerp(t.rigidbody.velocity.x, direction.x * speed, Time.deltaTime * 50);
					newPos.y = Mathf.Lerp(t.rigidbody.velocity.y, direction.y * speed, Time.deltaTime * 50);
						
					t.rigidbody.velocity = newPos;

					if (Vector3.Distance(t.position, p[0].position + new Vector3(grid.nodeRadius, grid.nodeRadius)) <= grid.nodeRadius/2)
					{
						p.RemoveAt(0);
					}
					return false;
				}
				else
				{
					t.rigidbody.velocity = Vector3.Lerp(t.rigidbody.velocity, Vector3.zero, Time.fixedDeltaTime * 2);
					return t.rigidbody.velocity.magnitude == 0;
				}
			}
			return true;
		}

		/*
		public void FindPath(Vector3 startPos, Vector3 targetPos)
		{
			Node startNode = grid.NodeFromWorldPoint(startPos);
			Node targetNode = grid.NodeFromWorldPoint(targetPos);
			
			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();
			
			openSet.Add (startNode);
			
			while(openSet.Count > 0)
			{
				Node currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i++)
				{
					// lowest cost
					if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) currentNode = openSet[i];
				}
				
				openSet.Remove(currentNode);
				closedSet.Add (currentNode);
				
				if(currentNode == targetNode)
				{
					RetracePath(startNode, targetNode);
					return;
				}
				
				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					if(!neighbour.walkable || closedSet.Contains(neighbour)) continue;
					int newMovementCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);
					
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = getDistance(neighbour, targetNode);
						
						neighbour.parent = currentNode;
						if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
					}
				}
			}
		}
		
		public void RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;
			
			while(currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			path.Reverse();
			
			grid.path = path;
		}*/
		
		public static int getDistance(Node a, Node b)
		{
			int distX = Mathf.Abs (a.gridX - b.gridX);
			int distY = Mathf.Abs (a.gridY - b.gridY);
			
			if(distX > distY) return distY + (distX - distY);
			return distX + (distY - distX);
		}
	}
}