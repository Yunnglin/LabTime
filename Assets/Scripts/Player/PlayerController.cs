using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private GameObject camera;

    private void Start()
    {
        if (!camera)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
        RotateWithCamera();
    }
    /// <summary>
    /// 控制玩家移动
    /// </summary>
    private void PlayerMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }

    private void RotateWithCamera()
    {
        float euler_y = camera.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0f, euler_y, 0f);
    }
}
