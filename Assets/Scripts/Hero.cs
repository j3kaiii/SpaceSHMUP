using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    [Header("Set Dinamically")]
    [SerializeField]
    private float _shieldLevel = 1f;

    private GameObject lastTriggerGo = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        } else
        {
            Debug.Log("Second Hero.S found");
        }

        //fireDelegate += TempFire;
    }
    
    void Update()
    {
        // слушаем клавиши
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        // двигаем Сокола
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // наклоны
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }*/

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    private void TempFire()
    {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position;
        Rigidbody rb = projGo.GetComponent<Rigidbody>();
        //rb.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGo.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefenition(proj.type).velocity;
        rb.velocity = Vector3.up * tSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Got collision with " + other.name);
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Get " + go.name);

        if (go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        } else
        {
            print("Not an anemy " + go.name);
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0) Destroy(this.gameObject);
            Main.S.DelayedRestart(gameRestartDelay);
        }
    }
}
