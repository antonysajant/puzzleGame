using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] GameObject closed;
    [SerializeField] GameObject open;
    [SerializeField] GameObject ui;
    bool opened = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        { 
            closed.SetActive(false);
            open.SetActive(true);
            opened = true;
        }
    }


    void Update()
    { 
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null && hit.transform.tag == "Block" && !opened)
            ui.SetActive(true);
        else
            ui.SetActive(false);
    }

}