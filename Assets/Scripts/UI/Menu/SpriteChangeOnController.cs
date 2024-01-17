using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChangeOnController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Sprite _keyboardSprite;
    [SerializeField] private Sprite _controllerSprite;

    void OnChangeInput(string Type)
    {
        Debug.Log(Type);
        if (Type.Equals("Controller"))
        {
            GetComponent<Image>().sprite = _controllerSprite;
        }
        else if (Type.Equals("KeyboardMouse"))
        {
            GetComponent<Image>().sprite = _keyboardSprite;
        }
    }
}
