using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;

    [Header("Set Dinamically")]
    public int levelShown = 0;

    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // текущая мощность щита
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        // если отличается от показанной
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            // задаем текстуре нужный оффсет
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        
        // вращаем щит
        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
