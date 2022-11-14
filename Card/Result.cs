using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public static Result instance;

    [SerializeField] Text result;

    [SerializeField] CardController cardPrefab;
    [SerializeField] public GameObject user;
    [SerializeField] public Transform user_field;
    [SerializeField] public Transform user_temp;
    [SerializeField] public Text user_spd;
    [SerializeField] public Text user_atk;
    [SerializeField] public Text user_hp;

    [SerializeField] public GameObject target;
    [SerializeField] public Transform target_field;
    [SerializeField] public Transform target_temp;
    [SerializeField] public Text target_spd;
    [SerializeField] public Text target_atk;
    [SerializeField] public Text target_hp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public IEnumerator process(string res, string _user)
    {
        if (res == "win")
        {
            if (result.text == "325")//吸血鬼
            {
                int count = 2;
                CardController[] temp_count = user_temp.GetComponentsInChildren<CardController>();
                foreach (CardController c in temp_count)
                {
                    if (c.model.cardID == 301)
                    {
                        count = count + 1;
                    }
                }
                Player p = user.GetComponent<Player>();
                foreach (int b in p.trash)
                {
                    if (b == 301)
                    {
                        count = count + 1;
                    }
                }
                int atk = int.Parse(user_atk.text);
                int hp = int.Parse(user_hp.text);
                user_atk.text = (atk + count).ToString();
                user_hp.text = (hp + count).ToString();
            }

            if (result.text == "339")//オルトロス
            {
                if (GameManager.ONLINE)
                {
                    if (_user == "player")
                    {
                        Player e = target.GetComponent<Player>();
                        for (int i = 0; i < 2; i++)
                        {
                            int num = Random.Range(0, e.hand.Count - 1);
                            e.trash.Add(e.hand[num]);
                            e.hand.RemoveAt(num);
                        }
                    }
                    else if (_user == "enemy")
                    {
                        Player p = target.GetComponent<Player>();
                        int num = Random.Range(0, p.hand.Count - 1);
                        int num2 = Random.Range(0, p.hand.Count - 1);
                        while (num == num2)
                        {
                            num2 = Random.Range(0, p.hand.Count - 1);
                        }
                        GameObject card = p.transform.GetChild(4).gameObject;//子要素の5番目(hand)を取得
                        Card_process[] cards = card.transform.GetComponentsInChildren<Card_process>();
                        CardController[] cc = card.transform.GetComponentsInChildren<CardController>();
                        CardController c = cc[num2];
                        cards[num].StartCoroutine(cards[num].DestroyCard());
                        yield return cards[num2].StartCoroutine(cards[num2].DestroyCard());
                        p.trash.Add(cc[num].model.cardID);
                        p.trash.Add(c.model.cardID);
                        p.hand.Remove(cc[num].model.cardID);
                        p.hand.Remove(c.model.cardID);
                        Destroy(cc[num].gameObject);
                        Destroy(c.gameObject);
                    }
                }
                else
                {
                    if (_user == "player")
                    {
                        Player e = target.GetComponent<Player>();
                        for (int i = 0; i < 2; i++)
                        {
                            int num = Random.Range(0, e.hand.Count - 1);
                            e.trash.Add(e.hand[num]);
                            e.hand.RemoveAt(num);
                        }
                    }
                    else if (_user == "enemy")
                    {
                        Player p = target.GetComponent<Player>();
                        int num = Random.Range(0, p.hand.Count - 1);
                        int num2 = Random.Range(0, p.hand.Count - 1);
                        while (num == num2)
                        {
                            num2 = Random.Range(0, p.hand.Count - 1);
                        }
                        GameObject card = p.transform.GetChild(4).gameObject;//子要素の5番目(hand)を取得
                        Card_process[] cards = card.transform.GetComponentsInChildren<Card_process>();
                        CardController[] cc = card.transform.GetComponentsInChildren<CardController>();
                        CardController c = cc[num2];
                        cards[num].StartCoroutine(cards[num].DestroyCard());
                        yield return cards[num2].StartCoroutine(cards[num2].DestroyCard());
                        p.trash.Add(cc[num].model.cardID);
                        p.trash.Add(c.model.cardID);
                        p.hand.Remove(cc[num].model.cardID);
                        p.hand.Remove(c.model.cardID);
                        Destroy(cc[num].gameObject);
                        Destroy(c.gameObject);
                    }
                }
            }
        }
        else if (res == "lose")
        {

        }
        yield return new WaitForSeconds(0.5f);
    }
}

