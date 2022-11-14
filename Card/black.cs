using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class black : MonoBehaviour
{

    public static black instance;
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

    public void black_300(string _user)//黒い煙
    {
        GameObject user = GameObject.Find(_user);
        Player p = user.transform.GetComponent<Player>();
        int count = 1;
        foreach (int i in p.trash)
        {
            if (i == 300)
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

    public IEnumerator black_314(string _user)//ミミック
    {
        if (_user == "Player")
        {
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            if (p_field_count.Length != 0)
            {
                bool flag = true;
                foreach (Text tt in BattleManager.instance.player_abi)
                {
                    if (tt.text == "無効" || tt.text == "一時無効")
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    GameObject pp = player_field.GetChild(0).gameObject;
                    GameManager.flag = "field";
                    Card_process p = pp.GetComponent<Card_process>();
                    p.StartCoroutine(p.Wait_flag());
                }
            }
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (e_field_count.Length != 0)
            {
                bool flag = true;
                foreach (Text tt in BattleManager.instance.enemy_abi)
                {
                    if (tt.text == "無効" || tt.text == "一時無効")
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    GameObject ee = enemy_field.GetChild(0).gameObject;
                    GameManager.flag = "field";
                    Card_process e = ee.GetComponent<Card_process>();
                    e.StartCoroutine(e.Wait_flag());
                }
            }
            yield return new WaitUntil(() => GameManager.flag == "wait");
            if (GameManager.obj != null)
            {
                Card_process c = GameManager.obj.GetComponent<Card_process>();
                CardController cc = GameManager.obj.GetComponent<CardController>();
                yield return c.StartCoroutine(c.Uses());
                int id = cc.model.cardID;
                Player p = Player.GetComponent<Player>();
                GameObject use = p.transform.GetChild(0).gameObject;//子要素の0番目(use)を取得
                GameObject cards = use.transform.GetChild(0).gameObject;//useの子要素の0番目(使ったカード)を取得
                CardController card = Instantiate(cardPrefab, use.transform);
                cards.tag = "destroy";
                card.Init(id);
                card.name = card.model.name;
                card.tag = "use";
                yield return new WaitForSeconds(0.1f);
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
                        GameObject pp = enemy_field.GetChild(0).gameObject;
                        Card_process p = pp.GetComponent<Card_process>();
                        CardController c = pp.GetComponent<CardController>();
                        yield return p.StartCoroutine(p.Uses());
                        int id = c.model.cardID;
                        Player E = Player.GetComponent<Player>();
                        GameObject use = E.transform.GetChild(0).gameObject;//子要素の0番目(use)を取得
                        GameObject cards = use.transform.GetChild(0).gameObject;//useの子要素の0番目(使ったカード)を取得
                        CardController card = Instantiate(cardPrefab, use.transform);
                        cards.tag = "destroy";
                        card.Init(id);
                        card.name = card.model.name;
                        card.tag = "enemy";
                        Vector3 scale = card.gameObject.transform.localScale;
                        scale.x = -1;
                        card.gameObject.transform.localScale = scale;
                        yield return new WaitForSeconds(0.1f);
                    }
                    else if (GameManager.online_num == 1)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        CardController cc = ee.GetComponent<CardController>();
                        yield return e.StartCoroutine(e.Uses());
                        int id = cc.model.cardID;
                        Player E = Enemy.GetComponent<Player>();
                        GameObject use = E.transform.GetChild(0).gameObject;//子要素の0番目(use)を取得
                        GameObject cards = use.transform.GetChild(0).gameObject;//useの子要素の0番目(使ったカード)を取得
                        CardController card = Instantiate(cardPrefab, use.transform);
                        cards.tag = "destroy";
                        card.Init(id);
                        card.name = card.model.name;
                        card.tag = "enemy";
                        Vector3 scale = card.gameObject.transform.localScale;
                        scale.x = -1;
                        card.gameObject.transform.localScale = scale;
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
            else
            {
                //CPUの処理
                //ATKとHPの合計値が高い方のカードをコピーするようにしたかったけどめんどくさいから中断
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
                // int stat = int.Parse(BattleManager.instance.atk_var.text) + int.Parse(BattleManager.instance.hp_var.text);
                // int enemy_stat = int.Parse(BattleManager.instance.enemy_atk_var.text) + int.Parse(BattleManager.instance.enemy_hp_var.text);
                if (e_field_count.Length != 0)
                {
                    bool flag = true;
                    foreach (Text tt in BattleManager.instance.enemy_abi)
                    {
                        if (tt.text == "無効" || tt.text == "一時無効")
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        GameObject ee = enemy_field.GetChild(0).gameObject;
                        Card_process e = ee.GetComponent<Card_process>();
                        CardController cc = ee.GetComponent<CardController>();
                        yield return e.StartCoroutine(e.Uses());
                        int id = cc.model.cardID;
                        Player E = Enemy.GetComponent<Player>();
                        GameObject use = E.transform.GetChild(0).gameObject;//子要素の0番目(use)を取得
                        GameObject cards = use.transform.GetChild(0).gameObject;//useの子要素の0番目(使ったカード)を取得
                        CardController card = Instantiate(cardPrefab, use.transform);
                        cards.tag = "destroy";
                        card.Init(id);
                        card.name = card.model.name;
                        card.tag = "enemy";
                        Vector3 scale = card.gameObject.transform.localScale;
                        scale.x = -1;
                        card.gameObject.transform.localScale = scale;
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else if (p_field_count.Length != 0)
                {
                    bool flag = true;
                    foreach (Text tt in BattleManager.instance.enemy_abi)
                    {
                        if (tt.text == "無効" || tt.text == "一時無効")
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        GameObject pp = enemy_field.GetChild(0).gameObject;
                        Card_process p = pp.GetComponent<Card_process>();
                        CardController c = pp.GetComponent<CardController>();
                        yield return p.StartCoroutine(p.Uses());
                        int id = c.model.cardID;
                        Player E = Player.GetComponent<Player>();
                        GameObject use = E.transform.GetChild(0).gameObject;//子要素の0番目(use)を取得
                        GameObject cards = use.transform.GetChild(0).gameObject;//useの子要素の0番目(使ったカード)を取得
                        CardController card = Instantiate(cardPrefab, use.transform);
                        cards.tag = "destroy";
                        card.Init(id);
                        card.name = card.model.name;
                        card.tag = "enemy";
                        Vector3 scale = card.gameObject.transform.localScale;
                        scale.x = -1;
                        card.gameObject.transform.localScale = scale;
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }
    }

    public void black_329(string _user)//ネクロマンサー
    {
        if (_user == "Player")
        {
            Player P = Player.GetComponent<Player>();
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            if (P.trash.Count != 0 && p_field_count.Length == 0)
            {
                int num = P.trash[P.trash.Count -1];
                P.trash.RemoveAt(P.trash.Count -1);
                CardController card = Instantiate(cardPrefab, player_field);
                card.Init(num);
                card.name = card.model.name;
                card.tag = "Untagged";
                BattleManager.instance.battle_data("player",card);
            }
        }
        else
        {
            Player E = Enemy.GetComponent<Player>();
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (E.trash.Count != 0 && e_field_count.Length == 0)
            {
                int num = E.trash[E.trash.Count -1];
                E.trash.RemoveAt(E.trash.Count -1);
                CardController card = Instantiate(cardPrefab, enemy_field);
                card.Init(num);
                card.name = card.model.name;
                card.tag = "Untagged";
                BattleManager.instance.battle_data("enemy",card);
            }
        }
    }

    public IEnumerator black_340(string _user)//死神
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
                    BattleManager.instance.hp_var.text = "0";
                }
                else if (GameManager.obj.transform.parent.name == "p2_field")
                {
                    Card_process c = GameManager.obj.GetComponent<Card_process>();
                    yield return c.StartCoroutine(c.Uses());
                    BattleManager.instance.enemy_hp_var.text = "0";
                }
                Player p = Player.GetComponent<Player>();
                GameObject card = p.transform.GetChild(4).gameObject;//子要素の5番目(hand)を取得
                Card_process[] cards = card.transform.GetComponentsInChildren<Card_process>();
                CardController[] cc = card.transform.GetComponentsInChildren<CardController>();
                foreach (Card_process c in cards)
                {
                    if (c == cards[cards.Length - 1])
                    {
                        yield return c.StartCoroutine(c.DestroyCard());
                    }
                    else
                    {
                        c.StartCoroutine(c.DestroyCard());
                    }
                }
                foreach (CardController c in cc)
                {
                    p.trash.Add(c.model.cardID);
                    Destroy(c.gameObject);
                }
                p.hand.Clear();
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
                        BattleManager.instance.hp_var.text = "0";
                    }
                    else if (GameManager.online_num == 1)
                    {
                        BattleManager.instance.enemy_hp_var.text = "0";
                    }
                    Player E = Enemy.GetComponent<Player>();
                    E.hand.Clear();
                }
            }
            else
            {
                Player E = Enemy.GetComponent<Player>();
                CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
                CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
                if (p_field_count.Length != 0)
                {
                    BattleManager.instance.hp_var.text = "0";
                }
                else if (e_field_count.Length != 0)
                {
                    BattleManager.instance.enemy_hp_var.text = "0";
                }
                E.hand.Clear();
            }
        }
    }

    public void black_343(string _user)//黒ドラゴン
    {
        if (_user == "Player")
        {
            CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
            if (p_field_count.Length != 0)
            {
                BattleManager.instance.hp_var.text = "0";
            }
        }
        else
        {
            CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
            if (e_field_count.Length != 0)
            {
                BattleManager.instance.enemy_hp_var.text = "0";
            }
        }
    }

}