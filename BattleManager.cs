using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 対戦処理を行うクラス カード移動や消滅のアニメーションもココで行う

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    [SerializeField] CardController cardPrefab;

    [SerializeField] GameObject p;
    [SerializeField] GameObject e;
    [SerializeField] Transform hand;
    [SerializeField] Transform player_use;
    [SerializeField] Transform player_temp;
    [SerializeField] Transform player_field;
    [SerializeField] public Text player_spd;
    [SerializeField] public Text[] player_abi;
    [SerializeField] Transform enemy_use;
    [SerializeField] Transform enemy_temp;
    [SerializeField] Transform enemy_field;
    [SerializeField] public Text enemy_spd;
    [SerializeField] public Text[] enemy_abi;
    [SerializeField] Text message;
    [SerializeField] GameObject message_window;

    [SerializeField] public Text atk_var;
    [SerializeField] public Text hp_var;
    [SerializeField] public Text lv_var;
    [SerializeField] public Text enemy_atk_var;
    [SerializeField] public Text enemy_hp_var;
    [SerializeField] public Text enemy_lv_var;

    [SerializeField] public Text player_result_process;
    [SerializeField] public Text enemy_result_process;


    [SerializeField] GameObject player_ini;
    [SerializeField] GameObject enemy_ini;
    public Image img;
    public Image img2;
    public Image img3;
    public Image img4;
    public Image img5;
    public Image img6;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            img.color = new Color(img.color[0], img.color[1], img.color[2], 0);
            img2.color = new Color(img2.color[0], img2.color[1], img2.color[2], 0);
            img3.color = new Color(img3.color[0], img3.color[1], img3.color[2], 0);
            img4.color = new Color(img4.color[0], img4.color[1], img4.color[2], 0);
            img5.color = new Color(img5.color[0], img5.color[1], img5.color[2], 0);
            img6.color = new Color(img6.color[0], img6.color[1], img6.color[2], 0);
            atk_var.text = "";
            hp_var.text = "";
            lv_var.text = "";
            enemy_atk_var.text = "";
            enemy_hp_var.text = "";
            enemy_lv_var.text = "";
        }
    }

    public void Restart()
    {
        img.color = new Color(img.color[0], img.color[1], img.color[2], 0);
        img2.color = new Color(img2.color[0], img2.color[1], img2.color[2], 0);
        img3.color = new Color(img3.color[0], img3.color[1], img3.color[2], 0);
        img4.color = new Color(img4.color[0], img4.color[1], img4.color[2], 0);
        img5.color = new Color(img5.color[0], img5.color[1], img5.color[2], 0);
        img6.color = new Color(img6.color[0], img6.color[1], img6.color[2], 0);
        Player P = p.transform.GetComponent<Player>();
        atk_var.text = "";
        hp_var.text = "";
        lv_var.text = "";
        player_spd.text = "";
        foreach (Text t in player_abi)
        {
            t.text = "";
        }
        P.hand.Clear();
        P.trash.Clear();
        P.deck.Clear();
        Player E = e.transform.GetComponent<Player>();
        enemy_atk_var.text = "";
        enemy_hp_var.text = "";
        enemy_lv_var.text = "";
        enemy_spd.text = "";
        foreach (Text t in enemy_abi)
        {
            t.text = "";
        }
        E.hand.Clear();
        E.trash.Clear();
        E.deck.Clear();
    }

    public void turn_calc()
    {
        if (GameManager.turn_flag == 0)
        {
            GameManager.turn_flag = 1;
        }
        else if (GameManager.turn_flag == 1)
        {
            Wait_click();
            //フィールドに出すモンスターを選んだ処理
        }
        else if (GameManager.turn_flag == 2)
        {
            if (GameManager.ONLINE == false)
            {
                CPU.instance.select_hand();
            }
            //裏面を初期設定に戻す
            Player_ura(true, "view", 0);
            Enemy_ura(true, "view", 0);
            Player_ura(true, "rotate", 0);
            Enemy_ura(true, "rotate", 0);
            //カードの移動、戦場のダメージ計算開始
            StartCoroutine(Battle_system());
        }
    }

    public void Start_turn()
    {
        if (GameManager.ONLINE)
        {
            GameManager.online_wait = true;
        }
        Player P = p.transform.GetComponent<Player>();
        Player E = e.transform.GetComponent<Player>();
        StartCoroutine(Turn_start());//ターン描写のエフェクト
        Initiative_move("");
        GameManager.turn_count = GameManager.turn_count + 1;
        if (GameManager.turn_count > 1)
        {
            P.Draw();
            E.Draw();
            Mana_regenate();
        }
        if (GameManager.turn_flag == 0)
        {
            CardController[] field_count = player_field.GetComponentsInChildren<CardController>();
            if (field_count.Length != 0)
            {
                GameManager.turn_flag = 1;
            }
            CardController[] temp_count = player_temp.GetComponentsInChildren<CardController>();
            if (temp_count.Length == 0)
            {
                GameManager.turn_flag = 1;
            }
            Wait_click();
        }
    }

    void Wait_click()
    {
        Player P = p.transform.GetComponent<Player>();
        if (GameManager.turn_flag == 0)
        {
            Card_process[] children = player_temp.GetComponentsInChildren<Card_process>();
            if (children != null)
            {
                foreach (Card_process obj in children)
                {
                    obj.StartCoroutine(obj.Wait_temp());
                }
            }
        }
        if (GameManager.turn_flag == 1)
        {
            CardController[] children = hand.GetComponentsInChildren<CardController>();
            if (children != null)
            {
                foreach (CardController aaa in children)
                {
                    if (aaa.model.cost <= P.mp)
                    {
                        GameObject obj = aaa.gameObject;
                        Card_process ta = obj.GetComponent<Card_process>();
                        ta.StartCoroutine(ta.Wait_hand());
                    }
                    else
                    {
                        GameObject obj = aaa.gameObject;
                        CardController c = obj.GetComponent<CardController>();
                        Card_process ta = obj.GetComponent<Card_process>();
                        if (c.model.name == "赤ドラゴン")
                        {
                            int count = 0;
                            foreach (int i in P.hand)
                            {
                                if (i == 108)
                                {
                                    count = count + 1;
                                }
                            }
                            if (c.model.cost - count <= P.mp)
                            {
                                ta.StartCoroutine(ta.Wait_hand());
                            }
                        }
                    }
                }
            }
        }
    }

    public void Initiative_move(string lose)
    {
        Player P = p.transform.GetComponent<Player>();
        Player E = e.transform.GetComponent<Player>();
        if (lose == "player")
        {
            player_ini.SetActive(true);
            enemy_ini.SetActive(false);
            P.initiative = true;
            E.initiative = false;
        }
        else if (lose == "enemy")
        {
            P.initiative = false;
            E.initiative = true;
        }
        else if (lose == "move")
        {
            if (P.initiative)//イニシアチブの表示
            {
                P.initiative = false;
                E.initiative = true;
            }
            else
            {
                P.initiative = true;
                E.initiative = false;
            }
        }
        if (P.initiative)
        {
            player_ini.SetActive(true);
        }
        else
        {
            player_ini.SetActive(false);
        }
        if (E.initiative)
        {
            enemy_ini.SetActive(true);
        }
        else
        {
            enemy_ini.SetActive(false);
        }
    }

    void Mana_regenate()
    {
        Player P = p.transform.GetComponent<Player>();
        Player E = e.transform.GetComponent<Player>();
        P.mp = P.mp + P.mp_reg;
        E.mp = E.mp + E.mp_reg;
        if (GameManager.turn_count == 5)
        {
            P.mp_reg = P.mp_reg + 1;
            E.mp_reg = E.mp_reg + 1;
        }
        win_lose();
    }

    IEnumerator Battle_system()
    {
        GameObject p_use = GameObject.FindWithTag("use");//プレイヤーが使用したカード取得
        GameObject p_battle = GameObject.FindWithTag("battle");
        if (GameManager.ONLINE)
        {
            if (p_use != null)
            {
                CardController send_card = p_use.GetComponent<CardController>();
                GameManager.instance.SendCard(send_card.model.cardID, "use");
            }
            if (p_battle != null)
            {
                CardController[] temp_cards = player_temp.gameObject.GetComponentsInChildren<CardController>();
                for (int number = 0; number < temp_cards.Length; number++)
                {
                    if (temp_cards[number].gameObject.name == p_battle.name)
                    {
                        GameManager.instance.SendCard(number, "move");
                        break;
                    }
                }
            }
            GameManager.instance.Send();
            yield return StartCoroutine(Wait());
        }
        Player P = p.transform.GetComponent<Player>();
        Player E = e.transform.GetComponent<Player>();
        float i = 0;

        //---------強制的にエフェクトを初期に戻す。コルーチンがきちんと処理できれば不要-----------
        if (p_use != null)
        {
            GameObject child = p_use.transform.Find("Effect").gameObject;
            Image image = child.GetComponent<Image>();
            image.color = new Color(image.color[0], image.color[1], image.color[2], 0);
        }
        //---------ここまで-----------
        //カードを使用したら使用カードを移動、透明にして裏面を表示する
        #region //アニメーション
        GameObject e_use = GameObject.FindWithTag("enemy");
        if (p_use != null)
        {
            p_use.transform.SetParent(player_use.transform);
            p_use.SetActive(false);//ここでオブジェクトを非アクティブにしているせいでコルーチンが止まる。要修正
            CardController card = p_use.GetComponent<CardController>();
            P.hand.Remove(card.model.cardID);
            P.mp = P.mp - card.model.cost;
            win_lose();
        }

        if (e_use != null)
        {
            e_use.SetActive(false);
            CardController card = e_use.GetComponent<CardController>();
            E.hand.Remove(card.model.cardID);
            E.mp = E.mp - card.model.cost;
            win_lose();
        }
        //裏面表示
        while (i < 1)
        {
            i = i + 0.1f;
            Player_ura((p_use != null), "view", i);
            Enemy_ura((e_use != null), "view", i);
            yield return new WaitForSeconds(0.01f);
        }
        //裏面表示終了

        // 戦場に移動する

        if (p_battle != null)
        {
            p_battle.transform.SetParent(player_field.transform);
            p_battle.tag = "Untagged";
            CardController data = p_battle.GetComponent<CardController>();
            battle_data("player", data);
        }
        if (GameManager.ONLINE == false)
        {
            CPU.instance.select_battle();
        }
        GameObject e_battle = GameObject.FindWithTag("enemy_battle");
        if (e_battle != null)
        {
            e_battle.transform.SetParent(enemy_field.transform);
            e_battle.tag = "Untagged";
            CardController e_data = e_battle.GetComponent<CardController>();
            battle_data("enemy", e_data);
        }
        yield return new WaitForSeconds(1f);

        //回転開始
        i = 0f;
        while (i < 91f)
        {
            Player_ura((p_use != null), "rotate", i);
            Enemy_ura((e_use != null), "rotate", i);
            i = i + 5f;
            yield return new WaitForSeconds(0.01f);
        }
        //90度回転表面表示
        if (p_use != null)
        {
            p_use.SetActive(true);
        }
        if (e_use != null)
        {
            e_use.SetActive(true);
        }
        while (i > 0f)
        {
            if (p_use != null)
            {
                p_use.transform.eulerAngles = new Vector3(0, i, 0);
            }
            if (e_use != null)
            {
                e_use.transform.eulerAngles = new Vector3(0, i, 0);
            }
            i = i - 5f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        #endregion

        //--------------------使ったカードの効果を発動する----------------------

        if (P.initiative)
        {
            if (p_use != null)
            {
                Card_process ta = p_use.GetComponent<Card_process>();
                yield return ta.StartCoroutine(ta.Run(Timing.USE, "player"));
                yield return StartCoroutine(DestroyCards());
                win_lose();
            }
            if (e_use != null)
            {
                Card_process ta = e_use.GetComponent<Card_process>();
                yield return ta.StartCoroutine(ta.Run(Timing.USE, "enemy"));
                yield return StartCoroutine(DestroyCards());
                win_lose();
            }
        }
        else
        {
            if (e_use != null)
            {
                Card_process ta = e_use.GetComponent<Card_process>();
                yield return ta.StartCoroutine(ta.Run(Timing.USE, "enemy"));
                yield return StartCoroutine(DestroyCards());
                win_lose();
            }
            if (p_use != null)
            {
                Card_process ta = p_use.GetComponent<Card_process>();
                yield return ta.StartCoroutine(ta.Run(Timing.USE, "player"));
                yield return StartCoroutine(DestroyCards());
                win_lose();
            }
        }

        yield return StartCoroutine(field_move());

        yield return new WaitForSeconds(1f);

        //もし戦場に1枚でもカードがあるなら
        CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
        CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
        if (p_field_count.Length > 0 || e_field_count.Length > 0)
        {
            message_window.SetActive(true);
            // ダメージ計算処理
            yield return StartCoroutine(Damage_calc());
        }
        GameManager.turn_flag = 0;
        yield return new WaitForSeconds(0.5f);
        Start_turn();
    }

    IEnumerator field_move()
    {
        GameObject p_use = GameObject.FindWithTag("use");//プレイヤーが使用したカード取得
        GameObject e_use = GameObject.FindWithTag("enemy");
        //待機所に移動する処理
        if (p_use != null)
        {
            Card_process[] children = player_temp.GetComponentsInChildren<Card_process>();
            if (children.Length == 3)
            {
                Card_process pro = p_use.GetComponent<Card_process>();
                yield return pro.StartCoroutine(pro.DestroyCard());
                Destroy(p_use);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                p_use.transform.SetParent(player_temp.transform);
                p_use.tag = "Untagged";
            }
        }
        if (e_use != null)
        {
            Card_process[] children = enemy_temp.GetComponentsInChildren<Card_process>();
            if (children.Length == 3)
            {
                Card_process pro = e_use.GetComponent<Card_process>();
                yield return pro.StartCoroutine(pro.DestroyCard());
                Destroy(e_use);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                e_use.transform.SetParent(enemy_temp.transform);
                e_use.tag = "enemy_temp";
            }
        }
    }

    void Player_ura(bool flag, string move, float i)//プレイヤーのカードの描写処理
    {
        if (flag)
        {
            if (move == "view")
            {
                img.color = new Color(img.color[0], img.color[1], img.color[2], i);
                img2.color = new Color(img2.color[0], img2.color[1], img2.color[2], i);
                img3.color = new Color(img3.color[0], img3.color[1], img3.color[2], i);
            }
            if (move == "rotate")
            {
                img.transform.eulerAngles = new Vector3(0, i, 0);
                img2.transform.eulerAngles = new Vector3(0, i, 0);
                img3.transform.eulerAngles = new Vector3(0, i, 0);
            }
        }
    }

    void Enemy_ura(bool flag, string move, float i)//敵のカードの描写処理
    {
        if (flag)
        {
            if (move == "view")
            {
                img4.color = new Color(img4.color[0], img4.color[1], img4.color[2], i);
                img5.color = new Color(img5.color[0], img5.color[1], img5.color[2], i);
                img6.color = new Color(img6.color[0], img6.color[1], img6.color[2], i);
            }
            if (move == "rotate")
            {
                img4.transform.eulerAngles = new Vector3(0, i, 0);
                img5.transform.eulerAngles = new Vector3(0, i, 0);
                img6.transform.eulerAngles = new Vector3(0, i, 0);
            }
        }
    }

    IEnumerator Turn_start()
    {
        yield return new WaitForSeconds(0.5f);
        message_window.SetActive(true);
        message.text = GameManager.turn_count + " ターン";
        message.color = new Color(255f, 255f, 255f, 0);
        float i = 0f;
        while (i < 1f)
        {
            i = i + 0.5f;
            message.color = new Color(message.color[0], message.color[1], message.color[2], i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        while (i > 0f)
        {
            i = i - 0.3f;
            message.color = new Color(message.color[0], message.color[1], message.color[2], i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        message_window.SetActive(false);
    }

    IEnumerator Damage_calc()
    {
        yield return new WaitForSeconds(0.1f);

        Player P = p.transform.GetComponent<Player>();
        Player E = e.transform.GetComponent<Player>();

        Result p_res = player_result_process.gameObject.GetComponent<Result>();
        Result e_res = enemy_result_process.gameObject.GetComponent<Result>();

        CardController[] p_field_count = player_field.GetComponentsInChildren<CardController>();
        CardController[] e_field_count = enemy_field.GetComponentsInChildren<CardController>();
        if (p_field_count.Length == 0 && e_field_count.Length == 0)//お互いの戦場にカードが無い
        {}

        else if (p_field_count.Length == 1 && e_field_count.Length == 0)//プレイヤーだけカードがある
        {
            GameObject p_card = player_field.GetChild(0).gameObject;
            Card_process p_process = p_card.GetComponent<Card_process>();
            CardController p_cc = p_card.GetComponent<CardController>();
            if (hp_var.text == "0")//プレイヤーの戦場のHPが0
            {
                yield return p_process.StartCoroutine(p_process.Blackout());
                yield return StartCoroutine(Judge("DRAW"));
                yield return p_process.StartCoroutine(p_process.DestroyCard());
                P.trash.Add(p_cc.model.cardID);
                Destroy(p_card);
                field_reset("player");
            }
            else
            {
                Initiative_move("enemy");
                yield return StartCoroutine(Judge("WIN"));
                if (player_result_process.text != "")
                {
                    if (result_win_lose("win", player_result_process.text))
                    {
                        yield return p_process.StartCoroutine(p_process.Run(Timing.WIN, "player"));
                        yield return p_res.StartCoroutine(p_res.process("win","player"));
                    }
                }
                E.hp = E.hp - 1;
                win_lose();
            }
        }

        else if (p_field_count.Length == 0 && e_field_count.Length == 1)//エネミーだけカードがある
        {
            GameObject e_card = enemy_field.GetChild(0).gameObject;
            Card_process e_process = e_card.GetComponent<Card_process>();
            CardController e_cc = e_card.GetComponent<CardController>();
            if (enemy_hp_var.text == "0")
            {
                yield return e_process.StartCoroutine(e_process.Blackout());
                yield return StartCoroutine(Judge("DRAW"));
                yield return e_process.StartCoroutine(e_process.DestroyCard());
                E.trash.Add(e_cc.model.cardID);
                Destroy(e_card);
                field_reset("enemy");
            }
            else
            {
                Initiative_move("player");
                yield return StartCoroutine(Judge("LOSE"));
                if (enemy_result_process.text != "")
                {
                    if (result_win_lose("win", enemy_result_process.text))
                    {
                        yield return e_process.StartCoroutine(e_process.Run(Timing.WIN, "enemy"));
                        yield return e_res.StartCoroutine(e_res.process("win","enemy"));
                    }
                }
                P.hp = P.hp - 1;
                win_lose();
            }
        }

        else if (p_field_count.Length == 1 && e_field_count.Length == 1)//対戦開始
        {
            GameObject p_card = player_field.GetChild(0).gameObject;
            GameObject e_card = enemy_field.GetChild(0).gameObject;
            Card_process p_process = p_card.GetComponent<Card_process>();
            Card_process e_process = e_card.GetComponent<Card_process>();
            CardController p_cc = p_card.GetComponent<CardController>();
            CardController e_cc = e_card.GetComponent<CardController>();
            int Pa = int.Parse(atk_var.text);
            int Ph = int.Parse(hp_var.text);
            int Ea = int.Parse(enemy_atk_var.text);
            int Eh = int.Parse(enemy_hp_var.text);
            if (enemy_hp_var.text == "0" && hp_var.text == "0")//引き分け
            {
                yield return StartCoroutine(Judge("DRAW"));
                //両方のエフェクトタイミングを合わせるため敵側だけyield return
                p_process.StartCoroutine(p_process.DestroyCard());
                yield return e_process.StartCoroutine(e_process.DestroyCard());
                P.trash.Add(p_cc.model.cardID);
                E.trash.Add(e_cc.model.cardID);
                Destroy(p_card);
                Destroy(e_card);

                field_reset("player");
                field_reset("enemy");
            }
            else if (hp_var.text == "0" && enemy_hp_var.text != "0") // 負け
            {
                yield return StartCoroutine(Judge("LOSE"));
                if (P.initiative)
                {
                    if (player_result_process.text != "")
                    {
                        if (result_win_lose("lose", player_result_process.text))
                        {
                            yield return p_process.StartCoroutine(p_process.Run(Timing.LOSE, "player"));
                            yield return p_res.StartCoroutine(p_res.process("lose","player"));
                        }
                    }
                    if (enemy_result_process.text != "")
                    {
                        if (result_win_lose("win", enemy_result_process.text))
                        {
                            yield return e_process.StartCoroutine(e_process.Run(Timing.WIN, "enemy"));
                            yield return e_res.StartCoroutine(e_res.process("win","enemy"));
                        }
                    }
                }
                else
                {
                    if (enemy_result_process.text != "")
                    {
                        if (result_win_lose("win", enemy_result_process.text))
                        {
                            yield return e_process.StartCoroutine(e_process.Run(Timing.WIN, "enemy"));
                            yield return e_res.StartCoroutine(e_res.process("win","enemy"));
                        }
                    }
                    if (player_result_process.text != "")
                    {
                        if (result_win_lose("lose", player_result_process.text))
                        {
                            yield return p_process.StartCoroutine(p_process.Run(Timing.LOSE, "player"));
                            yield return p_res.StartCoroutine(p_res.process("lose","player"));
                        }
                    }
                }
                yield return p_process.StartCoroutine(p_process.DestroyCard());
                P.trash.Add(p_cc.model.cardID);
                Destroy(p_card);
                P.hp = P.hp - 1;
                field_reset("player");
            }
            else if (enemy_hp_var.text == "0" && hp_var.text != "0") // 勝ち
            {
                yield return StartCoroutine(Judge("WIN"));
                if (P.initiative)
                {
                    if (player_result_process.text != "")
                    {
                        if (result_win_lose("win", player_result_process.text))
                        {
                            yield return p_process.StartCoroutine(p_process.Run(Timing.WIN, "player"));
                            yield return p_res.StartCoroutine(p_res.process("win","player"));
                        }
                    }
                    if (enemy_result_process.text != "")
                    {
                        if (result_win_lose("lose", enemy_result_process.text))
                        {
                            yield return e_process.StartCoroutine(e_process.Run(Timing.LOSE, "enemy"));
                            yield return e_res.StartCoroutine(e_res.process("lose","enemy"));
                        }
                    }
                }
                else
                {
                    if (enemy_result_process.text != "")
                    {
                        if (result_win_lose("lose", enemy_result_process.text))
                        {
                            yield return e_process.StartCoroutine(e_process.Run(Timing.LOSE, "enemy"));
                            yield return e_res.StartCoroutine(e_res.process("lose","enemy"));
                        }
                    }
                    if (player_result_process.text != "")
                    {
                        if (result_win_lose("win", player_result_process.text))
                        {
                            yield return p_process.StartCoroutine(p_process.Run(Timing.WIN, "player"));
                            yield return p_res.StartCoroutine(p_res.process("win","player"));
                        }
                    }
                }
                yield return e_process.StartCoroutine(e_process.DestroyCard());
                E.trash.Add(e_cc.model.cardID);
                Destroy(e_card);
                E.hp = E.hp - 1;
                field_reset("enemy");
            }
            else
            {
                yield return StartCoroutine(Judge("Fight"));

                int count = 0;
                #region イニシアチブと先行後行の計算
                if (P.initiative)
                {
                    count = count + 1;
                }
                if (P.initiative == false)
                {
                    count = count - 1;
                }
                if (player_spd.text == "先行")
                {
                    count = count + 2;
                }
                if (player_spd.text == "後攻")
                {
                    count = count - 2;
                }
                if (enemy_spd.text == "先行")
                {
                    count = count - 2;
                }
                if (enemy_spd.text == "後攻")
                {
                    count = count + 2;
                }
                #endregion

                foreach (Text t in enemy_abi)//急所と半減のダメージ量計算
                {
                    if (t.text == "急所")
                    {
                        Pa = Pa * 2;
                    }
                    if (t.text == "半減")
                    {
                        Pa = Pa / 2;
                    }
                }
                foreach (Text t in player_abi)
                {
                    if (t.text == "急所")
                    {
                        Ea = Ea * 2;
                    }
                    if (t.text == "半減")
                    {
                        Ea = Ea / 2;
                    }
                }

                if (count > 0)//プレイヤー先行
                {
                    while (true)//プレイヤー側から殴り始める処理
                    {
                        Eh = Eh - Pa;
                        yield return e_process.StartCoroutine(e_process.Hit());
                        if (Eh <= 0)
                        {
                            Eh = 0;
                            enemy_hp_var.text = Eh.ToString();
                            yield return e_process.StartCoroutine(e_process.Blackout());
                            break;
                        }
                        enemy_hp_var.text = Eh.ToString();
                        yield return new WaitForSeconds(0.5f);

                        Ph = Ph - Ea;
                        yield return p_process.StartCoroutine(p_process.Hit());
                        if (Ph <= 0)
                        {
                            Ph = 0;
                            hp_var.text = Ph.ToString();
                            yield return p_process.StartCoroutine(p_process.Blackout());
                            break;
                        }
                        hp_var.text = Ph.ToString();
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                else//エネミー先行
                {
                    while (true)//エネミー側から殴り始める処理
                    {
                        Ph = Ph - Ea;
                        yield return p_process.StartCoroutine(p_process.Hit());
                        if (Ph <= 0)
                        {
                            Ph = 0;
                            hp_var.text = Ph.ToString();
                            yield return p_process.StartCoroutine(p_process.Blackout());
                            break;
                        }
                        hp_var.text = Ph.ToString();
                        yield return new WaitForSeconds(0.5f);

                        Eh = Eh - Pa;
                        yield return e_process.StartCoroutine(e_process.Hit());
                        if (Eh <= 0)
                        {
                            Eh = 0;
                            enemy_hp_var.text = Eh.ToString();
                            yield return e_process.StartCoroutine(e_process.Blackout());
                            break;
                        }
                        enemy_hp_var.text = Eh.ToString();
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                if (enemy_hp_var.text == "0" && hp_var.text != "0") // 勝ち
                {
                    yield return StartCoroutine(Judge("WIN"));
                    if (P.initiative)
                    {
                        if (player_result_process.text != "")
                        {
                            if (result_win_lose("win", player_result_process.text))
                            {
                                yield return p_process.StartCoroutine(p_process.Run(Timing.WIN, "player"));
                                yield return p_res.StartCoroutine(p_res.process("win","player"));
                            }
                        }
                        if (enemy_result_process.text != "")
                        {
                            if (result_win_lose("lose", enemy_result_process.text))
                            {
                                yield return e_process.StartCoroutine(e_process.Run(Timing.LOSE, "enemy"));
                                yield return e_res.StartCoroutine(e_res.process("lose","enemy"));
                            }
                        }
                    }
                    else
                    {
                        if (enemy_result_process.text != "")
                        {
                            if (result_win_lose("lose", enemy_result_process.text))
                            {
                                yield return e_process.StartCoroutine(e_process.Run(Timing.LOSE, "enemy"));
                                yield return e_res.StartCoroutine(e_res.process("lose","enemy"));
                            }
                        }
                        if (player_result_process.text != "")
                        {
                            if (result_win_lose("win", player_result_process.text))
                            {
                                yield return p_process.StartCoroutine(p_process.Run(Timing.WIN, "player"));
                                yield return p_res.StartCoroutine(p_res.process("win","player"));
                            }
                        }
                    }
                    yield return e_process.StartCoroutine(e_process.DestroyCard());
                    E.trash.Add(e_cc.model.cardID);
                    Destroy(e_card);
                    E.hp = E.hp - 1;
                    Initiative_move("enemy");
                    field_reset("enemy");
                }
                else if (enemy_hp_var.text != "0" && hp_var.text == "0") // 負け
                {
                    yield return StartCoroutine(Judge("LOSE"));
                    if (P.initiative)
                    {
                        if (player_result_process.text != "")
                        {
                            if (result_win_lose("lose", player_result_process.text))
                            {
                                yield return p_process.StartCoroutine(p_process.Run(Timing.LOSE, "player"));
                                yield return p_res.StartCoroutine(p_res.process("lose","player"));
                            }
                        }
                        if (enemy_result_process.text != "")
                        {
                            if (result_win_lose("win", enemy_result_process.text))
                            {
                                yield return e_process.StartCoroutine(e_process.Run(Timing.WIN, "enemy"));
                                yield return e_res.StartCoroutine(e_res.process("win","enemy"));
                            }
                        }
                    }
                    else
                    {
                        if (enemy_result_process.text != "")
                        {
                            if (result_win_lose("win", enemy_result_process.text))
                            {
                                yield return e_process.StartCoroutine(e_process.Run(Timing.WIN, "enemy"));
                                yield return e_res.StartCoroutine(e_res.process("win","enemy"));
                            }
                        }
                        if (player_result_process.text != "")
                        {
                            if (result_win_lose("lose", player_result_process.text))
                            {
                                yield return p_process.StartCoroutine(p_process.Run(Timing.LOSE, "player"));
                                yield return p_res.StartCoroutine(p_res.process("lose","player"));
                            }
                        }
                    }
                    yield return p_process.StartCoroutine(p_process.DestroyCard());
                    P.trash.Add(p_cc.model.cardID);
                    Destroy(p_card);
                    P.hp = P.hp - 1;
                    Initiative_move("player");
                    field_reset("player");
                }
                else if (enemy_hp_var.text == "0" && hp_var.text == "0")//引き分け
                {
                    yield return StartCoroutine(Judge("DRAW"));
                    p_process.StartCoroutine(p_process.DestroyCard());
                    yield return e_process.StartCoroutine(e_process.DestroyCard());
                    P.trash.Add(p_cc.model.cardID);
                    E.trash.Add(e_cc.model.cardID);
                    Destroy(p_card);
                    Destroy(e_card);
                    field_reset("player");
                    field_reset("enemy");
                }
            }
        }
        yield return new WaitForSeconds(1f);
        GameManager.obj = null;
        GameManager.flag = "wait";
    }

    public void battle_data(string user, CardController data)
    {
        if (user == "player")
        {
            field_reset("player");
            atk_var.text = data.model.atk.ToString();
            hp_var.text = data.model.hp.ToString();
            lv_var.text = data.model.cost.ToString();
            if (data.model.spd == "先行")
            {
                player_spd.color = new Color(1, 0, 0, 1);
                player_spd.text = "先行";
            }
            if (data.model.spd == "後攻")
            {
                player_spd.color = new Color(0.2245f, 0.4622f, 0.2348f, 1);
                player_spd.text = "後攻";
            }
            if (data.model.abi != "")
            {
                player_abi[0].color = new Color(1, 1, 0, 1);
                player_abi[0].text = data.model.abi;
            }
            result_set("player", data.model.cardID);
        }
        else if (user == "enemy")
        {
            field_reset("enemy");
            enemy_atk_var.text = data.model.atk.ToString();
            enemy_hp_var.text = data.model.hp.ToString();
            enemy_lv_var.text = data.model.cost.ToString();
            if (data.model.spd == "先行")
            {
                enemy_spd.color = new Color(1, 0, 0, 1);
                enemy_spd.text = "先行";
            }
            if (data.model.spd == "後攻")
            {
                enemy_spd.color = new Color(0.2245f, 0.4622f, 0.2348f, 1);
                enemy_spd.text = "後攻";
            }
            if (data.model.abi != "")
            {
                enemy_abi[0].color = new Color(1, 1, 0, 1);
                enemy_abi[0].text = data.model.abi;
            }
            result_set("enemy", data.model.cardID);
        }
    }

    void result_set(string user, int id)
    {
        int[] nums = { 325, 339};
        foreach (int num in nums)
        {
            if (num == id)
            {
                if (user == "player")
                {
                    player_result_process.text = num.ToString();
                    break;
                }
                else if (user == "enemy")
                {
                    enemy_result_process.text = num.ToString();
                    break;
                }
            }
        }
    }

    bool result_win_lose(string fight_result, string id)
    {
        int[] wins = { 325, 339};
        int[] loses = { };
        if (fight_result == "win")
        {
            foreach (int win in wins)
            {
                if (win.ToString() == id)
                {
                    return true;
                }
            }
        }
        else if (fight_result == "lose")
        {
            foreach (int lose in loses)
            {
                if (lose.ToString() == id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void field_reset(string human)
    {
        if (human == "player")
        {
            Player P = p.transform.GetComponent<Player>();
            hp_var.text = "";
            atk_var.text = "";
            lv_var.text = "";
            player_spd.text = "";
            player_result_process.text = "";
            foreach (Text t in player_abi)
            {
                t.text = "";
            }
            win_lose();
        }
        if (human == "enemy")
        {
            Player E = e.transform.GetComponent<Player>();
            enemy_hp_var.text = "";
            enemy_atk_var.text = "";
            enemy_lv_var.text = "";
            enemy_spd.text = "";
            enemy_result_process.text = "";
            foreach (Text t in enemy_abi)
            {
                t.text = "";
            }
            win_lose();
        }
    }

    IEnumerator Wait()
    {
        while (GameManager.online_wait)
        {
            yield return null;
        }
        if (GameManager.enemy_use_flag)
        {
            CardController card = Instantiate(cardPrefab, enemy_use);
            card.Init(GameManager.online_num);
            card.name = card.model.name;
            card.tag = "enemy";
            GameObject obj = GameObject.FindWithTag("enemy");
            Vector3 scale = obj.transform.localScale;
            scale.x = -1;
            obj.transform.localScale = scale;
            GameManager.online_num = 0;
            GameManager.enemy_use_flag = false;
        }
    }

    public void win_lose()
    {
        Player P = p.transform.GetComponent<Player>();
        Player E = e.transform.GetComponent<Player>();
        P.Stat_update();
        E.Stat_update();
        if (P.hp == 0 && E.hp == 0)
        {
            //Draw
        }
        else if (E.hp == 0)
        {
            //勝利
        }
        else if (P.hp == 0)
        {
            //敗北
        }
    }

    public IEnumerator Judge(string data)
    {
        message_window.SetActive(true);
        message.text = data;
        if (data == "Figth")
        {
            message.fontSize = 60;
            message.color = new Color(0.8980f, 0.7701f, 0.1647f, 0);
        }
        if (data == "DRAW")
        {
            message.fontSize = 60;
            message.color = new Color(0, 0, 1, 0);
        }
        if (data == "WIN")
        {
            message.fontSize = 60;
            message.color = new Color(1, 0.3333f, 0, 0);
        }
        if (data == "LOSE")
        {
            message.fontSize = 60;
            message.color = new Color(0, 0, 1, 0);
        }
        if (data == "対戦相手が退出しました")
        {
            message.fontSize = 30;
        }

        message.color = new Color(message.color[0], message.color[1], message.color[2], 0);
        float i = 0f;
        while (i < 1f)
        {
            i = i + 0.5f;
            message.color = new Color(message.color[0], message.color[1], message.color[2], i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        while (i > 0f)
        {
            i = i - 0.3f;
            message.color = new Color(message.color[0], message.color[1], message.color[2], i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        message_window.SetActive(false);
    }

    IEnumerator DestroyCards()// 使用したカードを破壊するような効果の破壊エフェクト用。処理中にオブジェクトを破壊すると処理が止まってしまうため
    {
        GameObject destoy_card = GameObject.FindWithTag("destroy");
        if (destoy_card != null)
        {
            GameObject parent = destoy_card.transform.parent.gameObject;//親
            GameObject parent2 = parent.transform.parent.gameObject;//親の親。Player or Enemy
            Player p = parent2.GetComponent<Player>();
            Card_process ta = destoy_card.GetComponent<Card_process>();
            CardController c = destoy_card.GetComponent<CardController>();
            yield return ta.StartCoroutine(ta.DestroyCard());
            if (c.model.cardID != 314)
            {
                p.trash.Add(c.model.cardID);
            }
            Destroy(destoy_card);
        }
        yield return new WaitForSeconds(0.5f);
    }
}