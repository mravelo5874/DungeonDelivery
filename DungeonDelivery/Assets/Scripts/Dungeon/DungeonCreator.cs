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
        var test_dungeon = DungeonGenerator.instance.create(DungeonSize.MEDIUM);    
        construct(test_dungeon);
    }

    public void construct(Dungeon dungeon)
    {
        // place each chunk in world space
        foreach (var chunk in dungeon.chunks)
        {
            Vector3 pos = new Vector3(chunk.x * chunkUnit, 0f, chunk.y);
            Instantiate(chunkObject, pos, Quaternion.identity, chunkParent);
        }
    }
}
