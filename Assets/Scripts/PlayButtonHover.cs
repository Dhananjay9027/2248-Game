using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonHover : MonoBehaviour
{
    private Animator animator;
    private Button button;

    void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        // Add the event listeners for pointer enter and exit
        button.onClick.AddListener(OnClick);
        animator.SetBool("IsHover", true);
    }

    
    void OnClick()
    {
        animator.SetBool("IsHover", false);
    }
}
