using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������������ ���� ��������� ����� ������
/// ����� �������� ��� "Shield",
/// ����� ���� ����������� ����� ������
/// </summary>

public enum WeaponType
{
    none,       // �� ���������, ��� ������
    blaster,    // ������� �������
    spread,     // ������� �����, ��������� ��������
    phaser,     // �������� ����� (� �����������)
    missile,    // ��������������� ������ (� �����������)
    laser,      // ���, ���������� ��� �� ����, �������� �������
    shield      // ����������� shieldLevel
}

/// <summary>
/// ����� WeaponDefenition ��������� ����������� ��������
/// ����������� ���� ������ � ����������. ��� ����� ����� Main
/// ����� ������� ������ ��������� ���� WeaponDefenition
/// </summary>
[System.Serializable]
public class WeaponDefenition
{
    public WeaponType type = WeaponType.none;
    public string letter;   // ����� �� ������ ������
    public Color color = Color.white; // ���� ������ � ������
    public GameObject projectilePrefub; // ������ ��������
    public Color projectileColor = Color.white; // ���� ��������
    public float damageOnHit = 0; // �������������� ���������
    public float continiousDamage = 0; // ���������� � �������
    public float delayBetweenShots = 0; // �������� ���������
    public float velocity = 20; // �������� �������
}

public class Weapon : MonoBehaviour
{

}
