using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEndBehaviour : StateMachineBehaviour
{
    // Called when the state exits (after the animation finishes)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the animation has finished
        if (stateInfo.normalizedTime >= 1.0f)
        {
            // Trigger the scene load
            SceneManager.LoadScene(LevelList.Level1.ToString());
        }
    }
}