using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Image[] life;
    [SerializeField] GameObject[] keys;
    [SerializeField] TextMeshProUGUI levelNo;

    private void Start()
    {
        GetLevelNo();
    }

    public void UpdateLives()
    {
        if (playerController.Lives == 2)
        {
            life[2].enabled = false;
        }
        else if (playerController.Lives == 1)
        {
            life[1].enabled = false;
        }
        else if (playerController.Lives == 0)
        {
            life[0].enabled = false;
        }
        else
        {
            life[2].enabled = false;
            life[1].enabled = false;
            life[0].enabled = false;
        }
    }
    public void UpdateKeys()
    {
        if (playerController.Keys == 1)
        {
            keys[0].SetActive(true);
        }
        else if (playerController.Keys == 2)
        {
            keys[1].SetActive(true);
        }
        else if (playerController.Keys == 3)
        {
            keys[2].SetActive(true);
        }
        else
        {
            keys[0].SetActive(false);
            keys[1].SetActive(false);
            keys[2].SetActive(false);
        }
    }

    private void GetLevelNo()
    {
        string currentLevelno = SceneManager.GetActiveScene().name;
        levelNo.text = currentLevelno;
    }
}