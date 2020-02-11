using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes m_axes = RotationAxes.MouseXAndY;

    [SerializeField] GameObject player;
    [SerializeField] Vector3 offset; //距离玩家的位移
    [SerializeField] float maxDetectDistance = 100f;

    //镜头旋转
    [SerializeField] private float m_sensitivityX = 3f;
    [SerializeField] private float m_sensitivityY = 3f;
    // 水平方向的 镜头转向
    [SerializeField] private float m_minimumX = -360f;
    [SerializeField] private float m_maximumX = 360f;
    // 垂直方向的 镜头转向 (这里给个限度 最大仰角为45°)
    [SerializeField] private float m_minimumY = -45f;
    [SerializeField] private float m_maximumY = 45f;

    //镜头距离
    [SerializeField] private float zoomSpeed = 100f;
    [SerializeField] private float m_minDistance = 0f;
    [SerializeField] private float m_maxDistance = 100f;

    float m_rotationY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // 防止 刚体影响 镜头旋转
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraRotate();
        //CameraFOV();
        FollowPlayer();
        DetectEquipent();
    }

    private void DetectEquipent()
    {
        //屏幕中间发射射线
        //Vector3 screenMid = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        //Ray ray = Camera.main.ScreenPointToRay(screenMid);
        //鼠标位置发射射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag=="Equipment"&&raycastHit.distance < maxDetectDistance)
            {
                Debug.Log("碰撞成功");
            }
        }
    }

    private void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }


    /// <summary>
    /// 控制相机旋转
    /// </summary>
    private void CameraRotate()
    {
        if (m_axes == RotationAxes.MouseXAndY)
        {
            //用相对坐标
            float m_rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_sensitivityX;
            //Debug.Log(m_rotationX);
            m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
            m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

            transform.localEulerAngles = new Vector3(-m_rotationY, m_rotationX, 0);
        }
        else if (m_axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * m_sensitivityX, 0);
        }
        else
        {
            m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
            m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

            transform.localEulerAngles = new Vector3(-m_rotationY, transform.localEulerAngles.y, 0);
        }
    }
    /// <summary>
    /// 滚轮控制相机视角缩放
    /// </summary>
    private void CameraFOV()
    {
        //获取鼠标滚轮的滑动量
        float distance = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;
        distance = Mathf.Clamp(distance, m_minDistance, m_maxDistance);
        //改变相机的位置
        GameObject.FindGameObjectWithTag("MainCamera").transform.Translate(Vector3.forward * distance);
    }
}
