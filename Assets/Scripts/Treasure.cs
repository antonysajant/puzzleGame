using System.Collections;
using System.Threading;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] GameObject closed;
    [SerializeField] GameObject open;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject coin;
    bool opened = false;
    LevelManager lm;

    void Awake()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !opened)
        { 
            closed.SetActive(false);
            open.SetActive(true);
            StartCoroutine(CoinFlip());
            opened = true;
            lm.coinplus();
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

    IEnumerator CoinFlip()
    {
        coin.SetActive(true);
        yield return new WaitForSeconds(2f);
        coin.SetActive(false);
    }
}