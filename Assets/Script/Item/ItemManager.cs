using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{// �̱������� �ٸ� cs���Ͽ��� ���� �����ϰ� �Ѵ�.
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (!_instance)
            {// �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                _instance = FindObjectOfType(typeof(ItemManager)) as ItemManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public enum ITEM_KIND
    {
        ERR = -1, NULL,
        MATERIAL,
        LIVING_TOOL,
        OTHERS
    }

    public struct Item
    {
        public string itemName;
        public ITEM_KIND itemKind;
    }

    const int BAG_MAX = 24;
    public Button[] m_BagSlot = new Button[BAG_MAX]; // �κ��丮 �߾ӽ���(24ĭ)
    string[] itemName = new string[BAG_MAX]; // ������ ������ �̸��� ����
    int[] itemQuantity = new int[BAG_MAX]; // ������ ������ ������ ����
     
    public void InsertItem(Item _item, int _quantity)
    {
        int idx = 0; // �κ��丮 ��ȸ �� �ε���
        bool sameKindFlg = false; // ���� ���� ������ ���� �� true

        for (idx = 0; idx < itemName.Length; idx++)
        {
            if (itemName[idx] == _item.itemName)
            {// ���� �̸� ������ �ִ��� Ȯ��
                sameKindFlg = true;
                break;
            }
        }

        if (sameKindFlg)
        { // ���� �̸� ������ ���� ��
            Text slotText = m_BagSlot[idx].transform.GetComponentInChildren<Text>();
            itemQuantity[idx] += _quantity;
            slotText.text = "x" + itemQuantity[idx];
            UI_OnOff.Instance.PopupUI(_item, _quantity);
        }
        else // ���� �̸� ������ ���� ��
        {
            for (idx = 0; idx < itemName.Length; idx++)
            {
                if (itemName[idx] == null)
                {// �κ��丮 �� ���� ������ ��
                    SetSlotImage(idx, _item);
                    itemName[idx] = _item.itemName;
                    itemQuantity[idx] = _quantity;
                    Text slotText = m_BagSlot[idx].transform.GetComponentInChildren<Text>();
                    slotText.text = "x" + itemQuantity[idx];
                    UI_OnOff.Instance.PopupUI(_item, _quantity);
                    break;
                }
            }
        }
    } // end of InsertItem()

    public Item ConfigItem(string itemTag)
    {
        Item item;

        switch (itemTag)
        { // ������ Ȯ��(����ü)
            case "Tree":
            case "TreeMark":
            case "Wood":
                item.itemKind = ITEM_KIND.MATERIAL;
                item.itemName = "����";
                return item;
            case "Rock":
                item.itemKind = ITEM_KIND.MATERIAL;
                item.itemName = "��";
                return item;
            case "Pumpkin":
                item.itemKind = ITEM_KIND.MATERIAL;
                item.itemName = "ȣ��";
                return item;
            default:
                item.itemKind = ITEM_KIND.ERR;
                item.itemName = "error";
                return item;
        }
    }

    void SetSlotImage(int _idx, Item _item)
    { // �κ��丮 �� �������̹��� ����
        Color newColor;
        switch (_item.itemName)
        {
            case "����": newColor = Color.green; break;
            case "��": newColor = Color.gray; break;
            case "ȣ��": newColor = new Color(1f, 0.59f, 0f); break;
            default: newColor = Color.black; break;
        }
        ColorBlock cb = m_BagSlot[_idx].colors;
        cb.normalColor = newColor;
        m_BagSlot[_idx].colors = cb;

    }
}
