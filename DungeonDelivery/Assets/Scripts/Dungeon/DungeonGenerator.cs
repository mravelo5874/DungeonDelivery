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

    [Header("Special Path Length Maximum")]
    public int specialPathSmall;
    public int specialPathMedium;
    public int specialPathLarge;
    public int specialPathXLarge;

    public static DungeonGenerator instance;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Dungeon create(DungeonSize size)
    {
        var dungeon = new Dungeon();
        int path_len = 0;
        int special_rooms = 0;
        int max_special_path = 0;
        List<Vector2Int> vaildStartPos = new List<Vector2Int>();

        switch (size)
        {
            default:
            case DungeonSize.SMALL:
                path_len = pathLenSmall;
                special_rooms = specialRoomsSmall;
                max_special_path = specialPathSmall;
                break;
            case DungeonSize.MEDIUM:
                path_len = pathLenMedium;
                special_rooms = specialRoomsMedium;
                max_special_path = specialPathMedium;
                break;
            case DungeonSize.LARGE:
                path_len = pathLenLarge;
                special_rooms = specialRoomsLarge;
                max_special_path = specialPathLarge;
                break;
            case DungeonSize.X_LARGE:
                path_len = pathLenXLarge;
                special_rooms = specialRoomsXLarge;
                max_special_path = specialPathXLarge;
                break;
        }

        // path from spawn to end room
        List<Vector2Int> main_path = new List<Vector2Int>();
        main_path = CreatePath(new Vector2Int(0,0), path_len, dungeon);
        PathToChunks(main_path, dungeon, true);

        // add all main path rooms (except the end room)
        vaildStartPos.AddRange(main_path);
        vaildStartPos.RemoveAt(vaildStartPos.Count - 1);

        // add special room paths
        int special_rooms_left = special_rooms;
        List<List<Vector2Int>> special_paths = new List<List<Vector2Int>>();
        while (special_rooms_left > 0)
        {
            // choose a random room from the path (except the end room)
            int index = Random.Range(0, vaildStartPos.Count);
            Vector2Int startRoom = vaildStartPos[index];
            vaildStartPos.RemoveAt(index);

            path_len = Random.Range(1, max_special_path + 1);
            var special_path = CreatePath(startRoom, path_len, dungeon);
            PathToChunks(special_path, dungeon);

            // add to special paths if not null
            if (special_path != null)
            {
                special_rooms_left--;
                special_paths.Add(special_path);
            }
        }

        int count = 0;
        print ("main path:");
        foreach (var room in main_path)
        {
            print (count + " [" + room.x + ", " + room.y + "]");
            count++;
        }

        int path_num = 0;
        foreach (var path in special_paths)
        {
            print ("special path " + path_num + ":");
            path_num++;
            count = 0;
            foreach (var room in path)
            {
                print (count + " [" + room.x + ", " + room.y + "]");
                count++;
            }
        }

        // determine each chunk type
        foreach(var chunk in dungeon.chunks)
        {

        }

        return dungeon;
    }

    private void PathToChunks(List<Vector2Int> path, Dungeon dungeon, bool main_path = false)
    {
        // add paths to dungeon
        int count = 0;
        dungeon.chunks = new List<Chunk>();
        Chunk prev_chunk = new Chunk(0, 0);
        foreach(var room in path)
        {
            var chunk = new Chunk(room.x, room.y);
            
            // set room type
            if (count == 0 && main_path)
                chunk.roomType = RoomType.SPAWN;
            else if (count == path.Count - 1)
            {
                if (main_path)
                    chunk.roomType = RoomType.END;
                else
                    chunk.roomType = RoomType.SPECIAL;
            } 
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
    }

    private List<Vector2Int> CreatePath(Vector2Int start, int length, Dungeon dungeon)
    {
        var path = new List<Vector2Int>();

        path.Add(start);

        int path_left = length;
        int curr_x = start.x;
        int curr_y = start.y;

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
                if (!path.Contains(vector) && !dungeon.isChunkRoom(vector.x, vector.y))
                {
                    validDirections.Add(vector);
                }
            }

            // if no vaild directions, go back one room and try again
            if (validDirections.Count == 0)
            {
                if (path.Count != 0) //prevent IndexOutOfRangeException for empty list
                {
                    path.RemoveAt(path.Count - 1);
                    path_left++;
                    continue;
                }
                else
                {
                    return null; // no possible path found
                }
            }

            // choose a random direction from vaild rooms
            Vector2Int direction = validDirections[Random.Range(0, validDirections.Count)];

            // add valid room to path
            path.Add(direction);
            curr_x = direction.x;
            curr_y = direction.y;
            path_left--;
        }

        return path;
    }

    public void print_dungeon(Dungeon dungeon)
    {
        
    }
}