using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] bool isHorizontal = true;
    Rigidbody2D rb;
    DragController dg;
    bool run = false;

    void Awake()
    {
        dg = FindObjectOfType<DragController>();
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        if (dg.canDrag == true)
            run = false;
        if (run)
        cannotDrag();
    }

    public bool getBlock()
    {
        return isHorizontal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Block" || collision.transform.tag == "Border" || collision.transform.tag == "Gates")
        {
            dg.dragTargetPos = rb.position;
            dg.canDrag = false;
            run = true;
        }
    }

    void cannotDrag()
    {
        dg.canDrag = false;
    }
}
