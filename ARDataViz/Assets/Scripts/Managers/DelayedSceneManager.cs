using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomSceneManagement
{
    public class DelayedSceneManager : MonoBehaviour
    {

        void Start()
        {
            StartCoroutine(LoadScene("LobbyScene", 2.5f));
        }

        IEnumerator LoadScene(string name, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SceneManager.LoadScene(name);
        }
    }
}

