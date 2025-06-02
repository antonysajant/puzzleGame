using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Android.Gradle;
using UnityEngine;

public class DragController : MonoBehaviour
{
    bool isDragActive = false;
    Vector2 screenPos;
    Vector3 worldPos;
    public Vector2 dragTargetPos;
    Block lastDragged;
    public bool canDrag = true;

    void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
            Destroy(gameObject);
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
            Vector3 mousePos = Input.mousePosition;
            screenPos = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            screenPos = Input.GetTouch(0).position;
        }
        else
            return;

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
                Block draggable = hit.transform.gameObject.GetComponent<Block>();
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
        if (!canDrag) return;

        if (isDragActive && lastDragged != null)
        {
            Rigidbody2D rb = lastDragged.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 current = rb.position;
                Vector2 newPos = Vector2.Lerp(current, dragTargetPos, 0.2f);
                if (Vector2.Distance(newPos, current) > 0.001f)
                    rb.MovePosition(newPos);
            }
        }
    }


    void InitDrag()
    {
        isDragActive = true;
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
        lastDragged = null;
        canDrag = true;
    }
}
