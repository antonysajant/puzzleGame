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
    Block lastDragged;

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

    void InitDrag()
    {
        isDragActive = true;
    }

    void Drag()
    {
        Block draggable = lastDragged.GetComponent<Block>();
        lastDragged.transform.position = new Vector2(
            draggable.getBlock() ? worldPos.x : lastDragged.transform.position.x,
            draggable.getBlock() ? lastDragged.transform.position.y : worldPos.y
            );
    }

    void Drop()
    {
        isDragActive = false;
    }
}
