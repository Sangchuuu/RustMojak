using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OnOff : MonoBehaviour
{// �̱������� �ٸ� cs���Ͽ��� ���� �����ϰ� �Ѵ�.
    private static UI_OnOff _instance;
    public static UI_OnOff Instance
    {
        get 
        {
            if (!_instance)
            { // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                _instance = FindObjectOfType(typeof(UI_OnOff)) as UI_OnOff;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public Canvas m_Inventory;
    public Canvas m_gatheringUI;
    public Canvas m_gatheredUI;

    private bool isInventoryOn = false;
    private Text raycastTxt;
    private Text gatheredTxt;
    private GameObject clearObj;

    void Start()
    {
        //UI�� �ʱ⿡ ��ǥ�û��·� ����
        m_gatheringUI.enabled = false;
        m_gatheredUI.enabled = false;

        raycastTxt = m_gatheringUI.gameObject.GetComponentInChildren<Text>();
        gatheredTxt = m_gatheredUI.gameObject.GetComponentInChildren<Text>();
    }
    void Update()
    {
        InventoryOnOff();
    }

    void InventoryOnOff()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            isInventoryOn = !isInventoryOn;

        if (isInventoryOn)
        {
            m_Inventory.enabled = true;
            Cursor.visible = true;
        }
        else
        {
            m_Inventory.enabled = false;
            Cursor.visible = false;
        }
    }
    public void RaycastUI(bool _rayCollision, RaycastHit _hitInfo)
    {
        if (_rayCollision)
        {   // <EŰ�� ���� ��ȣ�ۿ�> �ؽ�ƮUI ����
            m_gatheringUI.enabled = true;
            switch (_hitInfo.collider.tag)
            {
                case "Wood":
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
                ItemManager.Item item;
                item = ItemManager.Instance.ConfigItem(_hitInfo.collider.tag.ToString());
                ItemManager.Instance.InsertItem(item, 1);

                clearObj = _hitInfo.transform.gameObject;
                Destroy(clearObj);
            }
        }
        else
            m_gatheringUI.enabled = false;
    }

    public void PopupUI(ItemManager.Item _item, int _quantity)
    {
        gatheredTxt.text += "\n" + _item.itemName + "+" + _quantity;
        m_gatheredUI.enabled = true;
    }
}
