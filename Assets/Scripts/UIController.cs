using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Image[] life;

    public void UpdateLives()
    {
        if (playerController.lives == 2)
        {
            life[2].enabled = false;
        }
        else if (playerController.lives == 1)
        {
            life[1].enabled = false;
        }
        else if (playerController.lives == 0)
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
}