using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField]
    GameObject playerBody;
    [SerializeField]
    Rigidbody rigidBody;
    [SerializeField]
    CapsuleCollider capsuleCollider;

    public float turnSpeed; // ���콺 ȸ�� �ӵ�
    public float moveSpeed; // �̵� �ӵ�
    public float jumpForce; // ���� ����
    public Vector3 rotForward; // �÷��̾ ���� ����

    private bool isGround = true;

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Ground")
            isGround = true;    //Ground�� ������ isGround�� true
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseRotation();
        KeyboardMove();
    }
    void MouseRotation()
    {
        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;
       
        // ī�޶� ȸ������ ���뿡 �ݿ�(Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    void KeyboardMove()
    {
        // WASD Ű �Ǵ� ȭ��ǥŰ�� �̵����� ����
        Vector3 dir = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
        );

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {   // shift������ W�Է½� ������ ����
            dir.z *= 2;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {   // control������ ����Ű �Է½� ���� �̵�
            dir.z /= 2; dir.x /= 2;
        }

        if (Input.GetButton("Jump"))
        {   //���� Ű�� ������ ��
            if (isGround == true)
            {   //���� ������ ���� ��
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                isGround = false;
            }
        }

        // �̵����� * �ӵ� * �����Ӵ��� �ð��� ���ؼ� �÷��̾��� Ʈ�������� �̵�
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
