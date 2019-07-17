using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rbody = null;
    private float damage = 0, force = 0;
    private GameObject myScopeCollider = null;

    public void Init(float _damage, float _force, GameObject scopeCollider)
    {
        damage = _damage;
        force = _force;
        myScopeCollider = scopeCollider;
        Move();
    }

    private void Move()
    {
        rbody.AddForce(transform.forward * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Wall")
        {
            Destroy(gameObject);
            Destroy(myScopeCollider);
            if (other.tag == "Enemy")
            {
                GameManager.Instance.LevelController.current_enemies[CharToInt2(other.name[other.name.Length - 1])].GetDamage(damage);
            }
        }
    }

    private int CharToInt2(char input)
    {
        int result = -1;

        int tempInt = 0;
        if (int.TryParse(input.ToString(), out tempInt) == true)
        {
            result = tempInt;
        }

        return result;
    }
}
