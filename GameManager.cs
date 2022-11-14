// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//ゲーム全体の処理を制御するクラス
public enum Timing
{ USE, TEMP, WIN, LOSE }
public enum Point
{ USE, TEMP, FIELD }
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public static bool ONLINE;
    public static string my_name;
    public static bool online_wait;
    public static int online_num;
    public static bool enemy_use_flag;
    [SerializeField] Text player_name;
    [SerializeField] Text enemy_name;


    [SerializeField] GameObject main_canvas;
    [SerializeField] GameObject Title;
    [SerializeField] GameObject temp_window;
    [SerializeField] Text info;
    [SerializeField] GameObject[] icons;
    public static int[] entry_deck;
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand;
    [SerializeField] GameObject message_window;
    [SerializeField] Text viewtext;
    [SerializeField] public GameObject player;
    [SerializeField] public Transform player_field;
    [SerializeField] public Transform player_temp;

    [SerializeField] public GameObject enemy;
    [SerializeField] public Transform enemy_field;
    [SerializeField] public Transform enemy_temp;
    [SerializeField] Transform enemy_use;
    [SerializeField] Text wait_matching;

    public static int turn_count = 0;
    public static int turn_flag = 0;
    public static int temp_count = 0;
    public static string flag = "wait";
    public static GameObject obj;
    public static bool start_ini = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        main_canvas.SetActive(false);
        temp_window.SetActive(false);
        icons[0].SetActive(false);
        icons[1].SetActive(false);
        icons[2].SetActive(false);
        icons[3].SetActive(false);
        info.gameObject.SetActive(false);
        wait_matching.gameObject.SetActive(false);
        OnlineManager.instance.OnlineStart();
    }

    public void Exit()
    {
        if (ONLINE)
        {
            photonView.RPC(nameof(RPC_Enemy_Exit), RpcTarget.Others);
            PhotonNetwork.LeaveRoom();
        }
        main_canvas.SetActive(false);
        Title.SetActive(true);
        ONLINE = false;
        online_num = 0;
        online_wait = false;
        start_ini = false;
        BattleManager.instance.Restart();
        turn_count = 0;
        turn_flag = 0;
        temp_count = 0;
        flag = "wait";
        wait_matching.gameObject.SetActive(false);
        CardController[] cc = main_canvas.GetComponentsInChildren<CardController>();
        foreach (CardController c in cc)
        {
            Destroy(c.gameObject);
        }
    }

    public void StartGame()
    {
        //カードの詳細表示を制作と同時に非表示にする
        main_canvas.SetActive(true);
        Title.SetActive(false);
        Card_data_view c = GameObject.Find("Card_data_view").GetComponent<Card_data_view>();
        c.Close();
        message_window.SetActive(false);
        //適当なデッキを配列で生成。赤スターター
        Player P = player.GetComponent<Player>();
        Player E = enemy.GetComponent<Player>();
        for (int i = 27 - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1); // ランダムで要素番号を１つ選ぶ（ランダム要素）
            var temps = entry_deck[i]; // 一番最後の要素を仮確保（temp）にいれる
            entry_deck[i] = entry_deck[j]; // ランダム要素を一番最後にいれる
            entry_deck[j] = temps; // 仮確保を元ランダム要素に上書き
        }

        if (ONLINE == false)
        {
            int num = Random.Range(0, 2);
            if (num == 0)
            {
                P.initiative = true;
            }
            else
            {
                P.initiative = false;
            }
            if (P.initiative)
            {
                E.initiative = false;
            }
            else
            {
                E.initiative = true;
            }
            //デッキ生成と手札を同時に行う
            E.Deck_create(entry_deck);
        }
        else
        {
            P.initiative = start_ini;
            SendName_and_Data(my_name, entry_deck);
            player_name.text = my_name;
        }

        P.Deck_create(entry_deck);
        P.hp = 4;
        P.mp = 1;
        P.mp_reg = 2;
        P.Stat_update();

        E.hp = 4;
        E.mp = 1;
        E.mp_reg = 2;
        E.Stat_update();
        BattleManager.instance.Start_turn();
    }

    public void CreateCard(int cardID)
    {
        //カードを手札に生成する
        CardController card = Instantiate(cardPrefab, playerHand);
        card.Init(cardID);
        card.name = card.model.name;
    }

    public IEnumerator Info(string data)
    {
        info.text = data;
        info.color = new Color(info.color[0], info.color[1], info.color[2], 0);
        info.gameObject.SetActive(true);
        float i = 0f;
        while (i < 1f)
        {
            i = i + 0.5f;
            info.color = new Color(info.color[0], info.color[1], info.color[2], i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        while (i > 0f)
        {
            i = i - 0.3f;
            info.color = new Color(info.color[0], info.color[1], info.color[2], i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        info.gameObject.SetActive(false);
    }

    public void select_starter(int i)
    {
        icons[0].SetActive(false);
        icons[1].SetActive(false);
        icons[2].SetActive(false);
        icons[3].SetActive(false);
        icons[i].SetActive(true);
    }

    public void Wait_Matching()
    {
        wait_matching.gameObject.SetActive(true);
    }

    public void SendName_and_Data(string name, int[] ary)
    {
        photonView.RPC(nameof(ResName_and_Data), RpcTarget.Others, name, ary);
    }

    [PunRPC]
    void ResName_and_Data(string name, int[] ary)
    {
        Player E = enemy.GetComponent<Player>();
        enemy_name.text = name;
        E.Deck_create(ary);
    }

    public void Send()
    {
        photonView.RPC(nameof(Res), RpcTarget.Others);
    }

    [PunRPC]
    void Res()
    {
        if (online_wait)
        {
            online_wait = false;
        }
        else
        {
            online_wait = true;
        }
    }

    public void SendCard(int num, string time)
    {
        if (time == "use")
        {
            photonView.RPC(nameof(RPC_ResCard), RpcTarget.Others, num);
        }
        if (time == "move")
        {
            photonView.RPC(nameof(RPC_ResMove), RpcTarget.Others, num);
        }
        if (time == "select")
        {
            photonView.RPC(nameof(RPC_ResSelect), RpcTarget.Others, num);
        }
    }

    [PunRPC]
    void RPC_ResCard(int num)
    {
        online_num = num;
        enemy_use_flag = true;
    }

    [PunRPC]
    void RPC_ResMove(int num)
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("enemy_temp");
        cards[num].tag = "enemy_battle";
    }

    [PunRPC]
    void RPC_ResSelect(int num)
    {
        online_num = num;
        online_wait = false;
    }

    [PunRPC]
    void RPC_Enemy_Exit()
    {
        StartCoroutine(enemy_exit());
    }

    IEnumerator enemy_exit()
    {
        yield return BattleManager.instance.StartCoroutine(BattleManager.instance.Judge("対戦相手が退出しました"));
        main_canvas.SetActive(false);
        Title.SetActive(true);
        ONLINE = false;
        online_num = 0;
        online_wait = false;
        BattleManager.instance.Restart();
        turn_count = 0;
        turn_flag = 0;
        temp_count = 0;
        flag = "wait";
        wait_matching.gameObject.SetActive(false);
        CardController[] cc = main_canvas.GetComponentsInChildren<CardController>();
        foreach (CardController c in cc)
        {
            Destroy(c.gameObject);
        }
        PhotonNetwork.LeaveRoom();
    }

}


