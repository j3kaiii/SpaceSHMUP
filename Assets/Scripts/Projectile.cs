using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dinamically")]
    public Rigidbody rb;
    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.offUp) Destroy(gameObject);
    }

    ///<summary>
    ///�������� ������� ���� _type � ������������� ���� ����� �������,
    ///��� ���������� � WeaponDefenition
    /// </summary>
    /// <param name="eType">
    /// ��� WeaponType ������������� ������.
    /// </param>
    /// 
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefenition def = Main.GetWeaponDefenition(_type);
        rend.material.color = def.projectileColor;
    }

}

