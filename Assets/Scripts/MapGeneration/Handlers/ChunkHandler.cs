using UnityEngine;
using System.Collections;

public class ChunkHandler : MonoBehaviour {

	public static ChunkHandler chunkHandler;

	public TextAsset chunkTextFile;

	public static Chunk[] chunks;

	void Awake()
	{
		if (chunkHandler != null)
		{
			GameObject.Destroy(chunkHandler);
		}
		else
		{
			chunkHandler = this;
		}
		
		DontDestroyOnLoad(this);

		ChunkReader chunkReader = new ChunkReader(chunkTextFile);

	}
	
	public static string ToString()
	{
		string retStr = "";
		for (int i = 0; i < chunks.Length; ++i)
		{
			if (chunks[i] == null)
			{
				retStr += " \nNULL\n ";
			}
			else
			{
				retStr += "\nChunk #" + i + "\n";
				retStr += chunks[i].ToString();
			}

		}

		return retStr;
	}

}
