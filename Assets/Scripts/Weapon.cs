using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Это перечисление всех возможных типов оружия
/// Также включает тим "Shield",
/// чтобы дать возможность апать защиту
/// </summary>

public enum WeaponType
{
    none,       // по умолчанию, нет оружия
    blaster,    // простой бластер
    spread,     // веерная пушка, несколько снарядов
    phaser,     // волновой фазер (в перспективе)
    missile,    // самонаводящиеся ракеты (в перспективе)
    laser,      // луч, уничтожает все на пути, которкий импульс
    shield      // увеличивает shieldLevel
}

/// <summary>
/// Класс WeaponDefenition позволяет настраивать свойства
/// конкретного вида оружия в инспекторе. Для этого класс Main
/// будет хранить массив элементов типа WeaponDefenition
/// </summary>
[System.Serializable]
public class WeaponDefenition
{
    public WeaponType type = WeaponType.none;
    public string letter;   // буква на кубике бонуса
    public Color color = Color.white; // цвет оружия и бонуса
    public GameObject projectilePrefub; // шаблон снарядов
    public Color projectileColor = Color.white; // цвет снарядов
    public float damageOnHit = 0; // разрушительная мощьность
    public float continiousDamage = 0; // разрушение в секунду
    public float delayBetweenShots = 0; // интервал выстрелов
    public float velocity = 20; // скорость снаряда
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dinamically")]
    private WeaponType _type = WeaponType.blaster;
    public WeaponDefenition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        } else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefenition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShotTime < def.delayBetweenShots) return;

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0) { vel.y = -vel.y; }

        switch(type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rb.velocity = vel;
                break;

            case WeaponType spread:
                p = MakeProjectile();
                p.rb.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rb.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rb.velocity = p.transform.rotation * vel;
                break;

        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefub);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        } else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }

}
