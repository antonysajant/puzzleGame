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
    Player p;

    [System.Obsolete]
    void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
            Destroy(gameObject);
        p= FindObjectOfType<Player>();
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

        if (isDragActive)
        {
            rb.MovePosition(dragTargetPos);
        }

    }
    void InitDrag()
    {
        isDragActive = true;
        rb = lastDragged.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (lastDragged.getBlock())
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX; // Allow horizontal movement
        else
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY; // Allow vertical movement
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
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        lastDragged = null;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        rb = null;
        p.CheckForMove();
    }
}
