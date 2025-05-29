using UnityEngine;

[System.Serializable]
public class BlockData
{
    public int startX, startY, length;
    public bool isHorizontal,isGoal;
}

public class GridManager : MonoBehaviour
{
    [SerializeField] int rows = 6;
    [SerializeField] int cols = 6;
    [SerializeField] float cellsize = 0.5f;
    [SerializeField] Vector2 startPos = new Vector2(-2.75f, -2.75f);

    [SerializeField] GameObject normaBlock;
    [SerializeField] GameObject goalBlock;

    [SerializeField] BlockData[] levelBlocks;

    int[,] logicalGrid;

    void Start()
    {
        logicalGrid = new int[cols, rows];

        foreach (BlockData data in levelBlocks)
        {
            if (!IsWithinBounds(data)) continue;

            Vector2 pos = startPos + new Vector2(data.startX * cellsize, data.startY * cellsize);
            GameObject prefab = data.isGoal ? goalBlock : normaBlock;
            GameObject block = Instantiate(prefab, pos, Quaternion.identity, transform);
            block.name = $"Block_{data.startX}_{data.startY}";

            if (data.isHorizontal)
                block.transform.localScale = new Vector3(transform.localScale.x * data.length, transform.localScale.y, transform.localScale.z);
            else
                block.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * data.length, transform.localScale.z);

            for (int i = 0; i < data.length; i++)
            {
                int x = data.startX + (data.isHorizontal ? i : 0);
                int y = data.startY + (data.isHorizontal ? 0 : i);
                logicalGrid[x, y] = 1;
            }
        }
    }

    bool IsWithinBounds(BlockData data)
    {
        if (data.isHorizontal)
            return data.startX + data.length <= cols;
        else
            return data.startY + data.length <= rows;
    }


    void Update()
    {
        

    }
}
