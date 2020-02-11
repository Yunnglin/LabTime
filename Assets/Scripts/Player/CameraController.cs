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
    [SerializeField] float maxDetectDistance = 2f;

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

    // 要改变的鼠标样式图片
    public Texture2D cursorTextureHand;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private GameObject pickGameObj = null;

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
        DetectEquipent("Plane", "Equipment");
    }

    /// <summary>
    /// 检测碰撞,并拖拽
    /// </summary>
    private void DetectEquipent(string planeTag, string targetTag)
    {
        //屏幕中间发射射线
        //Vector3 targetPos = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        //鼠标位置发射射线
        Vector3 targetPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(targetPos);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        //目前没有拾起物品
        if (pickGameObj == null)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag(targetTag))
                {
                    Debug.Log("碰撞成功" + hitInfo.collider.name);
                    //切换鼠标样式
                    //Cursor.SetCursor(cursorTextureHand, hotSpot, cursorMode);

                    if (Input.GetMouseButton(0))
                    {
                        //存储被抓取的对象
                        pickGameObj = hitInfo.collider.gameObject;
                    }
                }
            }
        }
        
        //如果有拾取的物体，并且鼠标与地面碰撞，则使物体移动
        if (pickGameObj != null)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag(planeTag))
                {
                    pickGameObj.GetComponent<Rigidbody>().freezeRotation = true;
                    pickGameObj.transform.position = new Vector3(
                        hitInfo.point.x,
                        hitInfo.point.y,
                        hitInfo.point.z + pickGameObj.GetComponent<Collider>().bounds.size.y / 2);
                }
                
                Debug.Log("第二次发射" + hitInfo.collider.name);
            }
        }
        //释放物体
        if (Input.GetMouseButtonUp(0))
        {
            pickGameObj.GetComponent<Rigidbody>().freezeRotation = false;
            pickGameObj = null;
        }
    }

    /// <summary>
    /// 跟随玩家
    /// </summary>
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
