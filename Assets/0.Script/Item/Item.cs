using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    private SpriteAnimation sa;
    // Start is called before the first frame update
    public virtual void Init()
    {
        sa = GetComponent<SpriteAnimation>();
        sa.SetSprite(sprites, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
