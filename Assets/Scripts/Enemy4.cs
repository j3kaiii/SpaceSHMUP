using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part - сериализуемый класс для храниения данных 
/// о частях коробля
/// </summary>
[System.Serializable]
public class Part
{
    //Значения этих полей должны определяться в инспекторе
    public string name;
    public float health;
    public string[] protectedBy;

    // Эти поля инициализируются автоматически в Start()
    [HideInInspector]
    public GameObject go; // объект части

    [HideInInspector]
    public Material mat; // материал повреждения
}


/// <summary>
/// Enemy4 создается за верхней границей, выбирает случайную точку,
/// перемещается к ней. Дальше снова выбирает случайную точку
/// и продолжает движение.
/// Это повторяется до уничтожения корабля.
/// </summary>

public class Enemy4 : Enemy
{
    [Header("Set in Inspector")]
    public Part[] parts; // массив частей корабля

    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4f;
    // Start is called before the first frame update
    void Start()
    {
        p0 = p1 = pos;
        InitMovement();

        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    void InitMovement()
    {
        p0 = p1;
        float wMinRad = bndCheck.camWidth - bndCheck.radius;
        float hMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-wMinRad, wMinRad);
        p1.y = Random.Range(-hMinRad, hMinRad);

        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if (prt.name == n) return (prt);
        }
        return null;
    }

    Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go) return (prt);
        }
        return null;
    }

    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if (prt == null) return true;

        return (prt.health <= 0);
    }

    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                // Поразить врага
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                // Проверить, защищена ли еще эта часть корабля
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        // Если хотя бы одна часть
                        // из защиты не разрушена
                        if (!Destroyed(s))
                        {
                            // Не разрушаем эту часть
                            Destroy(other); // Унитожаем снаряд
                            return;
                        }
                    }
                }
                // Эта часть не защищена, наносим урон
                prtHit.health -= Main.GetWeaponDefenition(p.type).damageOnHit;
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0) prtHit.go.SetActive(false); // деактивируем

                // проверим, бал ли корабль разрушен полностью
                bool allDestroyed = true;
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }
}
