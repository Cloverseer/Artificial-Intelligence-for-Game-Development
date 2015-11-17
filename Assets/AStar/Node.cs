namespace AStar
{
	using UnityEngine;
	
	public class Node
	{
		public bool walkable;
		public Vector3 position;
		
		public int gCost, hCost;
		public int gridX, gridY;
		
		public Node parent;
		
		public Node(bool walkable, Vector3 position, int gridX, int gridY)
		{
			this.walkable = walkable;
			this.position = position;
			this.gridX = gridX;
			this.gridY = gridY;
		}
		
		public int fCost
		{
			get { return gCost + hCost; }
		}
	}
}