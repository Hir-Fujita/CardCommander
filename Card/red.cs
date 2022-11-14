using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class red : MonoBehaviour
{

    public static red instance;
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

    public void red_100(string _user)
    {
        GameObject user = GameObject.Find(_user);
        Player p = user.transform.GetComponent<Player>();
        int count = 1;
        foreach (int i in p.trash)
        {
            if (i == 100)
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

    public IEnumerator red_114(string _user)//ゴブリンメイジ
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
                    string temp_atk = BattleManager.instance.atk_var.text;
                    string temp_hp = BattleManager.instance.hp_var.text;
                    BattleManager.instance.atk_var.text = temp_hp;
                    BattleManager.instance.hp_var.text = temp_atk;
                    if (cc.model.trive == "ゴブリン族")
                    {
                        int i = int.Parse(temp_hp);
                        BattleManager.instance.atk_var.text = (i + 2).ToString();
                    }
                }
                else if (GameManager.obj.transform.parent.name == "p2_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    CardController cc = GameManager.obj.GetComponent<CardController>();
                    yield return c.StartCoroutine(c.Uses());
                    string temp_atk = BattleManager.instance.enemy_atk_var.text;
                    string temp_hp = BattleManager.instance.enemy_hp_var.text;
                    BattleManager.instance.enemy_atk_var.text = temp_hp;
                    BattleManager.instance.enemy_hp_var.text = temp_atk;
                    if (cc.model.trive == "ゴブリン族")
                    {
                        int i = int.Parse(temp_hp);
                        BattleManager.instance.enemy_atk_var.text = (i + 2).ToString();
                    }
                }
            }
        }
        else
        {
            //オンライン時の処理
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
                        string temp_atk = BattleManager.instance.atk_var.text;
                        string temp_hp = BattleManager.instance.hp_var.text;
                        BattleManager.instance.atk_var.text = temp_hp;
                        BattleManager.instance.hp_var.text = temp_atk;
                        if (cc.model.trive == "ゴブリン族")
                        {
                            int i = int.Parse(temp_hp);
                            BattleManager.instance.atk_var.text = (i + 2).ToString();
                        }
                    }
                    else if (GameManager.online_num == 1)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        CardController cc = ee.GetComponent<CardController>();
                        yield return e.StartCoroutine(e.Uses());
                        string temp_atk = BattleManager.instance.enemy_atk_var.text;
                        string temp_hp = BattleManager.instance.enemy_hp_var.text;
                        BattleManager.instance.enemy_atk_var.text = temp_hp;
                        BattleManager.instance.enemy_hp_var.text = temp_atk;
                        if (cc.model.trive == "ゴブリン族")
                        {
                            int i = int.Parse(temp_hp);
                            BattleManager.instance.enemy_atk_var.text = (i + 2).ToString();
                        }
                    }
                    GameManager.online_num = 0;
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
                    string temp_atk = BattleManager.instance.enemy_atk_var.text;
                    string temp_hp = BattleManager.instance.enemy_hp_var.text;
                    BattleManager.instance.enemy_atk_var.text = temp_hp;
                    BattleManager.instance.enemy_hp_var.text = temp_atk;
                    if (cc.model.trive == "ゴブリン族")
                    {
                        int i = int.Parse(temp_hp);
                        BattleManager.instance.enemy_atk_var.text = (i + 2).ToString();
                    }
                }
                else if (p_field_count.Length != 0)
                {
                    GameObject pp = player_field.GetChild(0).gameObject;
                    Card_process p = pp.GetComponent<Card_process>();
                    CardController cc = pp.GetComponent<CardController>();
                    yield return p.StartCoroutine(p.Uses());
                    string temp_atk = BattleManager.instance.atk_var.text;
                    string temp_hp = BattleManager.instance.hp_var.text;
                    BattleManager.instance.atk_var.text = temp_hp;
                    BattleManager.instance.hp_var.text = temp_atk;
                    if (cc.model.trive == "ゴブリン族")
                    {
                        int i = int.Parse(temp_hp);
                        BattleManager.instance.atk_var.text = (i + 2).ToString();
                    }
                }
            }
        }
    }


    public IEnumerator red_117(string _user)//翼竜
    {
        GameObject user = GameObject.Find(_user);
        Player P = user.transform.GetComponent<Player>();
        if (P.mp > 0)
        {
            P.mp = P.mp - 1;
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
                        yield return c.StartCoroutine(c.Uses());
                        player_spd.color = new Color(1, 0, 0, 1);
                        player_spd.text = "先行";
                    }
                    else if (GameManager.obj.transform.parent.name == "p2_field")
                    {
                        Card_process c = GameManager.obj.GetComponent<Card_process>();
                        yield return c.StartCoroutine(c.Uses());
                        enemy_spd.color = new Color(1, 0, 0, 1);
                        enemy_spd.text = "先行";
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
                            yield return p.StartCoroutine(p.Uses());
                            player_spd.color = new Color(1, 0, 0, 1);
                            player_spd.text = "先行";
                        }
                        else if (GameManager.online_num == 1)
                        {
                            GameObject ee = enemy_field.GetChild(0).gameObject;
                            Card_process e = ee.GetComponent<Card_process>();
                            yield return e.StartCoroutine(e.Uses());
                            enemy_spd.color = new Color(1, 0, 0, 1);
                            enemy_spd.text = "先行";
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
                        enemy_spd.color = new Color(1, 0, 0, 1);
                        enemy_spd.text = "先行";
                    }
                    else if (p_field_count.Length != 0)
                    {
                        GameObject pp = player_field.GetChild(0).gameObject;
                        Card_process p = pp.GetComponent<Card_process>();
                        yield return p.StartCoroutine(p.Uses());
                        player_spd.color = new Color(1, 0, 0, 1);
                        player_spd.text = "先行";
                    }
                }
                GameManager.online_num = 0;
            }
        }
    }

    public IEnumerator red_128(string _user)//ゴブリン戦車
    {
        if (_user == "Player")
        {
            Card_process[] p_temp_count = player_temp.GetComponentsInChildren<Card_process>();
            if (p_temp_count.Length != 0)
            {
                GameManager.flag = "temp";
                foreach (Card_process i in p_temp_count)
                {
                    i.StartCoroutine(i.Wait_flag());
                }
                yield return new WaitUntil(() => GameManager.flag == "wait");
                CardController cc = GameManager.obj.GetComponent<CardController>();
                Card_process c = GameManager.obj.GetComponent<Card_process>();
                Player p = Player.GetComponent<Player>();
                yield return c.StartCoroutine(c.DestroyCard());
                p.trash.Add(c.model.cardID);
                Destroy(c.gameObject);
                if (BattleManager.instance.hp_var.text != "")
                {
                    int hp = int.Parse(BattleManager.instance.hp_var.text);
                    hp = hp - (cc.model.atk + 1);
                    if (hp < 0)
                    {
                        hp = 0;
                    }
                    BattleManager.instance.hp_var.text = hp.ToString();
                }
                if (BattleManager.instance.enemy_hp_var.text != "")
                {
                    int hp = int.Parse(BattleManager.instance.enemy_hp_var.text);
                    hp = hp - (cc.model.atk + 1);
                    if (hp < 0)
                    {
                        hp = 0;
                    }
                    BattleManager.instance.enemy_hp_var.text = hp.ToString();
                }
            }
        }
        else
        {
            if (GameManager.ONLINE)
            {
                Card_process[] e_temp_count = enemy_temp.GetComponentsInChildren<Card_process>();
                if (e_temp_count.Length != 0)
                {
                    GameManager.online_wait = true;
                    while (GameManager.online_wait)
                    {
                        yield return null;
                    }
                    Card_process cp = e_temp_count[GameManager.online_num];
                    CardController cc = cp.gameObject.GetComponent<CardController>();
                    Player e = Enemy.GetComponent<Player>();
                    yield return cp.StartCoroutine(cp.DestroyCard());
                    e.trash.Add(cp.model.cardID);
                    Destroy(cp.gameObject);
                    if (BattleManager.instance.hp_var.text != "")
                    {
                        int hp = int.Parse(BattleManager.instance.hp_var.text);
                        hp = hp - (cc.model.atk + 1);
                        if (hp < 0)
                        {
                            hp = 0;
                        }
                        BattleManager.instance.hp_var.text = hp.ToString();
                    }
                    if (BattleManager.instance.enemy_hp_var.text != "")
                    {
                        int hp = int.Parse(BattleManager.instance.enemy_hp_var.text);
                        hp = hp - (cc.model.atk + 1);
                        if (hp < 0)
                        {
                            hp = 0;
                        }
                        BattleManager.instance.enemy_hp_var.text = hp.ToString();
                    }
                    GameManager.online_num = 0;
                }
            }
            else
            {
                Card_process[] e_temp_count = enemy_temp.GetComponentsInChildren<Card_process>();
                if (e_temp_count.Length != 0)
                {
                    Card_process cp = e_temp_count[0];
                    CardController cc = cp.gameObject.GetComponent<CardController>();
                    Player e = Enemy.GetComponent<Player>();
                    yield return cp.StartCoroutine(cp.DestroyCard());
                    e.trash.Add(cp.model.cardID);
                    Destroy(cp.gameObject);
                    if (BattleManager.instance.hp_var.text != "")
                    {
                        int hp = int.Parse(BattleManager.instance.hp_var.text);
                        hp = hp - (cc.model.atk + 1);
                        if (hp < 0)
                        {
                            hp = 0;
                        }
                        BattleManager.instance.hp_var.text = hp.ToString();
                    }
                    if (BattleManager.instance.enemy_hp_var.text != "")
                    {
                        int hp = int.Parse(BattleManager.instance.enemy_hp_var.text);
                        hp = hp - (cc.model.atk + 1);
                        if (hp < 0)
                        {
                            hp = 0;
                        }
                        BattleManager.instance.enemy_hp_var.text = hp.ToString();
                    }
                }
            }
        }
    }

    public IEnumerator red_132(string _user)//炎使い
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
                    yield return c.StartCoroutine(c.Uses());
                    Player p = Player.GetComponent<Player>();
                    int count = 0;
                    foreach (int i in p.hand)
                    {
                        if (100 <= i && i < 199)
                        {
                            count = count + 1;
                        }
                    }
                    int atk = int.Parse(BattleManager.instance.atk_var.text);
                    BattleManager.instance.atk_var.text = (atk + count).ToString();
                }
                else if (GameManager.obj.transform.parent.name == "p2_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    yield return c.StartCoroutine(c.Uses());
                    Player p = Player.GetComponent<Player>();
                    int count = 0;
                    foreach (int i in p.hand)
                    {
                        if (100 <= i && i < 199)
                        {
                            count = count + 1;
                        }
                    }
                    int atk = int.Parse(BattleManager.instance.enemy_atk_var.text);
                    BattleManager.instance.enemy_atk_var.text = (atk + count).ToString();
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
                        yield return p.StartCoroutine(p.Uses());
                        Player _p = Enemy.GetComponent<Player>();
                        int count = 0;
                        foreach (int i in _p.hand)
                        {
                            if (100 <= i && i < 199)
                            {
                                count = count + 1;
                            }
                        }
                        int atk = int.Parse(BattleManager.instance.atk_var.text);
                        BattleManager.instance.atk_var.text = (atk + count).ToString();
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
                            if (100 <= i && i < 199)
                            {
                                count = count + 1;
                            }
                        }
                        int atk = int.Parse(BattleManager.instance.enemy_atk_var.text);
                        BattleManager.instance.enemy_atk_var.text = (atk + count).ToString();
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
                        if (100 <= i && i < 199)
                        {
                            count = count + 1;
                        }
                    }
                    int atk = int.Parse(BattleManager.instance.enemy_atk_var.text);
                    BattleManager.instance.enemy_atk_var.text = (atk + count).ToString();
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
                        if (100 <= i && i < 199)
                        {
                            count = count + 1;
                        }
                    }
                    int atk = int.Parse(BattleManager.instance.atk_var.text);
                    BattleManager.instance.atk_var.text = (atk + count).ToString();
                }
            }
            GameManager.online_num = 0;
        }
    }


    public IEnumerator red_147(string _user)//赤ドラゴン
    {
        CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
        if (p_field_count.Length != 0)
        {
            GameObject pp = player_field.GetChild(0).gameObject;
            Card_process p = pp.GetComponent<Card_process>();
            if (player_spd.text == "後攻")
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
            if (enemy_spd.text == "後攻")
            {
                yield return e.StartCoroutine(e.Uses());
                BattleManager.instance.enemy_hp_var.text = "0";
            }
        }
    }
}
