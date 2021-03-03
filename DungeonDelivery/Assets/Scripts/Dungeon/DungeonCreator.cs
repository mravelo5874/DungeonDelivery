using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public Vector3 origin;
    public float chunkUnit;
    public GameObject chunkObject;
    public Transform chunkParent;

    private void Start() 
    {
        var test_dungeon = DungeonGenerator.instance.create(DungeonSize.X_LARGE);    
        construct(test_dungeon);
    }

    public void construct(Dungeon dungeon)
    {
        print ("num chunks: " + dungeon.chunks.Count);

        // place each chunk in world space
        int count = 0;
        foreach (var chunk in dungeon.chunks)
        {
            print (count + " chunk: " + chunk.x + " " + chunk.y);
            Vector3 pos = new Vector3(chunk.x * chunkUnit, 0f, chunk.y * chunkUnit);
            Instantiate(chunkObject, pos, Quaternion.identity, chunkParent);
        }
    }
}
