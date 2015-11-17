
public class Chunk 
{

	private int width;
	private int height;

	private int[ , ] tileIds;

	public Chunk(int width, int height)
	{
		Width  = width;
		Height = height;
		tileIds = new int[height, width];
	}
	public int this[int row, int col]
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
			return tileIds[row, col];
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
			tileIds[row, col] = value;
		}
	}

	public int Width
	{
		get
		{
			return width;
		}
		private set
		{
			if (value < 0)
			{
				throw new System.ArgumentOutOfRangeException("Width must be >= 0");
			}
			width = value;
		}
	}
	
	public int Height
	{
		get
		{
			return height;
		}
		private set
		{
			if (value < 0)
			{
				throw new System.ArgumentOutOfRangeException("Height must be >= 0");
			}
			height = value;
		}
	}

	public int X
	{
		get;
		set;
	}
	public int Y
	{
		get;
		set;
	}

	public string ToString()
	{
		string retStr = "";
		for (int row = 0; row < Height; ++row)
		{
			for (int col = 0; col < Width; ++col)
			{
				retStr += this[row, col] + " "; 
			}
			retStr += "\n";
		}

		return retStr;
	}
}
