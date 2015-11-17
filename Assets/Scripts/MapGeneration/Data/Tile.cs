
public enum CollisionType
{
	NoCollision,
	NormalCollision,
	BottomLowerRight,
	BottomUpperRight,
	BottomLowerLeft,
	BottomUpperLeft,
	TopLowerRight,
	TopUpperRight,
	TopLowerLeft,
	TopUpperLeft,
	Ladder
}

public class Tile
{
	public Tile(int id, CollisionType collisionType)
	{
		Id            = id;
		CollisionType = collisionType;
	}

	public int Id
	{
		get;
		private set;
	}

	public CollisionType CollisionType
	{
		get;
		set;
	}

	public bool IsCollidable()
	{

		return IsDiagonalCollision (this) ||
			IsLowerCollision (this) ||
			IsNormalCollision (this) ||
			IsLeftCollision (this) ||
			IsTopCollision (this);

	
	}
	private bool IsNormalCollision(Tile tile)
	{
		if (tile.CollisionType == CollisionType.NormalCollision)
		{
			return true;
		}
		return false;
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
}
