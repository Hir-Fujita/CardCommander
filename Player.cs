using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤー（自分）側のデッキやライフ等を所持するクラス

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject temp;
    [SerializeField] public GameObject use;
    [SerializeField] public GameObject field;
    // [SerializeField] public Transform deck;
    // [SerializeField] public Transform trash;
    // [SerializeField] public Transform hand;
    public List<int> deck = new List<int>();
    public List<int> trash = new List<int>();
    public List<int> hand = new List<int>();
    [SerializeField] public Text hp_var;
    [SerializeField] public Text mp_var;
    [SerializeField] public Text mp_reg_var;
    [SerializeField] public Text deck_var;
    [SerializeField] public Text trash_var;
    [SerializeField] public Text hand_var;
    public int hp {get;set;}
    public int mp {get;set;}
    public int mp_reg {get;set;}
    public bool initiative {get;set;}
    [SerializeField] public GameObject obj_hand;

public void Stat_update()
{
    hp_var.text = hp.ToString();
    mp_var.text = mp.ToString();
    mp_reg_var.text = mp_reg.ToString();
    deck_var.text = deck.Count.ToString();
    trash_var.text = trash.Count.ToString();
    hand_var.text = hand.Count.ToString();
}

public void Deck_create(int[] array) //デッキを生成してシャッフル
{
    deck.Clear();
    deck.AddRange(array);
    if (initiative)
    {
        for (int i = 0; i < 5;i++)
        {
            Draw();
        }
    }
    else
    {
        for (int i = 0; i < 6;i++)
        {
            Draw();
        }
    }
}

public void Draw()
{
    if (deck.Count == 0){
        return;
    }
    int cardID = deck[0];
    hand.Add(cardID);
    deck.RemoveAt(0);
    if (transform.name == "Player")
    {
        GameManager.instance.CreateCard(cardID);
        GridLayoutGroup g = obj_hand.GetComponent<GridLayoutGroup>();
        float canvas_x = 436.5587f;
        float card_x = 70f;
        if (card_x * hand.Count > canvas_x)
        {
            float x = (canvas_x - hand.Count * card_x) / (hand.Count -1);
            g.spacing = new Vector2(x,0);
        }
        else
        {
            g.spacing = new Vector2(0,0);
        }
    }
}

public void Add_hand(int num)
{
    hand.Add(num);
    if (transform.name == "Player")
    {
        GameManager.instance.CreateCard(num);
        GridLayoutGroup g = obj_hand.GetComponent<GridLayoutGroup>();
        float canvas_x = 436.5587f;
        float card_x = 70f;
        if (card_x * hand.Count > canvas_x)
        {
            float x = (canvas_x - hand.Count * card_x) / (hand.Count -1);
            g.spacing = new Vector2(x,0);
        }
    }
}

}
