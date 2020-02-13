using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] const KeyCode rotateX = KeyCode.Q;
    [SerializeField] const KeyCode rotateY = KeyCode.E;
    [SerializeField] const KeyCode rotateZ = KeyCode.Space;
    private Vector3 mOffset;
    private float mZCoord;
    private bool mIsControl;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetControlState(false, false);
    }


    private void FixedUpdate()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        if (mIsControl)
        {
            RotateBind(rotateX, Vector3.right);
            RotateBind(rotateY, Vector3.up);
            RotateBind(rotateZ, Vector3.left);
        }
    }

    /// <summary>
    /// 绑定按键与旋转轴
    /// </summary>
    /// <param name="key">按键</param>
    /// <param name="axis">对应轴</param>
    private void RotateBind(KeyCode key,Vector3 axis)
    {
        if (Input.GetKeyDown(key))
        {
            SetControlState(true, false);
        }

        if (Input.GetKey(key))
        {
            transform.Rotate(axis, rotateSpeed, Space.World);
        }
        if (Input.GetKeyUp(key))
        {
            SetControlState(true, true);
        }
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        SetControlState(true, true);
    }
    private void OnMouseUp()
    {
        SetControlState(false, false);
    }

    void OnMouseEnter()
    {
        //TODO 改变鼠标样式
    }

    private void OnMouseExit()
    {

    }

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }


    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    /// <summary>
    /// 控制物品状态
    /// </summary>
    /// <param name="isControl">能否被控制</param>
    /// <param name="isFreeze">是否限制自由旋转</param>
    private void SetControlState(bool isControl, bool isFreeze)
    {
        mIsControl = isControl;
        rb.freezeRotation = isFreeze;
    }

}
