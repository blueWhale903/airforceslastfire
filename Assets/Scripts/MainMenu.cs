using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator SceneTransition;

    public void Play()
    {
        StartCoroutine(TransitionAndLoad("Level 1"));       
    }

    private IEnumerator TransitionAndLoad(string sceneName)
    {
        SceneTransition.Play("SceneTransitionReverse");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
