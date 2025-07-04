using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    bool isIdle = true;
    Rigidbody2D rb;
    public Vector3 newPos;
    [SerializeField] GameObject checkUp;
    [SerializeField] GameObject checkDown;
    [SerializeField] GameObject checkLeft;
    [SerializeField] GameObject checkRight;
    DragController dg;
    [SerializeField] GameObject waypoint;
    GameObject waypoints=null;
    Collider2D gateCollider;
    [SerializeField] float speed = 0.05f; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        newPos = transform.position;
        dg = FindObjectOfType<DragController>();
        gateCollider = GameObject.Find("Gates").GetComponent<Collider2D>();
    }

    private void Start()
    {
        //InvokeRepeating(nameof(CheckForMove), 0.75f, 0.5f);
        
        newPos = Vector3.zero;
        Physics2D.IgnoreCollision(gateCollider, GetComponent<Collider2D>());
        CheckForMove();
    }

    private void Update()
    {
        if (isIdle) //if idle do idle animation
            anim.SetBool("idleD", true);

        if (!isIdle)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, newPos) < 0.02f)
            {
                goIdle();
            }
        }

        if (waypoints == null)
            waypoints = new GameObject("Waypoints");

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
        CheckForMove();
    }

    public void startMove() 
    {
        isIdle = false;
        anim.SetBool("idleD", false);
        Debug.Log($"Moving to {newPos} from {transform.position}");
        GameObject waypoints = GameObject.Find("Waypoints");
        Destroy(waypoints);
        waypoints = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Gates")
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        if (collision.transform.tag == "Block")
            goIdle();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Gates")
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
    }

    bool IsGateAt(Vector3 position) 
    {
        Collider2D hit = Physics2D.OverlapPoint(position);
        if (hit !=null && hit.transform.tag == "Gates")
            return true;
        else
            return false;
    }

    bool IsTreasureAt(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position);
        if (hit!=null && hit.transform.tag == "Treasure")
            return true;
        else
            return false;
    }

    bool IsWaypointAt(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position);
        if (hit != null && hit.transform.tag == "Waypoint")
            return true;
        else
            return false;
    }

    public void CheckForMove()
    {
        //if (!isIdle) return;

        if(waypoints!=null)
            Destroy(waypoints);
        waypoints = new GameObject("Waypoints");

        Vector3 x;

        if (Physics2D.OverlapPoint(checkUp.transform.position)==null || IsGateAt(checkUp.transform.position) || IsTreasureAt(checkUp.transform.position) || IsWaypointAt(checkUp.transform.position))
        {
            int i = 0;
            x = Vector3.zero;
            do
            {
                i++;
                GameObject wp=Instantiate(waypoint,checkUp.transform.position + x, Quaternion.identity);
                wp.transform.parent=waypoints.transform;
                wp.name = $"Waypoint U{i}";
                x = x + Vector3.up;
            } while (Physics2D.OverlapPoint(checkUp.transform.position + x)==null || IsGateAt(checkUp.transform.position+x) || IsTreasureAt(checkUp.transform.position+x) || IsWaypointAt(checkUp.transform.position+x));
        }
        if (Physics2D.OverlapPoint(checkLeft.transform.position) == null || IsTreasureAt(checkLeft.transform.position) || IsWaypointAt(checkLeft.transform.position))
        {
            int i = 0;
            x = Vector3.zero;
            do
            {
                i++;
                GameObject wp = Instantiate(waypoint, checkLeft.transform.position + x, Quaternion.identity);
                wp.transform.parent = waypoints.transform;
                wp.name = $"Waypoint L{i}";
                x = x + Vector3.left;
            } while (Physics2D.OverlapPoint(checkLeft.transform.position + x) == null || IsTreasureAt(checkLeft.transform.position + x) || IsWaypointAt(checkLeft.transform.position + x));
        }
        if (Physics2D.OverlapPoint(checkRight.transform.position) == null || IsTreasureAt(checkRight.transform.position) || IsWaypointAt(checkRight.transform.position))
        {
            int i = 0;
            x = Vector3.zero;
            do
            {
                i++;
                GameObject wp = Instantiate(waypoint, checkRight.transform.position + x, Quaternion.identity);
                wp.transform.parent = waypoints.transform;
                wp.name = $"Waypoint R{i}";
                x = x + Vector3.right;
            } while (Physics2D.OverlapPoint(checkRight.transform.position + x) == null || IsTreasureAt(checkRight.transform.position + x) || IsWaypointAt(checkRight.transform.position + x));
        }
        if ((Physics2D.OverlapPoint(checkDown.transform.position) == null || IsTreasureAt(checkDown.transform.position) || IsWaypointAt(checkDown.transform.position))&& transform.position.y >= -1f)
        {
            int i = 0;
            x = Vector3.zero;
            do
            {
                i++;
                GameObject wp = Instantiate(waypoint, checkDown.transform.position + x, Quaternion.identity);
                wp.transform.parent = waypoints.transform;
                wp.name = $"Waypoint D{i}";
                x = x + Vector3.down;
            } while (Physics2D.OverlapPoint(checkDown.transform.position + x) == null || IsTreasureAt(checkDown.transform.position + x) || IsWaypointAt(checkDown.transform.position + x));
        }
    }
}