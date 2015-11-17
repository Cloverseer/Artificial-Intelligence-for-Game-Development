using System.IO;
using System.Text;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChunkReader
{
	private const char ChunkSeperator = '=';

	private TextAsset chunkTextFile;
	private List<int[ , ]> chunks;
	
	private string fileContents;


	public ChunkReader(TextAsset chunkFile)
	{
		chunkTextFile = chunkFile;

		if (HasChunkTextFile())
		{
			ParseFile();
		}
	}

	public int TotalChunks
	{
		get;
		private set;
	}
	private bool HasChunkTextFile()
	{
		return chunkTextFile != null;
	}

	private void ParseFile()
	{
		//TODO make this function not overly disgusting 
		fileContents = chunkTextFile.text;
		fileContents = fileContents.Trim ();

		string[] rows = fileContents.Split ('\n');
		TotalChunks = GetTotalChunks(rows);
		ChunkHandler.chunks = new Chunk[TotalChunks];

		List<List<int>> chunk = new List<List<int>> ();

		int chunkNum = 0;
		for (int i = 0; i < rows.Length; ++i)
		{
			string currentRow = rows[i];
			
			List<int> rowData = new List<int> ();
			
			currentRow = currentRow.Trim ();
			
			if (currentRow.Length > 0)
			{
				while(IsChunkSeperator(currentRow))
				{
					if (chunk.Count > 0)
					{
						Chunk actualChunk = new Chunk(chunk[0].Count, chunk.Count);
						
						for (int row = 0; row < chunk.Count; ++row)
						{
							for (int col = 0; col < chunk[0].Count; ++col)
							{
								actualChunk[actualChunk.Height - 1 - row, col] = chunk[row][col];
							}
						}
						ChunkHandler.chunks[chunkNum] = actualChunk;
						++chunkNum;
					}
					chunk = new List<List<int>> ();
					
					++i;
					if (i >= rows.Length)
					{
						return;
					}
					currentRow = rows[i];
					currentRow = currentRow.Trim();
				}
				
				rowData = new List<int>();
				foreach (string number in currentRow.Split(' '))
				{
					if (number.Length == 0) 
					{
						continue;
					}
					int numToAdd = int.Parse(number);
					if (numToAdd == -1)
					{
						numToAdd = TileHandler.tiles.Length - 1;
					}
					rowData.Add (numToAdd);
				}
				chunk.Add(rowData);
				
			}
		}

	}

	private bool IsChunkSeperator(string line)
	{
		if (line.Length == 0)
		{
			return false;
		}
		if (line[0] == ChunkSeperator)
		{
			return true;
		}
		return false;
	}

	private bool IsEmpty(string str)
	{
		if (str.Length == 0)
		{
			return true;
		}

		return false;
	}
	private string[] Trim(string[] lines)
	{
		string[] newLines = lines;
		for (int i = 0; i < newLines.Length; ++i) 
		{
			newLines[i] = newLines[i].Trim();
		}

		return newLines;
	}

	private int GetTotalChunks(string[] lines)
	{
		int chunkCount = 0;
		for (int i = 0; i < lines.Length; ++i)
		{
			if (i == lines.Length - 1)
			{
				break;
			}
			if (IsChunkSeperator(lines[i]))
			{
				++chunkCount;
			}
		}
		return chunkCount;
	}
	private int GetChunkSize(string[] lines, int startIndex)
	{
		int chunkRowNum = startIndex + 1;
		int chunkSize = 0;
		if (chunkRowNum < lines.Length)
		{
			while (!IsChunkSeperator(lines[chunkRowNum]))
			{
				Debug.Log("Line Size :" + lines[chunkRowNum].Length);
				if (lines[chunkRowNum].Length != 0)
				{
					++chunkSize;
					++chunkRowNum;
				}
				else
				{
					break;
				}
				if (chunkRowNum >= lines.Length) 
				{
					break;
				}
			}
		}

		for (int i = 0; i < chunkSize; ++i)
		{
			Debug.Log("Value at 0: " + (int) lines[startIndex + 1 + i][0]);
		}
		return chunkSize;
	}

}
