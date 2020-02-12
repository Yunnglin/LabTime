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

            if (Input.GetKeyDown(rotateX) || Input.GetKeyDown(rotateY) || Input.GetKeyDown(rotateZ))
            {
                SetControlState(true, false);
            }

            if (Input.GetKey(rotateX))
            {
                transform.Rotate(Vector3.right, rotateSpeed, Space.World);
            }
            else if (Input.GetKey(rotateY))
            {
                transform.Rotate(Vector3.up, rotateSpeed, Space.World);
            }
            else if (Input.GetKey(rotateZ))
            {
                transform.Rotate(Vector3.left, rotateSpeed, Space.World);
            }
            if (Input.GetKeyUp(rotateX) || Input.GetKeyUp(rotateY) || Input.GetKeyUp(rotateZ))
            {
                SetControlState(true, true);
            }
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
