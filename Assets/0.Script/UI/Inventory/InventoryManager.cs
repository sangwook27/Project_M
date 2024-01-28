using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    // 9. 18�� �׽�Ʈ
    public Transform parent;
    public InventoryItem item;
    // �κ��丮(��ü)����
    //[SerializeField] private GameObject invenBG;

    //[SerializeField] private InventoryItem item;
    //[SerializeField] private Transform parent;
    [SerializeField] private List<Sprite> itemIcons;
    private List<InventoryItem> items = new List<InventoryItem>();
    public TMP_Text mesoTxt;

    private int invenX = 5;
    private int invenY = 8;

    private bool isOpen = false;

    private int meso;
    public int Meso
    {
        get { return meso; }
        set
        {
            meso = value;
            mesoTxt.text = string.Format("{0:#,###}", meso);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //invenBG.SetActive(false);
        Meso = 0;
    }
    private void Update()
    {
        /*
        // �κ��丮 ���� �ִ� GameObj Ű�� ��
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            invenBG.SetActive(isOpen);
        }
        */
        //========������ �׽�Ʈ============
        // ������ ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int rand = Random.Range(0, itemIcons.Count);
            // �������� �ִ��� ������ üũ
            bool isAdd = true;
            InventoryItem iItem = null;
            for (int i = 0; i < items.Count; i++)
            {
                if (itemIcons[rand].name == items[i].SpriteName)
                {
                    isAdd = false;
                    iItem = items[i];
                    break;
                }
            }

            // ������ ������ ����
            if (isAdd)
            {
                InventoryItem i = Instantiate(item, parent);
                i.ItemIcon(itemIcons[rand]);
                i.ItemCount(1);
                items.Add(i);
            }
            // ������ ī��Ʈ����
            else if (iItem != null)
            {
                iItem.ItemCount(iItem.Count + 1);
            }
        }
        // ������
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // ���� ������
            Meso += Random.Range(5, 100);
        }
    }
}
