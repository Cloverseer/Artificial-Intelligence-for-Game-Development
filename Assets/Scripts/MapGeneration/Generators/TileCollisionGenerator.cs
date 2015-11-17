using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileCollisionGenerator 
{
	public Mesh Mesh
	{
		get;
		private set;
	}

	private MapData mapData;

	private List<Mesh> meshes;
	public TileCollisionGenerator(MapData mapData)
	{
		this.mapData = mapData;

		meshes = new List<Mesh>();

		MakeMesh();
	}

	public void MakeMesh()
	{
		GetFloorCollision    ();
		GetCeilingCollision  ();
		GetLeftCollision     ();
		GetRightCollision    ();
		GetDiagonalCollision ();
		
		this.Mesh = new Mesh();

		this.Mesh.CombineMeshes(CombineMeshes(meshes), true, false);
		this.Mesh.Optimize();
		
	}
	
	private void GetFloorCollision ()
	{
		Rect floorRect = new Rect();

		for (int row = 0; row < mapData.Height; ++row)
		{
			for (int col = 0; col < mapData.Width; ++col)
			{
				Tile currentTile = mapData[row, col];
				if (IsNormalCollision(currentTile))
				{
					if (row + 1 < mapData.Height)
					{
						Tile aboveTile = mapData[row + 1, col];
						if (IsNormalCollision(aboveTile))
						{
							continue;
						}
					}
					else
					{
						return;
					}
					floorRect.x = col;
					floorRect.y = row;
					
					while (IsInBounds(row + 1, col + 1))
					{
						currentTile = mapData[row, col + 1];
						if (row + 1 < mapData.Height)
						{
							Tile aboveTile = mapData[row + 1, col];
							if (IsNormalCollision(aboveTile))
							{ 
								--col;
								break;
							}
						}
						if (IsNormalCollision(currentTile))
						{
							++col;
						}
						else
						{
							break;
						}
					} 
					
					float floorWidth = col - floorRect.x + 1;
					
					floorRect.width  = floorWidth;
					floorRect.height = 1;
					
					RectangleMesh floorMesh = new RectangleMesh(floorRect);
					floorMesh.BuildHorizontalMesh();
					meshes.Add (floorMesh.Mesh);
				}
				
			}
		}
	}

	private void GetCeilingCollision ()
	{
		Rect ceilingRect = new Rect();
		
		for (int row = mapData.Height - 1; row >= 0; --row)
		{
			for (int col = 0; col < mapData.Width; ++col)
			{
				Tile currentTile = mapData[row, col];
				if (IsNormalCollision(currentTile))
				{
					if (row - 1 >= 0)
					{
						Tile belowTile = mapData[row - 1, col];
						if (IsNormalCollision(belowTile))
						{
							continue;
						}
					}
					else
					{
						continue;
					}
					ceilingRect.x = col;
					ceilingRect.y = row;
					
					while (IsInBounds(row - 1, col + 1))
					{
						currentTile = mapData[row, col + 1];
						if (row - 1 >= 0)
						{
							Tile belowTile = mapData[row - 1, col];
							if (IsNormalCollision(belowTile))
							{ 
								--col;
								break;
							}
						}
						if (IsNormalCollision(currentTile))
						{
							++col;
						}
						else
						{
							break;
						}
					}
					
					float floorWidth = col - ceilingRect.x + 1;
					
					ceilingRect.width  = floorWidth;
					ceilingRect.height = 0;
					
					RectangleMesh ceilingMesh = new RectangleMesh(ceilingRect);
					ceilingMesh.BuildHorizontalMesh();
					ceilingMesh.InvertMesh();
					meshes.Add (ceilingMesh.Mesh);
				}
			}
		}
	}

	private void GetLeftCollision ()
	{
		Rect leftRect = new Rect();
		
		for (int col = 0; col < mapData.Width; ++col)
		{
			for (int row = 0; row < mapData.Height; ++row)
			{
				Tile currentTile = mapData[row, col];
				if (IsNormalCollision(currentTile))
				{
					if (col - 1 >= 0 )
					{
						Tile leftTile = mapData[row, col - 1];
						if (IsNormalCollision(leftTile))
						{
							continue;
						}
					}
					else
					{
						continue;
					}
					
					leftRect.x = col;
					leftRect.y = row;
					
					while (row + 1 < mapData.Height)
					{
						currentTile = mapData[row + 1, col];
						if (col - 1 >= 0 )
						{
							Tile leftTile = mapData[row, col - 1];
							if (IsNormalCollision(leftTile))
							{
								--row;
								break;
							}
						}
						if (IsNormalCollision(currentTile))
						{
							++row;
						}
						else
						{
							break;
						}
					}
					leftRect.width = 0;
					leftRect.height = row - leftRect.y + 1;
					
					
					RectangleMesh leftMesh = new RectangleMesh(leftRect);
					leftMesh.BuildVerticalMesh();
					meshes.Add(leftMesh.Mesh);
				}
			}
		}
	}

	private void GetRightCollision ()
	{
		Rect rightRect = new Rect();

		for (int col = 0; col < mapData.Width; ++col)
		{
			for (int row = 0; row < mapData.Height; ++row)
			{
				Tile currentTile = mapData[row, col];
				if (IsNormalCollision(currentTile))
				{
					if (col + 1 < mapData.Width)
					{
						Tile rightTile = mapData[row, col + 1];
						if (IsNormalCollision(rightTile))
						{
							continue;
						}
					}
					else
					{
						return;
					}

					rightRect.x = col + 1;
					rightRect.y = row;

					while (row + 1 < mapData.Height)
					{
						currentTile = mapData[row + 1, col];
						if (col + 1 < mapData.Width)
						{
							Tile rightTile = mapData[row, col + 1];
							if (IsNormalCollision(rightTile))
							{
								--row;
								break;
							}
						}
						if (IsNormalCollision(currentTile))
						{
							++row;
						}
						else
						{
							break;
						}
					}
					rightRect.width = 0;
					rightRect.height = row - rightRect.y + 1;


					RectangleMesh rightMesh = new RectangleMesh(rightRect);
					rightMesh.BuildVerticalMesh();
					rightMesh.InvertMesh();
					
					meshes.Add(rightMesh.Mesh);
				}
			}
		}

	}
	private bool IsNormalCollision(Tile tile)
	{
		if (tile.CollisionType == CollisionType.NormalCollision)
		{
			return true;
		}
		return false;
	}
	private bool IsInBounds(int row, int col)
	{
		if (row < 0 || row >= mapData.Height)
		{
			return false;
		}
		if (col < 0 || col >= mapData.Width)
		{
			return false;
		}

		return true;
	}

	private void GetDiagonalCollision ()
	{
		Rect diagonalRect = new Rect();

		for (int row = 0; row < mapData.Height; ++row)
		{
			for (int col = 0; col < mapData.Width; ++col)
			{
				Tile currentTile = mapData[row, col];
				if (IsDiagonalCollision(currentTile))
				{
					if (IsLowerCollision(currentTile))
					{
						diagonalRect.x      = col;
						diagonalRect.y      = row;
						diagonalRect.width  = 1f;
						diagonalRect.height = 0.5f;
					}
					else // upper collision
					{
						diagonalRect.x      = col;
						diagonalRect.y      = row + 0.5f;
						diagonalRect.width  = 1f;
						diagonalRect.height = 0.5f;
					}

					if (IsLeftCollision(currentTile))
					{
						diagonalRect.x += 1;
						diagonalRect.width = -diagonalRect.width;

					}
					else
					{
					}

					RectangleMesh diagRectMesh = new RectangleMesh(diagonalRect);
					diagRectMesh.BuildDiagonalMesh();

					if (IsTopCollision(currentTile) || IsLeftCollision(currentTile))
					{
						diagRectMesh.InvertMesh();
					}



					meshes.Add(diagRectMesh.Mesh);
				}

			}
		}
	}
	private bool IsDiagonalCollision(Tile tile)
	{
		switch (tile.CollisionType)
		{
			case CollisionType.NoCollision:
			case CollisionType.NormalCollision:
			case CollisionType.Ladder:
			{
				return false;
			}
			default:
			{
				return true;
			}
		}
	}
	private bool IsLowerCollision(Tile tile)
	{
		switch(tile.CollisionType)
		{
			case CollisionType.BottomLowerLeft:
			case CollisionType.BottomLowerRight:
			case CollisionType.TopLowerRight:
			case CollisionType.TopLowerLeft:
			{
				return true;
			}
			default:
			{
				return false;
			}
		}
	}	
	private bool IsTopCollision(Tile tile)
	{
		switch(tile.CollisionType)
		{
			
			case CollisionType.TopLowerRight:
			case CollisionType.TopLowerLeft:
			case CollisionType.TopUpperRight:
			case CollisionType.TopUpperLeft:
			{
				return true;
			}
			default:
			{
				return false;
			}
		}
	}
	private bool IsLeftCollision(Tile tile)
	{
		switch(tile.CollisionType)
		{

			case CollisionType.BottomLowerLeft:
			case CollisionType.BottomUpperLeft:
			case CollisionType.TopLowerLeft:
			case CollisionType.TopUpperLeft:
			{
				return true;
			}
			default:
			{
				return false;
			}
		}
	}
	private CombineInstance[] CombineMeshes(List<Mesh> meshes)
	{
		CombineInstance[] meshesToCombine = new CombineInstance[meshes.Count];
		for (int i = 0; i < meshesToCombine.Length; ++i)
		{
			meshesToCombine[i].mesh = meshes[i];
		}
		return meshesToCombine;
	}
}
