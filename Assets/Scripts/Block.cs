using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] bool isHorizontal = true;

    public bool getBlock()
    {
        return isHorizontal;
    }
}
