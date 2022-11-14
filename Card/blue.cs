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

    public void blue_200(string _user)
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
            int[] cards = {P.deck[0],P.deck[1],P.deck[2],P.deck[3]};
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
                number = number +1;
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
            }
            else
            {
                E.hand.Add(E.deck[0]);
                E.deck.RemoveAt(0);
            }
        }
    }




}