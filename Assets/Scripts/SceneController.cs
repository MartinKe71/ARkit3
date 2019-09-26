using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update

    private int current;
    private int total;
    void Start()
    {
        current = SceneManager.GetActiveScene().buildIndex;
        total = SceneManager.sceneCountInBuildSettings;
    }

    // Update is called once per frame
    public void Prev()
    {
        if (current - 1 >= 0)
        {
            current--;
            SceneManager.LoadScene(current);
        }
    }

    public void Next()
    {
        if (current < total - 1)
        {
            current++;
            SceneManager.LoadScene(current);
        }
    }
}
