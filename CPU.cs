using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CPU対戦時の相手のカード選択を行うクラス

public class CPU : MonoBehaviour
{
    public static CPU instance;
    [SerializeField] CardController cardPrefab;
    [SerializeField] GameObject _enemy;
    [SerializeField] Transform use;
    [SerializeField] Transform field;

private void Awake()
{
    if(instance == null)
    {
        instance = this;
    }
}

public void select_hand()
{
    Player enemy = _enemy.transform.GetComponent<Player>();
    for (int i = enemy.hand.Count - 1; i > 0; i--) //手札をシャッフル
        {
            var j = Random.Range(0, i+1); // ランダムで要素番号を１つ選ぶ（ランダム要素）
            var temp = enemy.hand[i]; // 一番最後の要素を仮確保（temp）にいれる
            enemy.hand[i] = enemy.hand[j]; // ランダム要素を一番最後にいれる
            enemy.hand[j] = temp; // 仮確保を元ランダム要素に上書き
        }

    // enemy.mp = 10;
    // enemy.Stat_update();

    foreach (int i in enemy.hand)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/"+ i);
        if (cardEntity.Cost <= enemy.mp)//もし手札のi番目のカードのコストが現在の魔力以下なら
        {
            //カードを使う
            CardController card = Instantiate(cardPrefab, use);
            card.Init(i);
            card.name = card.model.name;
            card.tag = "enemy";
            GameObject obj = GameObject.FindWithTag("enemy");
            Vector3 scale = obj.transform.localScale;
            scale.x = -1;
            obj.transform.localScale = scale;
            break;
        }
    }
}

public void select_battle()
{
    GameObject enemy_field = GameObject.Find("p2_field");
    CardController[] e_field_count = enemy_field.transform.GetComponentsInChildren<CardController>();
    if (e_field_count.Length == 0)
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("enemy_temp");
        int r = Random.Range(0, cards.Length);
        if (cards.Length != 0)
        {
            cards[r].tag = "enemy_battle";
        }
    }
}
}