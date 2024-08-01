using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButtonScript : MonoBehaviour
{
    public Button playButton; // Assign the Play button in the Inspector
    public AudioSource audioSource; // Assign the AudioSource in the Inspector
    public float delayBeforeLoading = 2f; // Set the delay duration (in seconds)

    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoading);
        SceneManager.LoadScene("Main");
    }

}
