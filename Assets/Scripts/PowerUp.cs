using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    // � Vector2 ����� ������� ��� � ��� �������� ��� Random
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dinamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere;
        // onUnitSphere ���������� ������ �� ��������� �����
        // �� ����������� ����� �������� 1�. � ������� � 0,0,0
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        // ���������� �������
        transform.rotation = Quaternion.identity; // 0,0,0

        // ������������� ��������� ��������
        rotPerSecond = new Vector3(
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y)
            );

        birthTime = Time.time;
    }

    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen) Destroy(gameObject);
    }

    public void SetType(WeaponType wt)
    {
        WeaponDefenition def = Main.GetWeaponDefenition(wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedDy(GameObject target)
    {
        Destroy(this.gameObject);
        print("Picked by Hero");
    }
}
