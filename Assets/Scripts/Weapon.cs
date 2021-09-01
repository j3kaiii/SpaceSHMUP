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

}
