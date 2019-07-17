using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    //movement and rotation
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private NavMeshObstacle navObstacle = null;
    private Quaternion rotationToLook = new Quaternion(0, 0, 0, 0);
    private const float angular_speed = 5f; //velocidad de giro
    private Vector3 playerDistance = Vector3.zero;
    private float distanceToStop = 0;

    //life
    [SerializeField] private float maxHealth = 0;
    [SerializeField] private float currentHealth = 0;

    //attack
    private Coroutine attack_coroutine = null;
    private float damage = 1f;

    //HUD
    public Transform enemy_HUD_parent { get; set; }
    [SerializeField] private HUD_Enemy HUD_enemy = null;

    public void Init()
    {
        transform.eulerAngles = new Vector3(0, -90, 0);
        distanceToStop = agent.stoppingDistance + 0.4f;
        currentHealth = maxHealth;

        InitHUD();
    }

    private void Update()
    {
        playerDistance = GameManager.Instance.LevelController.player.transform.position - transform.position;

        if (Mathf.Abs(playerDistance.magnitude) <= distanceToStop && agent.enabled) //stopped
        {
            agent.enabled = false;
            navObstacle.enabled = true;
            Attack();
        }
        else if (Mathf.Abs(playerDistance.magnitude) > distanceToStop && !agent.enabled) //moving
        {
            navObstacle.enabled = false;
            agent.enabled = true;
        }

        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (agent.enabled)
            agent.SetDestination(GameManager.Instance.LevelController.player.transform.position);
    }

    private void Look()
    {
        if (agent.enabled)
        {
            if (new Vector3(agent.velocity.x, 0, agent.velocity.z) != Vector3.zero)
                rotationToLook = Quaternion.LookRotation(new Vector3(agent.velocity.x, 0, agent.velocity.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationToLook, angular_speed * Time.deltaTime);
        }
        else
        {
            if (new Vector3(playerDistance.x, 0, playerDistance.z) != Vector3.zero)
                rotationToLook = Quaternion.LookRotation(new Vector3(playerDistance.x, 0, playerDistance.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationToLook, angular_speed * Time.deltaTime);
        }
    }

    private void InitHUD()
    {
        HUD_enemy = Instantiate(HUD_enemy.gameObject, transform.position, new Quaternion(0,0,0,0), enemy_HUD_parent).GetComponent<HUD_Enemy>();
        HUD_enemy.Init(this, maxHealth, currentHealth);
    }

    public void GetDamage(float damageReceived)
    {
        currentHealth -= damageReceived;
        HUD_enemy.UpdateHealthSlider(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(HUD_enemy.gameObject);
            Destroy(gameObject);
            GameManager.Instance.LevelController.DeleteEnemy(this);
        }
    }

    private void Attack()
    {
        if (attack_coroutine != null)
            StopCoroutine(attack_coroutine);
        attack_coroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine(bool waitToAttack = true)
    {
        if (waitToAttack)
        {
            yield return new WaitForSeconds(0.2f);
        }
        if (Mathf.Abs(playerDistance.magnitude) <= distanceToStop)
        {
            GameManager.Instance.LevelController.player.GetDamage(damage);
            yield return new WaitForSeconds(1f);
            StopCoroutine(attack_coroutine);
            attack_coroutine = StartCoroutine(AttackCoroutine(false));
        }
        else
        {
            StopCoroutine(attack_coroutine);
        }
    }
}
