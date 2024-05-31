using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StarLabelScript : MonoBehaviour
{

    [SerializeField] List<SpriteRenderer> sr;
    [SerializeField] List<Sprite> icons;
    [SerializeField] SpriteRenderer iconRender;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitStats(float stats, int id)
    {
        int a = Mathf.RoundToInt(stats.Remap(0, 100f, 0, 6f));

        for(int i = 0; i < a; i++)
        {
            sr[i].enabled = true;
        }

        iconRender.sprite = icons[id];
    }
}
