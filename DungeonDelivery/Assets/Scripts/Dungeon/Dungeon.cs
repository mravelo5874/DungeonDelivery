using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public List<Chunk> chunks;

    public bool isChunkRoom(int x, int y)
    {
        foreach (var chunk in chunks)
        {
            if (chunk.x == x && chunk.y == y)
                return true;
        }
        return false;
    }
}
