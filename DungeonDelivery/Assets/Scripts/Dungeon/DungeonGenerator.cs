using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonSize 
{
    SMALL, MEDIUM, LARGE, X_LARGE
}

public enum CardinalDirection
{
    NORTH, SOUTH, EAST, WEST
}

public class DungeonGenerator : MonoBehaviour
{
    public int pathLenSmall;
    public int pathLenMedium;
    public int pathLenLarge;
    public int pathLenXLarge;

    void Start() 
    {
        var test_dungeon = create(DungeonSize.LARGE);
        print_dungeon(test_dungeon);
    }

    public Dungeon create(DungeonSize size)
    {
        var dungeon = new Dungeon();
        int path_len = 0;

        switch (size)
        {
            default:
            case DungeonSize.SMALL:
                path_len = pathLenSmall;
                break;
            case DungeonSize.MEDIUM:
                path_len = pathLenMedium;
                break;
            case DungeonSize.LARGE:
                path_len = pathLenLarge;
                break;
            case DungeonSize.X_LARGE:
                path_len = pathLenXLarge;
                break;
        }

        // path from spawn to end room
        List<Vector2Int> path = new List<Vector2Int>();

        // choose spawn room
        int curr_x = 0;
        int curr_y = 0;

        path.Add(new Vector2Int(curr_x, curr_y));

        int path_left = path_len;
        while (path_left > 0)
        {
            // choose a random direction
            var direction = (CardinalDirection)Random.Range(0, 4);

            var potential_x = curr_x;
            var potential_y = curr_y;

            switch (direction)
            {   
                default:
                case CardinalDirection.NORTH:
                    potential_y = curr_y + 1;
                    break;
                case CardinalDirection.SOUTH:
                    potential_y = curr_y - 1;
                    break;
                case CardinalDirection.EAST:
                    potential_x = curr_x + 1;
                    break;
                case CardinalDirection.WEST:
                    potential_x = curr_x - 1;
                    break;
            }

            // check to see if room is valid and empty
            if (path.Contains(new Vector2Int(potential_x, potential_y)))
                continue;
            else
            {
                // add valid room to path
                path.Add(new Vector2Int(potential_x, potential_y));
                curr_x = potential_x;
                curr_y = potential_y;

                // add path from prev room


                path_left--;
            }
        }

        // add path to dungeon
        int count = 0;
        dungeon.chunks = new List<Chunk>();
        Chunk prev_chunk = new Chunk(0, 0);
        foreach(var room in path)
        {
            var chunk = new Chunk(room.x, room.y);
            
            // set room type
            if (count == 0)
                chunk.roomType = RoomType.SPAWN;
            else if (count == path_len)
                chunk.roomType = RoomType.END;
            else 
                chunk.roomType = RoomType.PATH;

            // set chunk + prev_chunk entrances
            if (count != 0)
            {
                // North South
                if (chunk.x != prev_chunk.x)
                {
                    if (chunk.x > prev_chunk.x)
                    {
                        chunk.s = true;
                        prev_chunk.n = true;
                    }
                    else
                    {
                        chunk.n = true;
                        prev_chunk.s = true;
                    }
                }
                // East West
                else if (chunk.y != prev_chunk.y)
                {
                    if (chunk.y > prev_chunk.y)
                    {
                        chunk.w = true;
                        prev_chunk.e = true;
                    }
                    else
                    {
                        chunk.e = true;
                        prev_chunk.w = true;
                    }
                }
            }

            dungeon.chunks.Add(chunk);
            prev_chunk = chunk;
            count++;
        }

        // add special room paths


        // determine chunk type
        foreach(var chunk in dungeon.chunks)
        {

        }

        return dungeon;
    }

    public void print_dungeon(Dungeon dungeon)
    {
        // determine max + min width and heigh
        int max_x = int.MinValue;
        int min_x = int.MaxValue;
        int max_y = int.MinValue;
        int min_y = int.MaxValue;

        int count = 0;
        foreach(var chunk in dungeon.chunks)
        {
            print (count + " chunk: " + chunk.x + ", " + chunk.y);
            count++;

            if (chunk.x > max_x)
                max_x = chunk.x;
            if (chunk.x < min_x)
                min_x = chunk.x;
            if (chunk.y > max_y)
                max_y = chunk.y;
            if (chunk.y < min_y)
                min_y = chunk.y;
        }

        for (int y = min_y; y <= max_y; y++)
        {
            string row = "";
            for (int x = min_x; x <= max_x; x++)
            {
                if (dungeon.isChunkRoom(x, y))
                    row += "["+x+y+"]";
                else
                    row += "----";
            }
            print (row);
        }
    }
}
