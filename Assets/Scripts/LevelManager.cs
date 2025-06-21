using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager: MonoBehaviour
{
    Player p;
    [SerializeField] float yEnd;
    [SerializeField] Text coins;
    int count = 0;
    int sceneID;
    void Awake()
    {
        p = FindObjectOfType<Player>();
        sceneID= SceneManager.GetActiveScene().buildIndex;
    }
    void Update()
    {
        if (p.transform.position.y > yEnd)
        {
            if (sceneID == 4)
                sceneID = 0;
            else
                sceneID++;
            SceneManager.LoadScene(sceneID);
        }    
    }

    public void restart()
    {
        SceneManager.LoadScene(sceneID);
    }


    public void coinplus()
    {
        count++;
        if (count > 3)
            return;
        coins.text = $"{count} / 3";
        if (count == 3)
        {
            Color blue;
            if (ColorUtility.TryParseHtmlString("#11BEC5", out blue))
            {
                coins.color = blue;
            }
        }
    }
}
