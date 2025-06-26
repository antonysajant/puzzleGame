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
    Vector3 dragOffset;

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

        dragOffset = lastDragged.transform.position - worldPos;

        if (lastDragged.getBlock())
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX; 
        else
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }

    void Drag()
    {
        Vector3 targetPos = worldPos + dragOffset;
        dragTargetPos = new Vector2(
            lastDragged.getBlock() ? targetPos.x : lastDragged.transform.position.x,
            lastDragged.getBlock() ? lastDragged.transform.position.y : targetPos.y
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
        Vector2 pos = lastDragged.transform.position;
        Vector2 tempPos = pos;
        pos.x = lastDragged.getBlock() ? Mathf.Round(pos.x) : pos.x;
        pos.y = lastDragged.getBlock() ? pos.y : Mathf.Round(pos.y);
        if (lastDragged.getBigBlock())
        {
            float cellSize = 1f;
            float halfExtent = 1.5f; // 3 cells * 0.5
            if(lastDragged.getBlock())
                pos.x = Mathf.Round((tempPos.x - halfExtent) / cellSize) * cellSize + halfExtent;
            else
                pos.y = Mathf.Round((tempPos.y - halfExtent) / cellSize) * cellSize + halfExtent;

            lastDragged.transform.position = pos;
        }
        lastDragged.transform.position = pos;
        lastDragged = null;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        rb = null;
        p.CheckForMove();
    }
}
