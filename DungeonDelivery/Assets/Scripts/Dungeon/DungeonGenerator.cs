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
    [Header("Main Path Length")]
    public int pathLenSmall;
    public int pathLenMedium;
    public int pathLenLarge;
    public int pathLenXLarge;

    [Header("Number of Special Rooms")]
    public int specialRoomsSmall;
    public int specialRoomsMedium;
    public int specialRoomsLarge;
    public int specialRoomsXLarge;

    void Start() 
    {
        var test_dungeon = create(DungeonSize.LARGE);
        print_dungeon(test_dungeon);
    }

    public Dungeon create(DungeonSize size)
    {
        var dungeon = new Dungeon();
        int path_len = 0;
        int special_rooms = 0;
        List<Vector2Int> vaildStartPos = new List<Vector2Int>();

        switch (size)
        {
            default:
            case DungeonSize.SMALL:
                path_len = pathLenSmall;
                special_rooms = specialRoomsSmall;
                break;
            case DungeonSize.MEDIUM:
                path_len = pathLenMedium;
                special_rooms = specialRoomsMedium;
                break;
            case DungeonSize.LARGE:
                path_len = pathLenLarge;
                special_rooms = specialRoomsLarge;
                break;
            case DungeonSize.X_LARGE:
                path_len = pathLenXLarge;
                special_rooms = specialRoomsXLarge;
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
            List<Vector2Int> validDirections = new List<Vector2Int>();

            foreach(var cardinalDirection in (CardinalDirection[]) System.Enum.GetValues(typeof(CardinalDirection)))
            {
                var potential_x = curr_x;
                var potential_y = curr_y;

                switch (cardinalDirection)
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

                Vector2Int vector = new Vector2Int(potential_x, potential_y);

                // check to see if room is valid and empty
                if (!path.Contains(vector))
                {
                    validDirections.Add(vector);
                }
            }

            // if no vaild directions, go back one room and try again
            if (validDirections.Count == 0)
            {
                if(path.Count != 0) //prevent IndexOutOfRangeException for empty list
                {
                    path.RemoveAt(path.Count - 1);
                    path_left++;
                    continue;
                }
            }

            // choose a random direction from vaild rooms
            Vector2Int direction = validDirections[Random.Range(0, validDirections.Count)];

            // add valid room to path
            path.Add(direction);
            curr_x = direction.x;
            curr_y = direction.y;
            path_left--;

            // add room to list
            if (path_left != 0)
                vaildStartPos.Add(direction);
        }

        // add special room paths
        int special_rooms_left = special_rooms;
        while (special_rooms_left > 0)
        {
            // choose a random room from the path (except the end room)
            int index = Random.Range(0, vaildStartPos.Count);
            Vector2Int startRoom = vaildStartPos[index];

            path_left = Random.RandomRange(0, )


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