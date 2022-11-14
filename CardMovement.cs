using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//カードのクリックを制御するクラス

public class CardMovement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler //IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;

    public void OnPointerClick(PointerEventData eventData)
    {
        //クリック時の処理
        if (transform.name == "exit")
        {
            GameManager.instance.Exit();
        }
        else if (GameManager.turn_flag == 0)
        {
            cardParent = transform.parent;
            if (transform.name == "skip")
            {
                GameManager.turn_flag = 1;
                BattleManager.instance.turn_calc();
            }
            if (cardParent.name == "p1_temp")
            {
                GameObject trans = GameObject.Find("p1_field");
                CardController[] p1_field_list = trans.GetComponentsInChildren<CardController>();
                if (p1_field_list.Length == 0)//待機所をクリック、かつ戦場にカードがいなかったら
                {
                    transform.tag = "battle";
                    GameManager.turn_flag = 1;
                    BattleManager.instance.turn_calc();
                }
            }
        }
        else if (GameManager.turn_flag == 1)
        {
            if (transform.name == "skip")
            {
                GameManager.turn_flag = 2;
                BattleManager.instance.turn_calc();
            }
            else if (cardParent.name == "hand")
            {
                GameObject trans = GameObject.Find("p1_use");
                CardController[] p1_use_list = trans.GetComponentsInChildren<CardController>();
                if (p1_use_list.Length == 0)//手札をクリック
                {
                    CardController card = transform.GetComponent<CardController>();
                    GameObject player = GameObject.Find("Player");
                    Player p = player.GetComponent<Player>();
                    if (card.model.cost <= p.mp)
                    {
                        transform.tag = "use";
                        GameManager.turn_flag = 2;
                        BattleManager.instance.turn_calc();
                    }
                    else
                    {
                        CardController c = transform.GetComponent<CardController>();
                        if (c.model.name == "赤ドラゴン")
                        {
                            int count = 0;
                            foreach (int i in p.hand)
                            {
                                if (i == 108)
                                {
                                    count = count + 1;
                                }
                            }
                            if (c.model.cost - count <= p.mp)
                            {
                                transform.tag = "use";
                                GameManager.turn_flag = 2;
                                BattleManager.instance.turn_calc();
                            }
                        }
                    }
                }
            }
        }
        else if (GameManager.turn_flag == 2)
        {
            if (GameManager.flag == "field")
            {
                if (transform.parent.name == "p1_field" || transform.parent.name == "p2_field")
                {
                    if (GameManager.ONLINE)
                    {
                        if (transform.parent.name == "p1_field")
                        {
                            GameManager.instance.SendCard(1, "select");
                        }
                        else if (transform.parent.name == "p2_field")
                        {
                            GameManager.instance.SendCard(0, "select");
                        }
                    }
                    GameManager.obj = transform.gameObject;
                    GameManager.flag = "wait";
                }
            }
            if (GameManager.flag == "temp")
            {
                if (transform.parent.name == "p1_temp")
                {
                    if (GameManager.ONLINE)
                    {
                        CardController[] objs = transform.parent.gameObject.GetComponentsInChildren<CardController>();
                        for (int i = 0; i < objs.Length; i++)
                        {
                            if (objs[i].gameObject == transform.gameObject)
                            {
                                GameManager.instance.SendCard(i, "select");
                                break;
                            }
                        }
                    }
                    GameManager.obj = transform.gameObject;
                    GameManager.flag = "wait";
                }
            }
            if (GameManager.flag == "select")
            {
                if (transform.parent.name == "Panel")
                {
                    GameManager.obj = transform.gameObject;
                    GameManager.flag = "wait";
                    if (GameManager.ONLINE)
                    {
                        CardController cc = GameManager.obj.GetComponent<CardController>();
                        GameManager.instance.SendCard(cc.model.cardID, "select");
                    }
                }
            }
            if (GameManager.flag == "field_blue_219")//ウミガメ使用時の処理
            {
                if (transform.parent.name == "p1_field")
                {
                    int num = int.Parse(BattleManager.instance.lv_var.text);
                    if (num < 2)
                    {
                        if (GameManager.ONLINE)
                        {
                            GameManager.instance.SendCard(1, "select");
                        }
                        GameManager.obj = transform.gameObject;
                        GameManager.flag = "wait";
                    }
                }
                if (transform.parent.name == "p2_field")
                {
                    int num = int.Parse(BattleManager.instance.enemy_lv_var.text);
                    if (num < 2)
                    {
                        if (GameManager.ONLINE)
                        {
                            GameManager.instance.SendCard(0, "select");
                        }
                        GameManager.obj = transform.gameObject;
                        GameManager.flag = "wait";
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.name != "skip" && transform.name != "exit")
        {
            cardParent = transform.parent;
            CardController card = transform.GetComponent<CardController>();
            Card_data_view c = GameObject.Find("Card_data_view").GetComponent<Card_data_view>();
            c.Init(card);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.name != "skip" && transform.name != "exit")
        {
            Card_data_view c = GameObject.Find("Card_data_view").GetComponent<Card_data_view>();
            c.Close();
        }
    }
}