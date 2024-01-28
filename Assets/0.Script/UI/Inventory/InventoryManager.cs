using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    // 9. 18일 테스트
    public Transform parent;
    public InventoryItem item;
    // 인벤토리(전체)제어
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
        // 인벤토리 끄고 있는 GameObj 키는 거
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            invenBG.SetActive(isOpen);
        }
        */
        //========아이템 테스트============
        // 아이템 스폰
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int rand = Random.Range(0, itemIcons.Count);
            // 아이템이 있는지 없는지 체크
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

            // 없으면 아이템 생성
            if (isAdd)
            {
                InventoryItem i = Instantiate(item, parent);
                i.ItemIcon(itemIcons[rand]);
                i.ItemCount(1);
                items.Add(i);
            }
            // 있으면 카운트증가
            else if (iItem != null)
            {
                iItem.ItemCount(iItem.Count + 1);
            }
        }
        // 돈생성
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // 랜덤 돈생성
            Meso += Random.Range(5, 100);
        }
    }
}
