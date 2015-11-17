
public class MapData
{
	private int width;
	private int height;

	public MapData(int width, int height)
	{
		Width  = width;
		Height = height;

		TileGrid = new Tile[height, width];
	}


	public Tile[ , ] TileGrid
	{
		get;
		private set;
	}

	public Tile this[int row, int col]
	{
		get
		{
			if (row < 0 || row >= Height)
			{
				throw new System.IndexOutOfRangeException("Row must be >= 0 and less than " + Height + "(Height)");
			}
			if (col < 0 || col >= Width)
			{
				throw new System.IndexOutOfRangeException("Column must be >= 0 and less than " + Width + "(Width)");
			}
			return TileGrid[row, col];
		}
		set
		{
			if (row < 0 || row >= Height)
			{
				throw new System.IndexOutOfRangeException("Row must be >= 0 and less than " + Height + "(Height)");
			}
			if (col < 0 || col >= Width)
			{
				throw new System.IndexOutOfRangeException("Column must be >= 0 and less than " + Width + "(Width)");
			}
			TileGrid[row, col] = value;
		}
	}

	public void SetTiles(Chunk tiles)
	{
		int colBound = tiles.X + tiles.Width;
		int rowBound = tiles.Y + tiles.Height;

		int tileRow = 0;
		for (int row = tiles.Y; row < rowBound; ++row)
		{
			int tileCol = 0;
			for (int col = tiles.X; col < colBound; ++col)
			{
				Tile tileToSet = TileHandler.tiles[tiles[tileRow, tileCol]];
				TileGrid[row, col] = tileToSet;

				++tileCol;
			}

			++tileRow;
		}
	}
	public void SetTiles(Chunk tiles, int x, int y)
	{
		int colBound = x + tiles.Width;
		int rowBound = y + tiles.Height;
		
		int tileRow = 0;
		for (int row = y; row < rowBound; ++row)
		{
			int tileCol = 0;
			for (int col = x; col < colBound; ++col)
			{
				Tile tileToSet = TileHandler.tiles[tiles[tileRow, tileCol]];
				TileGrid[row, col] = tileToSet;
				
				++tileCol;
			}
			
			++tileRow;
		}
	}

	public int Width
	{
		get
		{
			return width;
		}
		set
		{
			if (value < 0)
			{
				throw new System.ArgumentOutOfRangeException("Width must be >= 0");
			}
			TileGrid = new Tile[Height, value];
			width = value;
		}
	}

	public int Height
	{
		get
		{
			return height;
		}
		set
		{
			if (value < 0)
			{
				throw new System.ArgumentOutOfRangeException("Height must be >= 0");
			}
			TileGrid = new Tile[value, Width];
			height = value;
		}
	}

	public string ToString()
	{
		string str = "Height = " + TileGrid.GetLength(0) + " Width = " + TileGrid.GetLength(1) + "\n";
		for (int row = 0; row < Height; ++row)
		{
			for (int col = 0; col < width; ++col)
			{
				str += this[row, col].Id + " ";
			}
			str += "\n";
		}

		return str;
	}
}
