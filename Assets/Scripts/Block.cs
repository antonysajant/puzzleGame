using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] bool isHorizontal = true;
    Rigidbody2D rb;

    [System.Obsolete]
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool getBlock()
    {
        return isHorizontal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
