using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenManager : MonoBehaviour
{
    // This method will be called when the Play button is clicked
    public void OnPlayButtonClicked()
    {
        // Load the game scene, replace "GameScene" with the name of your game scene
        SceneManager.LoadScene("Main");
    }
}
