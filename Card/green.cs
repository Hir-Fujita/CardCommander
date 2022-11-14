using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class green : MonoBehaviour
{

    public static green instance;
    [SerializeField] CardController cardPrefab;
    [SerializeField] public GameObject Player;
    [SerializeField] public Transform player_field;
    [SerializeField] public Transform player_temp;
    [SerializeField] public Text player_spd;

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

    public void green_400(string _user)//緑の煙
    {
        GameObject user = GameObject.Find(_user);
        Player p = user.transform.GetComponent<Player>();
        int count = 1;
        foreach (int i in p.trash)
        {
            if (i == 400)
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



    public void green_415(string _user)//エルフ
    {
        GameObject user = GameObject.Find(_user);
        Player P = user.transform.GetComponent<Player>();
        if (P.mp > 0)
        {
            P.mp = P.mp - 1;
            P.mp_reg = P.mp_reg + 1;
        }
    }


    public IEnumerator green_427(string _user)//植物使い
    {
        if (_user == "Player")
        {
            Player P = Player.GetComponent<Player>();
            if (P.mp > 0)
            {
                P.mp = P.mp - 1;
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                if (p_field_count.Length != 0)
                {
                    GameObject pp = player_field.GetChild(0).gameObject;
                    GameManager.flag = "field";
                    Card_process p = pp.GetComponent<Card_process>();
                    p.StartCoroutine(p.Wait_flag());
                }
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
                if (e_field_count.Length != 0)
                {
                    GameObject ee = enemy_field.GetChild(0).gameObject;
                    GameManager.flag = "field";
                    Card_process e = ee.GetComponent<Card_process>();
                    e.StartCoroutine(e.Wait_flag());
                }
                yield return new WaitUntil(() => GameManager.flag == "wait");
                if (GameManager.obj != null)
                {
                    if (GameManager.obj.transform.parent.name == "p1_field")
                    {
                        Card_process c = GameManager.obj.GetComponent<Card_process>();
                        yield return c.StartCoroutine(c.Uses());
                        Player p = Player.GetComponent<Player>();
                        int count = 0;
                        foreach (int i in p.hand)
                        {
                            if (400 <= i)
                            {
                                count = count + 1;
                            }
                        }
                        int hp = int.Parse(BattleManager.instance.hp_var.text);
                        BattleManager.instance.hp_var.text = (hp + count).ToString();
                    }
                    else if (GameManager.obj.transform.parent.name == "p2_field")
                    {
                        Card_process c = GameManager.obj.GetComponent<Card_process>();
                        yield return c.StartCoroutine(c.Uses());
                        Player p = Player.GetComponent<Player>();
                        int count = 0;
                        foreach (int i in p.hand)
                        {
                            if (400 <= i)
                            {
                                count = count + 1;
                            }
                        }
                        int hp = int.Parse(BattleManager.instance.enemy_hp_var.text);
                        BattleManager.instance.enemy_hp_var.text = (hp + count).ToString();
                    }
                }
            }
        }
        else
        {
            Player E = Enemy.GetComponent<Player>();
            if (E.mp > 0)
            {
                if (GameManager.ONLINE)
                {
                    CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                    CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
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
                            Card_process p = pp.GetComponent<Card_process>();
                            yield return p.StartCoroutine(p.Uses());
                            Player _p = Enemy.GetComponent<Player>();
                            int count = 0;
                            foreach (int i in _p.hand)
                            {
                                if (400 <= i)
                                {
                                    count = count + 1;
                                }
                            }
                            int hp = int.Parse(BattleManager.instance.hp_var.text);
                            BattleManager.instance.hp_var.text = (hp + count).ToString();
                        }
                        else if (GameManager.online_num == 1)
                        {
                            GameObject ee = enemy_field.GetChild(0).gameObject;
                            Card_process e = ee.GetComponent<Card_process>();
                            yield return e.StartCoroutine(e.Uses());
                            Player p = Enemy.GetComponent<Player>();
                            int count = 0;
                            foreach (int i in p.hand)
                            {
                                if (400 <= i)
                                {
                                    count = count + 1;
                                }
                            }
                            int hp = int.Parse(BattleManager.instance.enemy_hp_var.text);
                            BattleManager.instance.enemy_hp_var.text = (hp + count).ToString();
                        }
                    }
                }
                else
                {
                    CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                    CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
                    if (e_field_count.Length != 0)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        yield return e.StartCoroutine(e.Uses());
                        Player p = Enemy.GetComponent<Player>();
                        int count = 0;
                        foreach (int i in p.hand)
                        {
                            if (400 <= i)
                            {
                                count = count + 1;
                            }
                        }
                        int hp = int.Parse(BattleManager.instance.enemy_hp_var.text);
                        BattleManager.instance.enemy_hp_var.text = (hp + count).ToString();
                    }
                    else if (p_field_count.Length != 0)
                    {
                        GameObject pp = player_field.GetChild(0).gameObject;
                        Card_process p = pp.GetComponent<Card_process>();
                        yield return p.StartCoroutine(p.Uses());
                        Player _p = Enemy.GetComponent<Player>();
                        int count = 0;
                        foreach (int i in _p.hand)
                        {
                            if (400 <= i)
                            {
                                count = count + 1;
                            }
                        }
                        int hp = int.Parse(BattleManager.instance.hp_var.text);
                        BattleManager.instance.hp_var.text = (hp + count).ToString();
                    }
                }
            }
        }
    }


    public IEnumerator green_428(string _user)//狩人
    {
        if (_user == "Player")
        {
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            if (p_field_count.Length != 0)
            {
                GameObject pp = player_field.GetChild(0).gameObject;
                GameManager.flag = "field";
                Card_process p = pp.GetComponent<Card_process>();
                p.StartCoroutine(p.Wait_flag());
            }
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (e_field_count.Length != 0)
            {
                GameObject ee = enemy_field.GetChild(0).gameObject;
                GameManager.flag = "field";
                Card_process e = ee.GetComponent<Card_process>();
                e.StartCoroutine(e.Wait_flag());
            }
            yield return new WaitUntil(() => GameManager.flag == "wait");
            if (GameManager.obj != null)
            {
                if (GameManager.obj.transform.parent.name == "p1_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    int temp_atk = int.Parse(BattleManager.instance.atk_var.text);
                    int temp_lv = int.Parse(BattleManager.instance.lv_var.text);
                    int atk = temp_atk - 2;
                    if (atk < 0)
                    {
                        atk = 0;
                    }
                    int lv = temp_lv - 2;
                    if (lv <= 0)
                    {
                        lv = 0;
                        Player p = Player.GetComponent<Player>();
                        p.mp = p.mp + 2;
                        p.Stat_update();
                    }
                    BattleManager.instance.atk_var.text = atk.ToString();
                    BattleManager.instance.lv_var.text = lv.ToString();
                }
                else if (GameManager.obj.transform.parent.name == "p2_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    int temp_atk = int.Parse(BattleManager.instance.enemy_atk_var.text);
                    int temp_lv = int.Parse(BattleManager.instance.enemy_lv_var.text);
                    int atk = temp_atk - 2;
                    if (atk < 0)
                    {
                        atk = 0;
                    }
                    int lv = temp_lv - 2;
                    if (lv <= 0)
                    {
                        lv = 0;
                        Player p = Player.GetComponent<Player>();
                        p.mp = p.mp + 2;
                        p.Stat_update();
                    }
                    BattleManager.instance.enemy_atk_var.text = atk.ToString();
                    BattleManager.instance.enemy_lv_var.text = lv.ToString();
                }
            }
        }
        else
        {
            if (GameManager.ONLINE)
            {
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
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
                        Card_process p = pp.GetComponent<Card_process>();
                        CardController cc = pp.GetComponent<CardController>();
                        yield return p.StartCoroutine(p.Uses());
                        int temp_atk = int.Parse(BattleManager.instance.atk_var.text);
                        int temp_lv = int.Parse(BattleManager.instance.lv_var.text);
                        int atk = temp_atk - 2;
                        if (atk < 0)
                        {
                            atk = 0;
                        }
                        int lv = temp_lv - 2;
                        if (lv <= 0)
                        {
                            lv = 0;
                            Player P = Enemy.GetComponent<Player>();
                            P.mp = P.mp + 2;
                            P.Stat_update();
                        }
                        BattleManager.instance.atk_var.text = atk.ToString();
                        BattleManager.instance.lv_var.text = lv.ToString();
                    }
                    else if (GameManager.online_num == 1)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        CardController cc = ee.GetComponent<CardController>();
                        yield return e.StartCoroutine(e.Uses());
                        int temp_atk = int.Parse(BattleManager.instance.enemy_atk_var.text);
                        int temp_lv = int.Parse(BattleManager.instance.enemy_lv_var.text);
                        int atk = temp_atk - 2;
                        if (atk < 0)
                        {
                            atk = 0;
                        }
                        int lv = temp_lv - 2;
                        if (lv <= 0)
                        {
                            lv = 0;
                            Player p = Enemy.GetComponent<Player>();
                            p.mp = p.mp + 2;
                            p.Stat_update();
                        }
                        BattleManager.instance.enemy_atk_var.text = atk.ToString();
                        BattleManager.instance.enemy_lv_var.text = lv.ToString();
                    }
                }
            }
            else
            {
                //CPUの処理
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
                if (p_field_count.Length != 0)
                {
                    GameObject pp = player_field.GetChild(0).gameObject;
                    Card_process p = pp.GetComponent<Card_process>();
                    CardController cc = pp.GetComponent<CardController>();
                    yield return p.StartCoroutine(p.Uses());
                    int temp_atk = int.Parse(BattleManager.instance.atk_var.text);
                    int temp_lv = int.Parse(BattleManager.instance.lv_var.text);
                    int atk = temp_atk - 2;
                    if (atk < 0)
                    {
                        atk = 0;
                    }
                    int lv = temp_lv - 2;
                    if (lv <= 0)
                    {
                        lv = 0;
                        Player P = Enemy.GetComponent<Player>();
                        P.mp = P.mp + 2;
                        P.Stat_update();
                    }
                    BattleManager.instance.atk_var.text = atk.ToString();
                    BattleManager.instance.lv_var.text = lv.ToString();
                }
                else if (e_field_count.Length != 0)
                {
                    GameObject ee = enemy_field.GetChild(0).gameObject;
                    Card_process e = ee.GetComponent<Card_process>();
                    CardController cc = ee.GetComponent<CardController>();
                    yield return e.StartCoroutine(e.Uses());
                    int temp_atk = int.Parse(BattleManager.instance.enemy_atk_var.text);
                    int temp_lv = int.Parse(BattleManager.instance.enemy_lv_var.text);
                    int atk = temp_atk - 2;
                    if (atk < 0)
                    {
                        atk = 0;
                    }
                    int lv = temp_lv - 2;
                    if (lv <= 0)
                    {
                        lv = 0;
                        Player p = Enemy.GetComponent<Player>();
                        p.mp = p.mp + 2;
                        p.Stat_update();
                    }
                    BattleManager.instance.enemy_atk_var.text = atk.ToString();
                    BattleManager.instance.enemy_lv_var.text = lv.ToString();
                }
            }
        }
    }


    public IEnumerator green_443(string _user)//サイクロプス
    {
        if (_user == "Player")
        {
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            if (p_field_count.Length != 0)
            {
                GameObject pp = player_field.GetChild(0).gameObject;
                GameManager.flag = "field";
                Card_process p = pp.GetComponent<Card_process>();
                p.StartCoroutine(p.Wait_flag());
            }
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (e_field_count.Length != 0)
            {
                GameObject ee = enemy_field.GetChild(0).gameObject;
                GameManager.flag = "field";
                Card_process e = ee.GetComponent<Card_process>();
                e.StartCoroutine(e.Wait_flag());
            }
            yield return new WaitUntil(() => GameManager.flag == "wait");
            if (GameManager.obj != null)
            {
                if (GameManager.obj.transform.parent.name == "p1_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    foreach (Text tt in BattleManager.instance.player_abi)
                    {
                        if (tt.text == "")
                        {
                            tt.text = "半減";
                            break;
                        }
                    }
                }
                else if (GameManager.obj.transform.parent.name == "p2_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    foreach (Text tt in BattleManager.instance.enemy_abi)
                    {
                        if (tt.text == "")
                        {
                            tt.text = "半減";
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (GameManager.ONLINE)
            {
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
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
                        Card_process p = pp.GetComponent<Card_process>();
                        CardController cc = pp.GetComponent<CardController>();
                        yield return p.StartCoroutine(p.Uses());
                        foreach (Text tt in BattleManager.instance.enemy_abi)
                        {
                            if (tt.text == "")
                            {
                                tt.text = "半減";
                                break;
                            }
                        }
                    }
                    else if (GameManager.online_num == 1)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        CardController cc = ee.GetComponent<CardController>();
                        yield return e.StartCoroutine(e.Uses());
                        foreach (Text tt in BattleManager.instance.enemy_abi)
                        {
                            if (tt.text == "")
                            {
                                tt.text = "半減";
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                //CPUの処理
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
                if (e_field_count.Length != 0)
                {
                    GameObject ee = enemy_field.GetChild(0).gameObject;
                    Card_process e = ee.GetComponent<Card_process>();
                    CardController cc = ee.GetComponent<CardController>();
                    yield return e.StartCoroutine(e.Uses());
                    foreach (Text tt in BattleManager.instance.enemy_abi)
                    {
                        if (tt.text == "")
                        {
                            tt.text = "半減";
                            break;
                        }
                    }
                }
                else if (p_field_count.Length != 0)
                {
                    GameObject pp = player_field.GetChild(0).gameObject;
                    Card_process p = pp.GetComponent<Card_process>();
                    CardController cc = pp.GetComponent<CardController>();
                    yield return p.StartCoroutine(p.Uses());
                    foreach (Text tt in BattleManager.instance.enemy_abi)
                    {
                        if (tt.text == "")
                        {
                            tt.text = "半減";
                            break;
                        }
                    }
                }
            }
        }
    }


    public void green_439(string _user)//人面樹
    {
        if (_user == "Player")
        {
            CardController[] e_temp_count = enemy_temp.GetComponentsInChildren<CardController>();
            int count = 3 - e_temp_count.Length;
            for (int i = 0; i < count; i++)
            {
                CardController card = Instantiate(cardPrefab, enemy_temp);
                card.Init(414);
                card.name = card.model.name;
                card.tag = "enemy_temp";
                Vector3 scale = card.gameObject.transform.localScale;
                scale.x = -1;
                card.gameObject.transform.localScale = scale;
            }
        }
        else
        {
            CardController[] p_temp_count = player_temp.GetComponentsInChildren<CardController>();
            int count = 3 - p_temp_count.Length;
            for (int i = 0; i < count; i++)
            {
                CardController card = Instantiate(cardPrefab, player_temp);
                card.Init(414);
                card.name = card.model.name;
            }
        }
    }

    public IEnumerator green_446(string _user)//緑ドラゴン
    {
        CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
        if (p_field_count.Length != 0)
        {
            GameObject pp = player_field.GetChild(0).gameObject;
            Card_process p = pp.GetComponent<Card_process>();
            if (player_spd.text == "先行")
            {
                p.StartCoroutine(p.Uses());
                BattleManager.instance.hp_var.text = "0";
            }
        }
        CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
        if (e_field_count.Length != 0)
        {
            GameObject ee = enemy_field.GetChild(0).gameObject;
            Card_process e = ee.GetComponent<Card_process>();
            if (enemy_spd.text == "先行")
            {
                yield return e.StartCoroutine(e.Uses());
                BattleManager.instance.enemy_hp_var.text = "0";
            }
        }
    }


}


