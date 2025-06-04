using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    char dir = 'X'; // U = Up, D = Down, L = Left, R = Right
    bool isIdle = true;
    bool isMoving = false;
    Rigidbody2D rb;
    RaycastHit2D hit;
    Vector3 newPos;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        newPos=transform.position;
    }

    private void Update()
    {
        if (isIdle)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up);
            if (hit.collider == null || hit.transform.tag == "Gates")
            {
                dir = 'U';
                newPos = transform.position + Vector3.up;
                startMove();
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, Vector2.down);
                if (hit.collider == null || hit.transform.tag == "Gates")
                {
                    dir = 'D';
                    newPos = transform.position + Vector3.down;
                    startMove();
                }
                else
                {
                    hit = Physics2D.Raycast(transform.position, Vector2.left);
                    if (hit.collider == null)
                    {
                        dir = 'L';
                        newPos = transform.position + Vector3.left;
                        startMove();
                    }
                    else
                    {
                        hit = Physics2D.Raycast(transform.position, Vector2.right);
                        if (hit.collider == null)
                        {
                            dir = 'R';
                            newPos = transform.position + Vector3.right;
                            startMove();
                        }
                    }
                }
            }
        }
        if (!isIdle)
        { 
            if (dir == 'L')
                moveLeft();
            else if (dir == 'R')
                moveRight();
            else if (dir == 'U')
                moveUp();
            else if (dir == 'D')
                moveDown();
        }

        if(isMoving)
            transform.position = Vector3.MoveTowards(transform.position, newPos, 0.1f);

        if (Vector3.Distance(transform.position,newPos)<0.01f)
            goIdle();
    }

    [ContextMenu("MoveLeft")]
    void moveLeft()
    {
        anim.SetBool("runL", true);
    }

    [ContextMenu("MoveRight")]
    void moveRight()
    {
        anim.SetBool("runR", true);
    }

    [ContextMenu("MoveUp")]
    void moveUp()
    {
        anim.SetBool("runU", true);
    }

    [ContextMenu("MoveDown")]
    void moveDown()
    {
        anim.SetBool("runD", true);
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
        dir = 'X';
    }

    void startMove()
    {
        isIdle = false;
        anim.SetBool("idleD", false);
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Gates")
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
    }
}
