using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour
{
    [SerializeField]
    Canvas m_gatheringUI;
    [SerializeField]
    Canvas m_gatheringClearUI;

    public float turnSpeed; // ���콺 ȸ�� �ӵ�
    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )

    private bool rayCollision = false;  // �÷��̾ ���� ���⿡ ä������ �ִ��� ����
    private const float RAY_COLLISION_MAX_DISTANCE = 2.0f;    // �����浹 �ִ� �Ÿ�
    private Text raycastTxt;
    private Text gatheringClearTxt;
    private GameObject clearObj;

    private void Start()
    {
        //UI�� �ʱ⿡ ��ǥ�û��·� ����
        m_gatheringUI.enabled = false;
        m_gatheringClearUI.enabled = false;
        // 
        raycastTxt = GameObject.Find("GatheringText").GetComponent<Text>();
        gatheringClearTxt = GameObject.Find("GatheringClearText").GetComponent<Text>();
    }

    void Update()
    {
        RaycastControl();   // �÷��̾� ���� ���⿡ �����浹
        MouseRotation();    // ���콺�� �����ӿ� ���� ī�޶� ȸ��
        KeyboardControl();  // Ű���� �Է� ��Ʈ��
    }

    void MouseRotation()
    {
        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
    void KeyboardControl()
    {

    }
    void RaycastControl()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Gathering"); 
        RaycastHit hitInfo;

        rayCollision = Physics.Raycast(this.transform.position,
           transform.forward,
           out hitInfo,
           RAY_COLLISION_MAX_DISTANCE,
           layerMask
           );  // �÷��̾� �Ӹ�����, ���¹�������, 2.0f�Ÿ���, ä���� ���̾�� �浹�Ұ��

        if (rayCollision)
        {   // <EŰ�� ���� ��ȣ�ۿ�> �ؽ�ƮUI ����
            m_gatheringUI.enabled = true;
            switch (hitInfo.collider.tag)
            {
                case "Tree":
                    raycastTxt.text = "����";
                    break;
                case "Rock":
                    raycastTxt.text = "����";
                    break;
                case "Pumpkin":
                    raycastTxt.text = "ȣ��";
                    break;
                default: 
                    break;
            }
            raycastTxt.text += "\nEŰ�� ���� ��ȣ�ۿ�";

            if (Input.GetKeyDown(KeyCode.E))
            {   // UI ���� �� ��ȣ�ۿ� Ű �Է� �� ���� �� ����������Ʈ ����
                gatheringClearTxt.text += "\n" + hitInfo.collider.tag.ToString() + "+1";
                m_gatheringClearUI.enabled = true;

                InsertItemToInventory(hitInfo.collider.tag.ToString());

                clearObj = hitInfo.transform.gameObject;
                Destroy(clearObj);
            }
        }
        else
            m_gatheringUI.enabled = false;
    }


    private const int BAG_MAX = 24;
    public Button[] SlotImage = new Button[BAG_MAX];
    private bool[] isFilled = new bool[BAG_MAX];

    public void InsertItemToInventory(string str1)
    {
        for (int i = 0; i < BAG_MAX; i++)
        {
            if (isFilled[i] == false)
            {
                ColorBlock cb = SlotImage[i].colors;
                Color newColor = Color.green;
                cb.normalColor = newColor;
                SlotImage[i].colors = cb;

                isFilled[i] = true;
                break;
            }
        }
    }
}
