using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Animator SceneTransition;
    public void Play()
    {
        Debug.Log("in1");
        //SceneManager.LoadScene("Level 1");
        StartCoroutine(TransitionAndLoad("Level 1"));       
    }

    private IEnumerator TransitionAndLoad(string sceneName)
    {
        Debug.Log("in2");
        SceneTransition.Play("SceneTransitionReverse");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
