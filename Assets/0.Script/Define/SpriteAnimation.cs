using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    private List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer image;
    private SpriteRenderer subSprite;
    private UnityAction action;

    float[] pos;
    float delay, time = 0f;
    int animationCount = 0;
    bool loop = true;

    void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sprites.Count == 0)
            return;

        time += Time.deltaTime;
        if (time > delay)
        {
            time = 0f;

            if (subSprite != null)
            {
                Vector2 pos = subSprite.transform.localPosition;
                pos.y = this.pos[animationCount];
                subSprite.transform.localPosition = pos;
            }

            image.sprite = sprites[animationCount];
            animationCount++;

            if (animationCount > sprites.Count - 1)
            {
                if (loop)
                {
                    animationCount = 0;
                }
                else
                {
                    if(action != null)
                    {
                        Invoke("DealyAction", delay);
                    }
                    else
                    {
                        sprites.Clear();
                        DataInit();
                    }
                }
            }
        }
    }

    void DataInit()
    {
        image.enabled = true;
        time = float.MaxValue;
        animationCount = 0;

        subSprite = null;
        pos = null;
        action = null;
    }

    public void SetSprite(List<Sprite> sprites, float delay, bool loop = true)
    {
        DataInit();

        this.sprites = sprites.ToList();
        this.delay = delay;
        this.loop = loop;
        
        image.sprite = sprites[0];
    }

    public void SetSpritePos(List<Sprite> sprites, float delay, float[] pos, SpriteRenderer sub, bool loop = true)
    {
        DataInit();

        subSprite = sub;
        this.sprites = sprites.ToList();
        this.delay = delay;
        this.loop = loop;
        this.pos = pos;

        image.sprite = sprites[0];
    }

    public void SetSprite(List<Sprite> sprites, float delay, UnityAction action, bool loop = true)
    {
        DataInit();

        this.sprites = sprites.ToList();
        this.delay = delay;
        this.loop = loop;
        this.action = action;

        image.sprite = sprites[0];
    }

    void DealyAction()
    {
        sprites.Clear();
        image.sprite = null;
        animationCount = 0;
        image.enabled = false;

        action();
    }
}
