using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//カードの見た目を制御するクラス

public class CardView : MonoBehaviour
{
    // [SerializeField] Text nameText, atkText, costText;
    [SerializeField] Image iconImage,colorImage,Effect;
    [SerializeField] Sprite img;

    public void Show(CardModel cardModel) // cardModelのデータ取得と反映
    {
        // nameText.text = cardModel.name;
        // atkText.text = cardModel.atk.ToString();
        // costText.text = cardModel.cost.ToString();
        if(cardModel.color_name == "red"){
            colorImage.color = new Color(255,0,0,255);
        }
        if(cardModel.color_name == "blue"){
            colorImage.color = new Color(0,0,255,255);
        }
        if(cardModel.color_name == "green"){
            colorImage.color = new Color(0,255,0,255);
        }
        if(cardModel.color_name == "black"){
            colorImage.color = new Color(80f/255f,80f/255f,80f/255f,255f/255f);
        }
        iconImage.sprite = cardModel.icon;
        Effect.sprite = img;
    }
}