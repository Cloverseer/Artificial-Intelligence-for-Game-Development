namespace AStar
{
	using UnityEngine;
	using System.Collections.Generic;
	
	public class Grid : MonoBehaviour
	{
		public Transform player;
		public Vector2 gridWorldSize;
		public float nodeRadius;
		public LayerMask unwalkableMask;
		public Path path;
		public Map map;
		
		private Node[ , ] grid;

		[HideInInspector]
		public float nodeDiameter;
		private int gridSizeX, gridSizeY;

		public void Start()
		{
			nodeDiameter = nodeRadius * 2;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

			CreateGrid();
		}
		
		public void CreateGrid()
		{
			grid = new Node[gridSizeX, gridSizeY];
			
			// world bottom left change to fit with xy not xz
			Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
			
			for(int x = 0; x < gridSizeX; x++)
			{
				for(int y = 0; y < gridSizeY; y++)
				{
					// node pts
					Vector3 worldPoint = bottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.up * (y * nodeDiameter + nodeRadius));
					bool walkable = true;

					if (map != null) 
					{
						MapData mapData = map.GetMapData ();

						if (mapData[y,x].IsCollidable())
						{
							walkable = false;
						}

					}
					else walkable = !Physics.CheckSphere(worldPoint, nodeRadius-0.05f, unwalkableMask);



					grid[x, y] = new Node(walkable, worldPoint, x, y);
				}
			}
		}
		
		public List<Node> GetNeighbours(Node node)
		{
			List<Node> neighbours = new List<Node>();
			
			for(int x = -1; x <= 1; x++)
			{
				for(int y = -1; y <= 1; y++)
				{
					if (y == -1 || y == 1) 
					{
						if (x == 1 || x == -1) continue;
					} 

					if (x == -1 || x == 1) 
					{
						if (y == 1 || y == -1) continue;
					} 
						
					if(x == 0 && y == 0) continue;
					int checkX = node.gridX + x;
					int checkY = node.gridY + y;
					
					if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					{
						neighbours.Add(grid[checkX, checkY]);
					}
				}
			}
			return neighbours;
		}
		
		public Node NodeFromWorldPoint(Vector3 worldPos)
		{
			float percentX = (worldPos.x) / gridWorldSize.x;
			float percentY = (worldPos.y) / gridWorldSize.y;
			percentX = Mathf.Clamp01 (percentX);
			percentY = Mathf.Clamp01 (percentY);
			
			int x = Mathf.RoundToInt ((gridSizeX) * percentX);
			int y = Mathf.RoundToInt ((gridSizeY) * percentY);
			if (x > 0 && x < gridSizeX &&
			    y > 0 && y < gridSizeY)
			{
				return grid[x, y];
			}
			return null;
		}
		
		//check if working, can delete anything dealing with path
		void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
			if (grid != null)
			{
				// check if working, can delete anything dealing with playernode
				//Node playerNode = NodeFromWorldPoint(player.position);

				foreach (Node n in grid)
				{
					Gizmos.color = (n.walkable) ? new Color(1.0f, 1.0f, 1.0f, 0.1f) : new Color(1.0f, 0.0f, 0.0f, 0.2f);
					//if(playerNode == n) Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.3f);
					if(path != null)
					{
						if(path.Contains(n)) Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.2f);
					}
					Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - 0.05f));
				}
			}
		}
	}
}