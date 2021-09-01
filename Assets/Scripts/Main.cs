using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    static public Main S;
    static Dictionary<WeaponType, WeaponDefenition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaulPadding = 1.5f;
    public WeaponDefenition[] weaponDefenitions;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefenition>();
        foreach(WeaponDefenition def in weaponDefenitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }
   
    public void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[randomEnemy]);

        float enemyPadding = enemyDefaulPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;

        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene_1");
    }

    /// <summary>
    /// ����������� �������, ������������ WeaponDefenition �� ������������
    /// ����������� ���� WEAP_DICT ����� Main.
    /// </summary>
    /// <returns> ��������� WeaponDefenition ���, ���� ��� ������ �����������
    /// ��� ���������� WeaponType, ���������� ����� ���������
    /// WeaponDefenition � ����� none.</returns>
    /// <param name="wt"> ��� ������ WeaponType, ��� �������� ��������� ��������
    /// WeaponDefenition</param>
    /// 
    static public WeaponDefenition GetWeaponDefenition(WeaponType wt)
    {
        // ��������� ������� ����� � �������
        // ���� ����� ���, ����� ������
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        // ���� �� ����� ����, ���������� ����� ���������
        // �������� � ����� ������ none
        return (new WeaponDefenition());
    }
    
}
