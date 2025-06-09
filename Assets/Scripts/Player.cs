using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    bool isIdle = true;
    Rigidbody2D rb;
    public Vector3 newPos;
    [SerializeField] Vector3 current;
    [SerializeField] GameObject checkUp;
    [SerializeField] GameObject checkDown;
    [SerializeField] GameObject checkLeft;
    [SerializeField] GameObject checkRight;
    DragController dg;
    [SerializeField] bool dgmove;
    [SerializeField] GameObject waypoint;
    [SerializeField] float yOffset;
    [SerializeField] float xOffset;
    public bool wpcreated = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        newPos = transform.position;
        dg = FindObjectOfType<DragController>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckForMove), 0f, 1f);
        newPos = Vector3.zero;
    }

    private void Update()
    {
        dgmove = dg.move;
        if (isIdle) //if idle do idle animation
            anim.SetBool("idleD", true);

        if (!isIdle && dg.move)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, 0.01f);

            if (Vector3.Distance(transform.position, newPos) < 0.02f)
            {
                goIdle();
            }
        }
            
        current = transform.position;
    }

    [ContextMenu("Idle")]
    void goIdle()   //triggers run animations off and returns to idle state
    {
        anim.SetBool("runL", false);
        anim.SetBool("runR", false);
        anim.SetBool("runU", false);
        anim.SetBool("runD", false);
        isIdle = true;
        anim.SetBool("idleD", true);
    }

    public void startMove() // sets isIdle and animation to false
    {
        isIdle = false;
        anim.SetBool("idleD", false);
        Debug.Log($"Moving to {newPos} from {transform.position}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Gates")
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        if (collision.transform.tag == "Block")
            goIdle();
    }

    bool IsGateAt(Vector3 position) 
    {
        Collider2D hit = Physics2D.OverlapPoint(position);
        if (hit.transform.tag == "Gates")
            return true;
        else
            return false;
    }

    void CheckForMove()
    {
        if (!dg.move) return;

        if (!isIdle) return;

        if (wpcreated) return;

        GameObject waypoints = new GameObject("Waypoints");

        Vector3 x;
        Vector3 xoff = new Vector3(xOffset, 0, 0);
        Vector3 yoff = new Vector3(0, yOffset, 0);
        if (Physics2D.OverlapPoint(checkUp.transform.position) == null || IsGateAt(checkUp.transform.position))
        {
            x = Vector3.zero;
            do
            {
                GameObject wp=Instantiate(waypoint,checkUp.transform.position + x-yoff, Quaternion.identity);
                wp.transform.parent=waypoints.transform;
                x = x + Vector3.up;
            } while (Physics2D.OverlapPoint(checkUp.transform.position + x) == null || IsGateAt(checkUp.transform.position + x)) ;
        }
        if (Physics2D.OverlapPoint(checkLeft.transform.position) == null)
        {
            x = Vector3.zero;
            do
            {
                GameObject wp = Instantiate(waypoint, checkLeft.transform.position + x +xoff-yoff, Quaternion.identity);
                wp.transform.parent = waypoints.transform;
                x = x + Vector3.left;
            } while (Physics2D.OverlapPoint(checkLeft.transform.position + x) == null);
        }
        if (Physics2D.OverlapPoint(checkRight.transform.position) == null)
        {
            x = Vector3.zero;
            do
            {
                GameObject wp = Instantiate(waypoint, checkRight.transform.position + x -xoff-yoff, Quaternion.identity);
                wp.transform.parent = waypoints.transform;
                x = x + Vector3.right;
            } while (Physics2D.OverlapPoint(checkRight.transform.position + x) == null);
        }
        if (Physics2D.OverlapPoint(checkDown.transform.position) == null || !IsGateAt(checkDown.transform.position))
        {
            x = Vector3.zero;
            do
            {
                GameObject wp = Instantiate(waypoint, checkDown.transform.position + x + yoff, Quaternion.identity);
                wp.transform.parent = waypoints.transform;
                x = x + Vector3.down;
            } while (Physics2D.OverlapPoint(checkDown.transform.position + x) == null || !IsGateAt(checkDown.transform.position + x));
        }
        wpcreated = true;
    }
}
