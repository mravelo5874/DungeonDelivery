using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkType // [number of entrances] _ [room shape]
{
    EMPTY, NONE_CLOSED, ONE_DEADEND, TWO_CORNER, TWO_STRAIGHT, THREE_T, FOUR_ALL
}

public enum RoomType
{
    SPAWN, END, PATH
}

public class Chunk
{
    public Chunk(int _x, int _y)
    {
        x = _x;
        y = _y;
        chunkType = ChunkType.EMPTY;
    }

    public int x;
    public int y;

    public bool n;
    public bool s;
    public bool e;
    public bool w;

    public ChunkType chunkType;
    public RoomType roomType;
}
