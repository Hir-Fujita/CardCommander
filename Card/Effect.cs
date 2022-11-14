using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public Sprite[] sprites;
    public float speed;

    public static Effect instance;

void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

public void Hit_effect()
{
    StartCoroutine(img_update());
}

IEnumerator img_update()
{
    GameObject img = this.gameObject;
    img.SetActive(true);
    // Image image = this.transform.GetComponent<Image>;
    print(this.transform);
    foreach(Sprite i in sprites)
    {
        // image.sprite = i;
        yield return new WaitForSeconds(0.01f);
        // print("描写中");
        // print(i);
    }
}
}
