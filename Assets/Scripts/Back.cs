using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    public Button Button_b;
    public AudioSource Click;
    public AudioClip sound;
    public float delayBeforeLoading = 0.4f;

    private void Start()
    {
        if (Button_b != null)
        {
            Button_b.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        if (Click!= null)
        {
            Click.clip = sound;
            Debug.Log("has sound");
            Click.Play();
        }
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoading);
        SceneManager.LoadScene("HomeScreen");
    }
}
