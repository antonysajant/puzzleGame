using UnityEngine;

public class EnablePlayer : MonoBehaviour
{
    [SerializeField] Vector2 StartPosition;
    Player player;
    Animator anim;

    void Awake()
    { 
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        anim.SetBool("idleD", false);
        anim.SetBool("runU", true);
    }

    void Update()
    {
        transform.position=Vector2.MoveTowards(transform.position, StartPosition, 3.5f * Time.deltaTime);
        if(Vector2.Distance(transform.position,StartPosition)<0.02f)
        {
            player.enabled = true;
            anim.SetBool("idleD", true);
            anim.SetBool("runU", false);
            this.enabled = false;
        }
    }
}
