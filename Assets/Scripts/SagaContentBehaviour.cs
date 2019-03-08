using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SagaContentBehaviour : MonoBehaviour
{
    public Image leftDeco;
    public Image rightDeco;
    public Image actionDeco;

    public Text contentText;

    public void SetDecos(Sprite left, Sprite action, Sprite right)
    {
        leftDeco.sprite = left;
        actionDeco.sprite = action;
        rightDeco.sprite = right;
    }

    public void SetText(string content)
    {
        contentText.text = content;
    }
}
