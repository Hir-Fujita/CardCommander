using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_process : MonoBehaviour
{
    public CardModel model; // カードのデータを処理

    string user;
    string target;
    public Sprite wait;
    public Sprite toumei;
    public Sprite[] hits;
    public Sprite[] uses;
    public Sprite[] destroy;

    public IEnumerator Wait_temp()
    {
        GameObject child = transform.Find("Effect").gameObject;
        Image image = child.GetComponent<Image>();
        image.sprite = wait;
        float i = 0;
        bool flag = true;
        while (GameManager.turn_flag == 0)
        {
            if (flag)
            {
                i = i + 0.05f;
            }
            else
            {
                i = i - 0.05f;
            }
            image.color = new Color(image.color[0], image.color[1], image.color[2], i);
            yield return new WaitForSeconds(0.01f);
            if (i >= 1)
            {
                flag = false;
                yield return new WaitForSeconds(0.5f);
            }
            if (i <= 0)
            {
                flag = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
        image.sprite = toumei;
        image.color = new Color(image.color[0], image.color[1], image.color[2], 1);
        yield break;
    }

    public IEnumerator Wait_hand()
    {
        GameObject child = transform.Find("Effect").gameObject;
        Image image = child.GetComponent<Image>();
        image.sprite = wait;
        float i = 0;
        bool flag = true;
        while (GameManager.turn_flag == 1)
        {
            if (flag)
            {
                i = i + 0.05f;
            }
            else
            {
                i = i - 0.05f;
            }
            image.color = new Color(image.color[0], image.color[1], image.color[2], i);
            yield return new WaitForSeconds(0.01f);
            if (i >= 1)
            {
                flag = false;
                yield return new WaitForSeconds(0.5f);
            }
            if (i <= 0)
            {
                flag = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
        image.sprite = toumei;
        image.color = new Color(image.color[0], image.color[1], image.color[2], 1);
        yield break;
    }

    public IEnumerator Wait_flag()
    {
        GameObject child = transform.Find("Effect").gameObject;
        Image image = child.GetComponent<Image>();
        image.sprite = wait;
        float i = 0;
        bool flag = true;
        while (GameManager.flag != "wait")
        {
            if (flag)
            {
                i = i + 0.05f;
            }
            else
            {
                i = i - 0.05f;
            }
            image.color = new Color(image.color[0], image.color[1], image.color[2], i);
            yield return new WaitForSeconds(0.01f);
            if (i >= 1)
            {
                flag = false;
                yield return new WaitForSeconds(0.5f);
            }
            if (i <= 0)
            {
                flag = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
        image.sprite = toumei;
        image.color = new Color(image.color[0], image.color[1], image.color[2], 1);
        yield break;
    }

    public IEnumerator Hit()
    {
        GameObject child = transform.Find("Effect").gameObject;
        Image image = child.GetComponent<Image>();
        foreach (var i in hits)
        {
            image.sprite = i;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator Uses()
    {
        GameObject child = transform.Find("Effect").gameObject;
        Image image = child.GetComponent<Image>();
        image.color = new Color(image.color[0], image.color[1], image.color[2], 1);
        foreach (var i in uses)
        {
            image.sprite = i;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator Blackout()
    {
        GameObject img = transform.Find("Image").gameObject;
        Image image = img.GetComponent<Image>();
        image.color = new Color(image.color[0], image.color[1], image.color[2], 1);
        for (float nu = 1; nu > 0; nu = nu - 0.1f)
        {
            image.color = new Color(nu, nu, nu, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator DestroyCard()
    {
        GameObject child = transform.Find("Effect").gameObject;
        Image image = child.GetComponent<Image>();
        image.color = new Color(image.color[0], image.color[1], image.color[2], 1);
        foreach (var i in destroy)
        {
            image.sprite = i;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public IEnumerator Run(Timing t, string User)
    {
        CardController card = this.transform.GetComponent<CardController>();
        if (User == "player")
        {
            user = "Player";
            target = "Enemy";
        }
        if (User == "enemy")
        {
            user = "Enemy";
            target = "Player";
        }

        if (t == Timing.WIN)
        {
            yield return StartCoroutine(Uses());
        }
        else if (t == Timing.LOSE)
        {
            yield return StartCoroutine(Uses());
        }

        else if (t == Timing.USE)
        {
            if (card.model.color_name == "red")
            {
                switch (card.model.cardID)
                {
                    case 100://赤い煙
                        yield return StartCoroutine(Uses());
                        red.instance.red_100(user);
                        break;

                    case 103://サソリ
                        break;

                    case 114://ゴブリンメイジ
                        yield return StartCoroutine(Uses());
                        yield return red.instance.StartCoroutine(red.instance.red_114(user));
                        break;

                    case 117://翼竜
                        yield return StartCoroutine(Uses());
                        yield return red.instance.StartCoroutine(red.instance.red_117(user));
                        break;

                    case 124://ミノタウルス
                        break;

                    case 127://赤鬼
                        break;

                    case 128://ゴブリン戦車
                        yield return StartCoroutine(Uses());
                        yield return red.instance.StartCoroutine(red.instance.red_128(user));
                        break;

                    case 132://炎使い
                        yield return StartCoroutine(Uses());
                        yield return red.instance.StartCoroutine(red.instance.red_132(user));
                        break;

                    case 147://赤ドラゴン
                        yield return StartCoroutine(Uses());
                        yield return red.instance.StartCoroutine(red.instance.red_147(user));
                        break;

                    default:
                        Debug.Log("------------カード効果未実装------------");
                        StartCoroutine(Uses());
                        break;
                }
            }


            else if (card.model.color_name == "blue")
            {
                switch (card.model.cardID)
                {
                    case 200://青い煙
                        yield return StartCoroutine(Uses());
                        blue.instance.blue_200(user);
                        break;

                    case 201://タツノオトシゴ
                        break;

                    case 203://タコ
                        yield return StartCoroutine(Uses());
                        yield return blue.instance.StartCoroutine(blue.instance.blue_203(user));
                        break;

                    case 219://ウミガメ
                        yield return StartCoroutine(Uses());
                        yield return blue.instance.StartCoroutine(blue.instance.blue_219(user));
                        break;

                    case 226://青鬼
                        break;

                    case 227://人魚マダム
                        break;

                    case 229://河童
                        yield return StartCoroutine(Uses());
                        blue.instance.blue_229(user);
                        break;

                    case 245://ヤマタノオロチ
                        break;

                    case 250://シーサーペント
                        yield return StartCoroutine(Uses());
                        yield return blue.instance.StartCoroutine(blue.instance.blue_250(user));
                        break;

                    default:
                        Debug.Log("------------カード効果未実装------------");
                        StartCoroutine(Uses());
                        break;
                }
            }




            else if (card.model.color_name == "black")
            {
                switch (card.model.cardID)
                {
                    case 300://黒い煙
                        yield return StartCoroutine(Uses());
                        black.instance.black_300(user);
                        break;

                    case 301://コウモリ
                        break;

                    case 314://ミミック
                        yield return StartCoroutine(Uses());
                        yield return black.instance.StartCoroutine(black.instance.black_314(user));
                        break;

                    case 320://ガーゴイル
                        break;

                    case 325://吸血鬼
                        break;

                    case 329://ネクロマンサー
                        yield return StartCoroutine(Uses());
                        black.instance.black_329(user);
                        break;

                    case 339://オルトロス
                        break;

                    case 340://死神
                        yield return StartCoroutine(Uses());
                        yield return black.instance.StartCoroutine(black.instance.black_340(user));
                        break;

                    case 343://黒ドラゴン
                        yield return StartCoroutine(Uses());
                        black.instance.black_343(user);
                        break;

                    default:
                        Debug.Log("------------カード効果未実装------------");
                        StartCoroutine(Uses());
                        break;
                }
            }




            else if (card.model.color_name == "green")
            {
                switch (card.model.cardID)
                {
                    case 400://緑の煙
                        yield return StartCoroutine(Uses());
                        green.instance.green_400(user);
                        break;

                    case 401://アルマジロ
                        break;

                    case 415://エルフ
                        yield return StartCoroutine(Uses());
                        green.instance.green_415(user);
                        break;

                    case 425://ゾウ
                        break;

                    case 427://植物使い
                        yield return StartCoroutine(Uses());
                        yield return green.instance.StartCoroutine(green.instance.green_427(user));
                        break;

                    case 428://狩人
                        yield return StartCoroutine(Uses());
                        yield return green.instance.StartCoroutine(green.instance.green_428(user));
                        break;

                    case 439://人面樹
                        yield return StartCoroutine(Uses());
                        green.instance.green_439(user);
                        yield return new WaitForSeconds(0.1f);
                        break;

                    case 443://サイクロプス
                        yield return StartCoroutine(Uses());
                        yield return green.instance.StartCoroutine(green.instance.green_443(user));
                        break;

                    case 446://緑ドラゴン
                        yield return StartCoroutine(Uses());
                        yield return green.instance.StartCoroutine(green.instance.green_446(user));
                        break;

                    default:
                        Debug.Log("------------カード効果未実装------------");
                        StartCoroutine(Uses());
                        break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }


    }

}
