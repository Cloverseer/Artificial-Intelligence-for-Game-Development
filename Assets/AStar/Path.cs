namespace AStar
{
	using UnityEngine;
	using System.Collections.Generic;

	public class Path
	{
		private List<Node> list;
		
		public Path()
		{
			list = new List<Node>();
		}
		
		public Path(List<Node> l)
		{
			list = l;
		}
		
		public Node this[int x]
		{
			get { return list[x]; }
			set { list[x] = value; }
		}
		
		public int Count
		{
			get { return list.Count; }
		}
		
		public void Add(Node n)
		{
			list.Add(n);
		}
		
		public void Clear()
		{
			list.Clear();
		}
		
		public bool Contains(Node n)
		{
			return list.Contains(n);
		}
		
		public void RemoveAt(int x)
		{
			list.RemoveAt(x);
		}
		
		public void Reverse()
		{
			list.Reverse();
		}
		
		public Node closestNode(Vector3 position)
		{
			Node temp = list[0];
			
			foreach(Node n in list)
			{
				if (Vector3.Distance(position, temp.position) > Vector3.Distance(position, n.position))
				{
					temp = n;
				}
			}
			
			return temp;
		}
	}
}