using System.Collections;
using UnityEngine;

public interface IEnemy
{
    int Health { get; set; }
    float MoveSpeed { get; set; }
    GameObject PointA { get; }
    GameObject PointB { get; }
    Sounds[] FootstepClips { get; set; }
    Sounds[] AttackClips { get; set; }
    bool CanAttack { get; }

    void EnemyPatrol();
    void MoveEnemy();
    void EndPointController();
    void FlipEnemy();
    IEnumerator Attack(PlayerController player);
    IEnumerator TakePlayerLives(PlayerController player);
    void TakeDamage(int damage);
    IEnumerator HandleDeath();
    void PlayFootstepSounds();
    void PlayAttackSounds();
}