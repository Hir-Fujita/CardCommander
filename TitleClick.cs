using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//カードのクリックを制御するクラス

public class TitleClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler //IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    [SerializeField] CardController cardPrefab;
    [SerializeField] GameObject temp_window;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Enemy;
    [SerializeField] Text info;
    [SerializeField] GameObject[] icons;
    [SerializeField] Text Player_Name;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.name == "start_cpu")
        {
            if (GameManager.entry_deck != null)
            {
                if (GameManager.entry_deck.Length == 27)
                {
                    GameManager.ONLINE = false;
                    GameManager.instance.StartGame();
                }
                else
                {
                    GameManager.instance.StartCoroutine(GameManager.instance.Info("デッキの枚数が不正です"));
                }
            }
            else
            {
                GameManager.instance.StartCoroutine(GameManager.instance.Info("デッキが選択されていません"));
            }
        }

        if (transform.name == "start_online")
        {
            if (GameManager.entry_deck != null)
            {
                if (GameManager.entry_deck.Length == 27)
                {
                    if (Player_Name.text == "")
                    {
                        GameManager.instance.StartCoroutine(GameManager.instance.Info("プレイヤー名を入力してください"));
                    }
                    else
                    {
                        OnlineManager.instance.Join_Room();
                    }
                }
                else
                {
                    GameManager.instance.StartCoroutine(GameManager.instance.Info("デッキの枚数が不正です"));
                }
            }
            else
            {
                GameManager.instance.StartCoroutine(GameManager.instance.Info("デッキが選択されていません"));
            }
        }

        if (transform.name == "red")
        {
            int[] ary = { 100, 100, 100, 103, 103, 103, 114, 114, 114, 124, 124, 124, 117, 117, 117, 127, 127, 127, 132, 132, 132, 128, 128, 128, 147, 147, 147 };
            GameManager.entry_deck = ary;
            GameManager.instance.select_starter(0);
        }
        if (transform.name == "blue")
        {
            int[] ary = {200,200,200,201,201,201,203,203,203,219,219,219,226,226,226,227,227,227,229,229,229,245,245,245,250,250,250};
            GameManager.entry_deck = ary;
            GameManager.instance.select_starter(1);
        }
        if (transform.name == "black")
        {
            int[] ary = { 300, 300, 300, 301, 301, 301, 314, 314, 314, 320, 320, 320, 325, 325, 325, 329, 329, 329, 339, 339, 339, 340, 340, 340, 343, 343, 343 };
            GameManager.entry_deck = ary;
            GameManager.instance.select_starter(2);
        }
        if (transform.name == "green")
        {
            int[] ary = { 400, 400, 400, 401, 401, 401, 415, 415, 415, 425, 425, 425, 427, 427, 427, 428, 428, 428, 439, 439, 439, 443, 443, 443, 446, 446, 446 };
            GameManager.entry_deck = ary;
            GameManager.instance.select_starter(3);
        }








        if (transform.name == "cansel")
        {
            temp_window.SetActive(false);
            CardController[] temp_window_count = temp_window.GetComponentsInChildren<CardController>();
            foreach (CardController c in temp_window_count)
            {
                Destroy(c.gameObject);
            }

        }
        if (transform.name == "p1_trash")
        {
            Player P = Player.GetComponent<Player>();
            temp_window.SetActive(true);
            foreach (int i in P.trash)
            {
                CardController card = Instantiate(cardPrefab, panel.transform);
                card.Init(i);
                card.name = card.model.name;
            }
        }
        if (transform.name == "p2_trash")
        {
            Player E = Enemy.GetComponent<Player>();
            temp_window.SetActive(true);
            foreach (int i in E.trash)
            {
                CardController card = Instantiate(cardPrefab, panel.transform);
                card.Init(i);
                card.name = card.model.name;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}