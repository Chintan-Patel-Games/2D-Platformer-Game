using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Spitter : EnemyController
{
    [SerializeField] private Animator spitterAnimator; 
    private Sounds[] footstepClips;
    private Sounds[] attackClips;

    private void Awake()
    {
        // Set the animator specific to Chomper (can also do this in the Inspector)
        if (spitterAnimator != null)
        {
            enemyAnimator = spitterAnimator;
        }
        else
        {
            enemyAnimator = GetComponent<Animator>(); // Fallback to default
        }
    }

    public override IEnumerator Attack(PlayerController player)
    {
        canAttack = false;
        enemyAnimator.SetTrigger("attack");
        StartCoroutine(TakePlayerLives(player));

        // Wait for cooldown period
        yield return new WaitForSeconds(3f);
        canAttack = true;
    }

    public override IEnumerator HandleDeath()
    {
        SoundManager.Instance.Play(Sounds.spitDie);
        enemyAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // This method is called via animation events
    public override void PlayFootstepSounds()
    {
        footstepClips = Enum.GetValues(typeof(Sounds))
                        .Cast<Sounds>()
                        .Where(sound => sound.ToString().StartsWith("spitFoots"))
                        .ToArray();

        if (footstepClips.Length > 0)
        {
            // Randomize footstep sounds
            Sounds clip = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length)];
            SoundManager.Instance.Play(clip);
        }
    }

    // This method is called via animation events
    public override void PlayAttackSounds()
    {
        attackClips = Enum.GetValues(typeof(Sounds))
                    .Cast<Sounds>()
                    .Where(sound => sound.ToString().StartsWith("spitAttack"))
                    .ToArray();

        if (attackClips.Length > 0)
        {
            // Randomize attack sounds
            Sounds clip = attackClips[UnityEngine.Random.Range(0, attackClips.Length)];
            SoundManager.Instance.Play(clip);
        }
    }
}