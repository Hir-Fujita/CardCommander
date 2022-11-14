using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//カードの数値を制御するクラス

[System.Serializable]
public class CardModel
{
    public int cardID;
    public string name;
    public string color_name;
    public int cost;
    public int hp;
    public int atk;
    public string spd;
    public string trive;
    public string abi;
    public string text_data;
    public Sprite icon;

    public CardModel(int ID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/"+ ID);
        cardID = cardEntity.CardID;
        color_name = cardEntity.Color_name;
        name = cardEntity.Name;
        cost = cardEntity.Cost;
        hp = cardEntity.Hp;
        atk = cardEntity.Atk;
        spd = cardEntity.Spd;
        trive = cardEntity.Trive;
        abi = cardEntity.Abi;
        text_data = cardEntity.Text_data;
        icon = cardEntity.Icon;
    }
}