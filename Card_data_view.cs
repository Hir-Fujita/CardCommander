using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// カードの詳細画面のコンポーネントを変更するクラス

public class Card_data_view : MonoBehaviour
{
    [SerializeField] GameObject card_view;
    [SerializeField] Text change_name;
    [SerializeField] Image change_image;
    [SerializeField] Text change_text;
    [SerializeField] Text change_cost;
    [SerializeField] Text change_atk;
    [SerializeField] Text change_hp;
    [SerializeField] Text change_spd;
    [SerializeField] Text change_trive;

    public void Init(CardController data)
    {
        change_name.text = data.model.name;
        change_image.sprite = data.model.icon;
        change_text.text = data.model.text_data;
        change_cost.text = data.model.cost.ToString();
        change_atk.text = data.model.atk.ToString();
        change_hp.text = data.model.hp.ToString();
        change_spd.text = data.model.spd;
        change_trive.text = data.model.trive;
        card_view.SetActive(true);
    }
    public void Close()
    {
        card_view.SetActive(false);
    }
}
