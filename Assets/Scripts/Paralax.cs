using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject poi; //������� ������
    public GameObject[] panels; // �������������� ������ ��������� �����
    public float scrollSpeed = -1f;
    // motionMult ���������� ������� ������� ������� �� ����������� ������
    public float motionMult = 2f;

    private float panelHt; // ������ ������
    private float depth; // ������� (pos.z) �������
    // Start is called before the first frame update
    void Start()
    {
        panelHt = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);
        if (poi != null) tX = -poi.transform.position.x * motionMult;

        // �������� ������ [0]
        panels[0].transform.position = new Vector3(tX, tY, depth);
        // �������� ������ [1]
        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - panelHt, depth);
        } else
        {
            panels[1].transform.position = new Vector3(tX, tY + panelHt, depth);
        }
    }
}
