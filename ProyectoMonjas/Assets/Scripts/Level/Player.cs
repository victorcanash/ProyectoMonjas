using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    //HUD
    [SerializeField] private HUD_Player HUD_player = null;

    //life
    [SerializeField] private float maxHealth = 0;
    [SerializeField] private float currentHealth = 0;

    //point and shoot
    private enum STATE_WEAPON { NULL, POINTING, SHOOTING }
    [Header("ATTACK")]
    [SerializeField] private STATE_WEAPON currentStateWeapon = STATE_WEAPON.NULL;
    [SerializeField] private GameObject bullet = null;
    private float scope = 5f; //alcance de la bala
    private float damage = 1f; //daño de la bala
    private float bullet_force = 750f; //velocidad de la bala
    private const float timeShooting = 0.5f; //tiempo disparando
    private Vector2 direction_attackJoystick = Vector2.zero;
    private Coroutine shoot_coroutine = null;
    private GameObject scopeColliderGO = null;
    private BoxCollider scopeCollider = null;

    //movement and rotation
    private Quaternion rotationToLook = new Quaternion(0, 0, 0, 0);
    private const float angular_speed = 5f; //velocidad de giro
    [Header("MOVEMENT")]
    [SerializeField] private NavMeshAgent agent = null;

    //general
    public bool active { get; set; }

    public void Init()
    {
        active = false;
        HUD_player = Instantiate(HUD_player.gameObject, transform.position, transform.rotation, null).GetComponent<HUD_Player>();
        HUD_player.Init(this, scope);
        currentHealth = maxHealth;
        GameManager.Instance.LevelController.HUD_level.InitPlayerHealthSlider(maxHealth);
    }

    private void Update()
    {
        if (active)
        {
            Point();
            LookMoveDirection();
        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Move();
        }
    }

    private void Move()
    {
        agent.SetDestination(new Vector3(transform.position.x + (GameManager.Instance.LevelController.HUD_level.moveJoystick.Horizontal), transform.position.y, transform.position.z + (GameManager.Instance.LevelController.HUD_level.moveJoystick.Vertical)));
        HUD_player.UpdateHUD_move();
    }

    private void Point()
    {
        if (GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction != Vector2.zero) //cuando estas apuntando
        {
            currentStateWeapon = STATE_WEAPON.POINTING;
            direction_attackJoystick = GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction;
            HUD_player.UpdateHUD_point(false);
        }
        else if (GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction == Vector2.zero && currentStateWeapon == STATE_WEAPON.POINTING) //cuando dejas de apuntar
        {
            Shoot(false);
        }
    }

    public void Shoot(bool pointed_auto)
    {
        if (shoot_coroutine != null) StopCoroutine(shoot_coroutine);
        currentStateWeapon = STATE_WEAPON.SHOOTING;
        if (pointed_auto)
        {
            HUD_player.UpdateHUD_point(true);
        }
        shoot_coroutine = StartCoroutine(ShootCoroutine(pointed_auto));
    }

    private IEnumerator ShootCoroutine(bool pointed_auto)
    {
        HUD_player.SetAttackColorHUD_point();
        if (!pointed_auto)
        {
            if (new Vector3(direction_attackJoystick.x, 0, direction_attackJoystick.y) != Vector3.zero)
                rotationToLook = Quaternion.LookRotation(new Vector3(direction_attackJoystick.x, 0, direction_attackJoystick.y));
            transform.rotation = rotationToLook;
        }
        Bullet blt = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation, null).GetComponent<Bullet>();
        blt.Init(damage, bullet_force, CreateScopeCollider());
        yield return new WaitForSeconds(timeShooting);
        HUD_player.DisableHUD_point();
        currentStateWeapon = STATE_WEAPON.NULL;
        StopCoroutine(shoot_coroutine);
    }

    private GameObject CreateScopeCollider()
    {
        scopeColliderGO = new GameObject("ScopeCollider");
        scopeColliderGO.tag = "Wall";
        scopeCollider = scopeColliderGO.AddComponent<BoxCollider>();
        scopeCollider.isTrigger = true;
        scopeCollider.center = new Vector3(0, 1, 0);
        scopeCollider.size = new Vector3(1, 2, 1);
        scopeCollider.transform.position = transform.position + transform.forward * scope;
        scopeCollider.transform.rotation = transform.rotation;
        return scopeColliderGO;
    }

    private void LookMoveDirection()
    {
        if (GameManager.Instance.LevelController.HUD_level.moveJoystick.Direction != Vector2.zero && currentStateWeapon != STATE_WEAPON.SHOOTING)
        {
            if (new Vector3(agent.velocity.x, 0, agent.velocity.z) != Vector3.zero)
                rotationToLook = Quaternion.LookRotation(new Vector3(agent.velocity.x, 0, agent.velocity.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationToLook, angular_speed * Time.deltaTime);
        }
    }

    public void GetDamage(float damageReceived)
    {
        currentHealth -= damageReceived;
        GameManager.Instance.LevelController.HUD_level.UpdateHealthSlider(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.LevelController.GameOver();
        }
    }
}
