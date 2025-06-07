using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    bool isDragActive = false;
    Vector2 screenPos;
    Vector3 worldPos;
    public Vector2 dragTargetPos;
    Block lastDragged;
    Rigidbody2D rb;
    public bool canDrag = true;
    public bool move=false;

    [System.Obsolete]
    void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
            Destroy(gameObject);
    }
    void Start()
    {
        Debug.Log("move TRUE");
    }

    void Update()
    {
        if (isDragActive && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Drop();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            screenPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            screenPos = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                Block draggable = hit.transform.GetComponent<Block>();
                if (draggable != null)
                {
                    lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (rb == null || lastDragged == null)
            return;

        if (!canDrag)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        if (isDragActive)
        {
            Vector2 current = rb.position;
            Vector2 newPos = Vector2.Lerp(current, dragTargetPos, 0.2f);
            if (Vector2.Distance(newPos, current) > 0.001f)
                rb.MovePosition(newPos);
        }
    }

    void InitDrag()
    {
        isDragActive = true;
        rb = lastDragged.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }

    void Drag()
    {
        dragTargetPos = new Vector2(
            lastDragged.getBlock() ? worldPos.x : lastDragged.transform.position.x,
            lastDragged.getBlock() ? lastDragged.transform.position.y : worldPos.y
        );
    }

    void Drop()
    {
        isDragActive = false;
        canDrag = true;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        lastDragged = null;
        rb = null;
        move = true;
        Debug.Log("move TRUE");
    }
}
