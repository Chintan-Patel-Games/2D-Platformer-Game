using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator[] lifeAnimators; // Array of Animators for each life
    [SerializeField] private GameObject[] keyIcons;   // Array of key icons
    [SerializeField] private TextMeshProUGUI levelNoText; // UI element to display the level number

    private void Start()
    {
        DisplayLevelNumber();
    }

    // Call this when the player loses a life
    public void TakeLife(int lifeIndex)
    {
        if (IsValidLifeIndex(lifeIndex))
        {
            lifeAnimators[lifeIndex].SetTrigger("healthLost");
        }
    }

    // Call this when the player gains a life
    public void GainLife(int lifeIndex)
    {
        lifeIndex--;
        if (IsValidLifeIndex(lifeIndex))
        {
            lifeAnimators[lifeIndex].SetTrigger("healthGain");
        }
    }

    // Update the key icons based on the current key count
    public void UpdateKeysUI(int currentKeys)
    {
        for (int i = 0; i < keyIcons.Length; i++)
        {
            keyIcons[i].SetActive(i < currentKeys);
        }
    }

    // Display the current level number
    private void DisplayLevelNumber()
    {
        levelNoText.text = $"Level: {SceneManager.GetActiveScene().name}";
    }

    // Validate if the given life index is within bounds
    private bool IsValidLifeIndex(int index)
    {
        return index >= 0 && index < lifeAnimators.Length;
    }
}