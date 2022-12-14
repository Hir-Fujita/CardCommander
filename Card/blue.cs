using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blue : MonoBehaviour
{

    public static blue instance;
    [SerializeField] CardController cardPrefab;
    [SerializeField] GameObject temp_window;

    [SerializeField] public GameObject Player;
    [SerializeField] public Transform player_field;
    [SerializeField] public Transform player_temp;
    [SerializeField] public Text player_spd;
    [SerializeField] Transform Player_hand;

    [SerializeField] public GameObject Enemy;
    [SerializeField] public Transform enemy_field;
    [SerializeField] public Transform enemy_temp;
    [SerializeField] public Text enemy_spd;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void blue_200(string _user) //青い煙
    {
        GameObject user = GameObject.Find(_user);
        Player p = user.transform.GetComponent<Player>();
        int count = 1;
        foreach (int i in p.trash)
        {
            if (i == 200)
            {
                count = count + 1;
            }
        }
        p.mp = p.mp + count;
        p.Stat_update();
        GameObject card = p.transform.GetChild(0).gameObject;//子要素の0番目(use)を取得
        GameObject cards = card.transform.GetChild(0).gameObject;//useの子要素の0番目(使ったカード)を取得
        cards.tag = "destroy";
    }

    public IEnumerator blue_203(string _user)//タコ
    {
        if (_user == "Player")
        {
            Player P = Player.GetComponent<Player>();
            int[] cards = { P.deck[0], P.deck[1], P.deck[2], P.deck[3] };
            temp_window.SetActive(true);
            GameManager.flag = "select";
            GameObject panel = temp_window.transform.GetChild(0).gameObject;
            int number = 0;
            foreach (int i in cards)
            {
                CardController card = Instantiate(cardPrefab, panel.transform);
                card.Init(i);
                card.name = number.ToString();
                Card_process c_process = card.gameObject.GetComponent<Card_process>();
                c_process.StartCoroutine(c_process.Wait_flag());
                number = number + 1;
            }
            yield return new WaitUntil(() => GameManager.flag == "wait");
            if (GameManager.obj != null)
            {
                CardController cc = GameManager.obj.GetComponent<CardController>();
                P.Add_hand(cc.model.cardID);
                int num = int.Parse(GameManager.obj.name);
                GameManager.obj.name = cc.model.name;
                P.deck.RemoveAt(num);
            }
            temp_window.SetActive(false);
            CardController[] temp_window_count = temp_window.GetComponentsInChildren<CardController>();
            foreach (CardController c in temp_window_count)
            {
                Destroy(c.gameObject);
            }
        }
        else
        {
            Player E = Enemy.GetComponent<Player>();
            if (GameManager.ONLINE)
            {

                GameManager.online_wait = true;
                while (GameManager.online_wait)
                {
                    yield return null;
                }
                E.hand.Add(GameManager.online_num);
                E.deck.Remove(GameManager.online_num);
            }
            else
            {
                E.hand.Add(E.deck[0]);
                E.deck.RemoveAt(0);
            }
        }
    }


    public IEnumerator blue_219(string _user)//ウミガメ
    {
        Player P = Player.GetComponent<Player>();
        Player E = Enemy.GetComponent<Player>();
        if (_user == "Player")
        {
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (p_field_count.Length != 0 || e_field_count.Length != 0)
            {
                bool flag = false;
                if (p_field_count.Length != 0)
                {
                    int p_lv = int.Parse(BattleManager.instance.lv_var.text);
                    if (p_lv < 2)
                    {
                        GameObject pp = player_field.GetChild(0).gameObject;
                        Card_process p = pp.GetComponent<Card_process>();
                        GameManager.flag = "field_blue_219";
                        p.StartCoroutine(p.Wait_flag());
                        flag = true;
                    }
                }
                if (e_field_count.Length != 0)
                {
                    int e_lv = int.Parse(BattleManager.instance.enemy_lv_var.text);
                    if (e_lv < 2)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        GameManager.flag = "field_blue_219";
                        e.StartCoroutine(e.Wait_flag());
                        flag = true;
                    }
                }
                if (flag)
                {
                    yield return new WaitUntil(() => GameManager.flag == "wait");
                }
            }
            if (GameManager.obj != null)
            {
                if (GameManager.obj.transform.parent.name == "p1_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    P.hand.Add(cc.model.cardID);
                    GameManager.obj.transform.SetParent(Player_hand);
                    BattleManager.instance.field_reset("player");
                    P.Stat_update();
                }
                else if (GameManager.obj.transform.parent.name == "p2_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    E.hand.Add(cc.model.cardID);
                    Destroy(cc.gameObject);
                    BattleManager.instance.field_reset("enemy");
                    E.Stat_update();
                }
            }
        }
        else
        {
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (p_field_count.Length != 0 || e_field_count.Length != 0)
            {
                bool flag = false;
                if (p_field_count.Length != 0)
                {
                    int p_lv = int.Parse(BattleManager.instance.lv_var.text);
                    if (p_lv < 2)
                    {
                        flag = true;
                    }
                }
                if (e_field_count.Length != 0)
                {
                    int e_lv = int.Parse(BattleManager.instance.enemy_lv_var.text);
                    if (e_lv < 2)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    if (GameManager.ONLINE)
                    {
                        if (p_field_count.Length != 0 || e_field_count.Length != 0)
                        {
                            GameManager.online_wait = true;
                            while (GameManager.online_wait)
                            {
                                yield return null;
                            }
                            if (GameManager.online_num == 0)
                            {
                                GameObject pp = player_field.GetChild(0).gameObject;
                                Card_process c = pp.GetComponent<Card_process>();
                                CardController cc = pp.GetComponent<CardController>();
                                yield return c.StartCoroutine(c.Uses());
                                P.hand.Add(cc.model.cardID);
                                pp.transform.SetParent(Player_hand);
                                BattleManager.instance.field_reset("player");
                                P.Stat_update();
                            }
                            else if (GameManager.online_num == 1)
                            {
                                GameObject ee = enemy_field.GetChild(0).gameObject;
                                Card_process c = ee.GetComponent<Card_process>();
                                CardController cc = ee.GetComponent<CardController>();
                                yield return c.StartCoroutine(c.Uses());
                                E.hand.Add(cc.model.cardID);
                                Destroy(cc.gameObject);
                                BattleManager.instance.field_reset("enemy");
                                E.Stat_update();
                            }
                        }
                    }
                    else
                    {
                        if (p_field_count.Length != 0)
                        {
                            int p_lv = int.Parse(BattleManager.instance.lv_var.text);
                            if (p_lv < 2)
                            {
                                GameObject pp = player_field.GetChild(0).gameObject;
                                Card_process c = pp.GetComponent<Card_process>();
                                CardController cc = pp.GetComponent<CardController>();
                                yield return c.StartCoroutine(c.Uses());
                                P.hand.Add(cc.model.cardID);
                                pp.transform.SetParent(Player_hand);
                                BattleManager.instance.field_reset("player");
                                P.Stat_update();
                            }
                        }
                        else if (e_field_count.Length != 0)
                        {
                            int e_lv = int.Parse(BattleManager.instance.enemy_lv_var.text);
                            if (e_lv < 2)
                            {
                                GameObject ee = enemy_field.GetChild(0).gameObject;
                                Card_process c = ee.GetComponent<Card_process>();
                                CardController cc = ee.GetComponent<CardController>();
                                yield return c.StartCoroutine(c.Uses());
                                E.hand.Add(cc.model.cardID);
                                Destroy(cc.gameObject);
                                BattleManager.instance.field_reset("enemy");
                                E.Stat_update();
                            }
                        }
                    }
                }
            }
        }
    }

    public IEnumerator blue_227(string _user)//人魚マダム
    {
        Player P = Player.GetComponent<Player>();
        Player E = Enemy.GetComponent<Player>();
        if (_user == "Player" && P.mp > 0)
        {
            P.mp = P.mp - 1;
            Card_process[] e_temp_count = enemy_temp.GetComponentsInChildren<Card_process>();
            Card_process[] e_field_count = enemy_field.GetComponentsInChildren<Card_process>();
            if (e_temp_count.Length != 0 && e_field_count.Length != 0)
            {
                GameManager.flag = "enemy_temp";
                foreach (Card_process i in e_temp_count)
                {
                    i.StartCoroutine(i.Wait_flag());
                }
                yield return new WaitUntil(() => GameManager.flag == "wait");
                int num = GameManager.obj.transform.GetSiblingIndex();
                GameObject card = enemy_field.GetChild(0).gameObject;
                CardController cc = GameManager.obj.GetComponent<CardController>();
                cc.gameObject.transform.SetParent(enemy_field);
                card.transform.SetParent(enemy_temp);
                card.transform.SetSiblingIndex(num);
                BattleManager.instance.battle_data("enemy", cc);
            }
        }
        else if (_user == "enemy" && E.mp > 0)
        {
            E.mp = E.mp - 1;
            Card_process[] p_temp_count = player_temp.GetComponentsInChildren<Card_process>();
            Card_process[] p_field_count = player_field.GetComponentsInChildren<Card_process>();
            if (p_temp_count.Length != 0 && p_field_count.Length != 0)
            {
                if (GameManager.ONLINE)
                {
                    GameManager.online_wait = true;
                    while (GameManager.online_wait)
                    {
                        yield return null;
                    }
                    GameObject card = player_field.GetChild(0).gameObject;
                    GameObject card2 = player_temp.GetChild(GameManager.online_num).gameObject;
                    card2.transform.SetParent(player_field);
                    card.transform.SetParent(player_temp);
                    card.transform.SetSiblingIndex(GameManager.online_num);
                    CardController cc = card2.GetComponent<CardController>();
                    BattleManager.instance.battle_data("player", cc);
                }
                else
                {
                    GameObject card = player_field.GetChild(0).gameObject;
                    GameObject card2 = player_temp.GetChild(0).gameObject;
                    card2.transform.SetParent(player_field);
                    card.transform.SetParent(player_temp);
                    card.transform.SetSiblingIndex(0);
                    CardController cc = card2.GetComponent<CardController>();
                    BattleManager.instance.battle_data("player", cc);
                }
            }
        }
    }

    public void blue_229(string _user)//河童
    {
        BattleManager.instance.Initiative_move("move");
    }

    public IEnumerator blue_245(string _user)//ヤマタノオロチ
    {
        Player P = Player.GetComponent<Player>();
        Player E = Enemy.GetComponent<Player>();
        Card_process[] p_temp_count = player_temp.GetComponentsInChildren<Card_process>();
        Card_process[] e_temp_count = enemy_temp.GetComponentsInChildren<Card_process>();
        if (p_temp_count.Length != 0 || e_temp_count.Length != 0)
        {
            if (_user == "Player")
            {
                GameManager.flag = "all_temp";
                foreach (Card_process i in p_temp_count)
                {
                    i.StartCoroutine(i.Wait_flag());
                }
                foreach (Card_process i in e_temp_count)
                {
                    i.StartCoroutine(i.Wait_flag());
                }
                yield return new WaitUntil(() => GameManager.flag == "wait");
                if (GameManager.obj.transform.parent.name == "p1_temp")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    P.hand.Add(cc.model.cardID);
                    GameManager.obj.transform.SetParent(Player_hand);
                    BattleManager.instance.field_reset("player");
                    P.Stat_update();
                }
                else if (GameManager.obj.transform.parent.name == "p2_temp")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    E.hand.Add(cc.model.cardID);
                    Destroy(cc.gameObject);
                    BattleManager.instance.field_reset("enemy");
                    E.Stat_update();
                }
            }
            else
            {
                if (GameManager.ONLINE)
                {
                    GameManager.online_wait = true;
                    while (GameManager.online_wait)
                    {
                        yield return null;
                    }
                    if (GameManager.online_num < 3)
                    {
                        GameObject card = player_temp.GetChild(GameManager.online_num).gameObject;
                        Card_process c = card.GetComponent<Card_process>();
                        CardController cc = card.GetComponent<CardController>();
                        yield return c.StartCoroutine(c.Uses());
                        P.hand.Add(cc.model.cardID);
                        card.transform.SetParent(Player_hand);
                        BattleManager.instance.field_reset("player");
                        P.Stat_update();
                    }
                    else
                    {
                        int num = GameManager.online_num - 3;
                        GameObject card = enemy_temp.GetChild(num).gameObject;
                        Card_process c = card.GetComponent<Card_process>();
                        CardController cc = card.GetComponent<CardController>();
                        yield return c.StartCoroutine(c.Uses());
                        E.hand.Add(cc.model.cardID);
                        Destroy(card);
                        BattleManager.instance.field_reset("enemy");
                        E.Stat_update();
                    }
                }
                else
                {
                    if (p_temp_count.Length != 0)
                    {
                        GameObject pp = player_temp.GetChild(0).gameObject;
                        Card_process c = pp.GetComponent<Card_process>();
                        CardController cc = pp.GetComponent<CardController>();
                        yield return c.StartCoroutine(c.Uses());
                        P.hand.Add(cc.model.cardID);
                        pp.transform.SetParent(Player_hand);
                        BattleManager.instance.field_reset("player");
                        P.Stat_update();
                    }
                    else if (e_temp_count.Length != 0)
                    {
                        GameObject pp = enemy_temp.GetChild(0).gameObject;
                        Card_process c = pp.GetComponent<Card_process>();
                        CardController cc = pp.GetComponent<CardController>();
                        yield return c.StartCoroutine(c.Uses());
                        E.hand.Add(cc.model.cardID);
                        Destroy(cc.gameObject);
                        BattleManager.instance.field_reset("enemy");
                        E.Stat_update();
                    }
                }
            }
        }
    }

    public IEnumerator blue_250(string _user)//シーサーペント
    {
        Player P = Player.GetComponent<Player>();
        Player E = Enemy.GetComponent<Player>();
        CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
        CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
        if (p_field_count.Length != 0)
        {
            GameObject pp = player_field.GetChild(0).gameObject;
            Card_process c = pp.GetComponent<Card_process>();
            CardController cc = pp.GetComponent<CardController>();
            yield return c.StartCoroutine(c.Uses());
            P.hand.Add(cc.model.cardID);
            pp.transform.SetParent(Player_hand);
            BattleManager.instance.field_reset("player");
            P.Stat_update();
        }
        if (e_field_count.Length != 0)
        {
            GameObject ee = enemy_field.GetChild(0).gameObject;
            Card_process c = ee.GetComponent<Card_process>();
            CardController cc = ee.GetComponent<CardController>();
            yield return c.StartCoroutine(c.Uses());
            E.hand.Add(cc.model.cardID);
            Destroy(cc.gameObject);
            BattleManager.instance.field_reset("enemy");
            E.Stat_update();
        }
    }












}

