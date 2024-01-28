using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayUI : MonoBehaviour
{
    public Image hp;
    [SerializeField] private Image level;
    public TMP_Text levelTxt;

    [SerializeField] private Image chaPick;

    int lev = 0;

    private void Start()
    {
        var l = FindObjectsOfType<PlayUI>();
        if (l.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        level.fillAmount = 0;
        levelTxt.text = lev.ToString();
    }
    private void Update()
    {
        if (level.fillAmount == 1)
        {
            levelTxt.text += lev.ToString();
            level.fillAmount = 0;
        }
    }

    public void Level()
    {
        level.fillAmount += 0.5f;
    }
    public void OnSword()
    {
        ImageValue();
    }
    public void OnArc()
    {
        ImageValue();
    }
    void ImageValue()
    {
        chaPick.gameObject.SetActive(false);
    }
}
