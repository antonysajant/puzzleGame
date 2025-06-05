using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    bool isIdle = true;
    Rigidbody2D rb;
    [SerializeField]Vector3 newPos;
    [SerializeField] Vector3 current;
    [SerializeField] GameObject checkUp;
    [SerializeField] GameObject checkDown;
    [SerializeField] GameObject checkLeft;
    [SerializeField] GameObject checkRight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        newPos = transform.position;
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckForMove), 0f, 1f);
    }

    private void Update()
    {
        if (isIdle)
            anim.SetBool("idleD", true);

        if(!isIdle)
            transform.position = Vector3.MoveTowards(transform.position, newPos, 0.01f);

        if (Vector3.Distance(transform.position,newPos)<0.02f)
            goIdle();

        current = transform.position;
    }

    [ContextMenu("Idle")]
    void goIdle()
    {
        anim.SetBool("runL", false);
        anim.SetBool("runR", false);
        anim.SetBool("runU", false);
        anim.SetBool("runD", false);
        isIdle = true;
        anim.SetBool("idleD", true);
    }

    void startMove()
    {
        isIdle = false;
        anim.SetBool("idleD", false);
        //Debug.Log($"Moving to {newPos} from {transform.position}");
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
        if (Physics2D.OverlapPoint(checkUp.transform.position) == null || IsGateAt(checkUp.transform.position))
        {
            Debug.Log("Moving UP");
            anim.SetBool("runU", true);
            newPos = checkUp.transform.position;
            startMove();
        }
        else
        {
            if (Physics2D.OverlapPoint(checkLeft.transform.position) == null)
            {
                Debug.Log("Moving LEFT");
                anim.SetBool("runL", true);
                newPos = checkLeft.transform.position;
                startMove();
            }
            else
            {
                if (Physics2D.OverlapPoint(checkRight.transform.position) == null)
                {
                    Debug.Log("Moving RIGHT");
                    anim.SetBool("runR", true);
                    newPos = checkRight.transform.position;
                    startMove();
                }
                else
                {
                    if (Physics2D.OverlapPoint(checkDown.transform.position) == null)
                    {
                        Debug.Log("Moving DOWN");
                        anim.SetBool("runD", true);
                        newPos = checkDown.transform.position;
                        startMove();
                    }
                }
            }
        }
    }
}
