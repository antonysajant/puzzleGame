using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] bool isHorizontal = true;
    Rigidbody2D rb;
    DragController dg;

    void Awake()
    {
        dg = FindObjectOfType<DragController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool getBlock()
    {
        return isHorizontal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Block" || collision.transform.tag == "Border")
        {
            dg.dragTargetPos = rb.position;
            dg.canDrag = false;
        }
    }
}
