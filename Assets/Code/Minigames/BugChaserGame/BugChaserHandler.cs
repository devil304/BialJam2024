using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BugChaserHandler : MonoBehaviour
{
    public int score;
    public GameObject prefab;
    public Transform swapper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        StartCoroutine(NewBug());
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = GameManager.Instance.MainInput.Main.MousePos.ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = -1;
        swapper.position = mousePos;
            
    }

    IEnumerator NewBug()
    {
        CreateNewBug(prefab);

        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            CreateNewBug(prefab);
        }
    }

    void CreateNewBug(GameObject bugPrefab)
    {
        GameObject newBug = Instantiate(bugPrefab,this.transform,false);
        newBug.transform.localPosition = new Vector3(StrongRandom.RNG.Next(-900, 900) / 2000f, StrongRandom.RNG.Next(-900, 900) / 2000f, 0);
        newBug.GetComponent<BugScript>().handler = this;
    }


}
