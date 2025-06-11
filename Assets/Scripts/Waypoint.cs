using System;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    Player p;
    [SerializeField] int n;
    [SerializeField] char dir;

    void Awake()
    {
        p=FindObjectOfType<Player>();
    }

    void Start()
    {
        n = (int)char.GetNumericValue(gameObject.name[10]);
        dir=gameObject.name[9];
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);

            RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                setPos();
            }
        }
    }

    void OnMouseDown()
    {
        setPos();
    }

    void setPos()
    {
        if (dir == 'U')
        {
            p.newPos = p.transform.position + n * Vector3.up;
            p.anim.SetBool("runU",true);

        }
        else if (dir == 'L')
        {
            p.newPos = p.transform.position + n * Vector3.left;
            p.anim.SetBool("runL", true);
        }

        else if (dir == 'R')
        {
            p.newPos = p.transform.position + n * Vector3.right;
            p.anim.SetBool("runR", true);
        }

        else if (dir == 'D')
        {
            p.newPos = p.transform.position + n * Vector3.down;
            p.anim.SetBool("runD", true);
        }
           
        p.startMove();
    }
}
