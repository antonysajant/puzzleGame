using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager: MonoBehaviour
{
    Player p;
    [SerializeField] float yEnd;
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
}
