using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adsorption : MonoBehaviour
{
    public Vector2 Size = new Vector2(1, 1);

    public float Distance = 0.5f;

    public LayerMask m_TankMask;
    public float m_ExplodionRadius = 0.3f;

    //获取两个模型的接口坐标信息
    Transform m_door;
    Transform t_door;

    private void Awake()
    {
        m_door = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        Collider[] colliders = Physics.OverlapSphere(m_door.position, m_ExplodionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            t_door = colliders[i].transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        t_door = null;
    }

    private void Update()
    {
        if (IsClose())
        {
            RotateThis();
            MoveTo();
        }
    }

    private void MoveTo()
    {
        Vector3 moveVector = t_door.position - m_door.position;
        m_door.parent.Translate(moveVector, Space.World);
    }

    private void RotateThis()
    {
        Vector3 rotate = Vector3.Cross(m_door.forward, t_door.forward);
        float angle1 = Vector3.Angle(m_door.forward, t_door.forward) + 180;
        m_door.parent.Rotate(rotate, angle1, Space.World);

        float angle2 = Vector3.Angle(t_door.up, m_door.up);
        m_door.parent.Rotate(t_door.forward, angle2, Space.World);
    }

    private bool IsClose()
    {
        if (m_door != null && t_door != null)
        {
            float tempDistance = Vector3.Distance(m_door.position, t_door.position);
            if (tempDistance <= Distance && tempDistance > 0)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Vector2 halfSize = Size * 0.5f;
        Gizmos.color = Color.red;
        float lineLength = Mathf.Min(Size.x, Size.y);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * lineLength);
        Gizmos.color = Color.blue;

        Vector3 topLeft = transform.position - (transform.right * halfSize.x) + (transform.up * Size.y) / 2;
        Vector3 topRight = transform.position + (transform.right * halfSize.x) + (transform.up * Size.y) / 2;

        Vector3 bottomLeft = transform.position - (transform.right * halfSize.x) - (transform.up * Size.y) / 2;
        Vector3 bottomRight = transform.position + (transform.right * halfSize.x) - (transform.up * Size.y) / 2;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
