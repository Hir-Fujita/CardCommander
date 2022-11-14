using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードの基礎データをunity上で入力するためのクラス

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] string color_name;
    [SerializeField] int cardID;
    [SerializeField] int cost;
    [SerializeField] int hp;
    [SerializeField] int atk;
    [SerializeField] string spd;
    [SerializeField] string trive;
    [SerializeField] string abi;
    [SerializeField] Sprite icon;
    [SerializeField,TextArea] string text_data;

    public string Name {get => name;}
    public string Color_name {get => color_name;}
    public int CardID {get => cardID;}
    public int Cost {get => cost;}
    public int Hp {get => hp;}
    public int Atk {get => atk;}
    public string Spd {get => spd;}
    public string Trive {get => trive;}
    public string Abi {get => abi;}
    public Sprite Icon {get => icon;}
    public string Text_data {get => text_data;}
}