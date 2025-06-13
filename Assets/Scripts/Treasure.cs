using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]GameObject closed;
    [SerializeField]GameObject open;

    void Awake()
    {
        //closed = transform.GetChild(0).gameObject;
        //open=transform.GetChild(1).gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        { 
            closed.SetActive(false);
            open.SetActive(true);
        }
    }
}
