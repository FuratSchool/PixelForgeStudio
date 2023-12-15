using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtons : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _animation;

    [SerializeField] private bool DoNotPause;
    // Start is called before the first frame update
    void Start()
    {
        
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        if (!DoNotPause)
        {
            InvertColor(false);
            _animator.enabled = false;
        }
        else
        {
            InvertColor(true);
        }
    }
    public void OnSelect (BaseEventData eventData) 
    {
        _animator.enabled = true;
        _animator.Play(_animation.name);
        InvertColor(true);
    }
    
    public void OnDeselect (BaseEventData eventData) 
    {
        _animator.enabled = false;
        transform.localScale = new Vector3(1,1,1);
        InvertColor(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.enabled = true;
        _animator.Play(_animation.name);
        InvertColor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.enabled = false;
        transform.localScale = new Vector3(1,1,1);
        InvertColor(false);
    }

    void InvertColor(bool invert)
    {
        if (invert)
        {
            GetComponent<Image>().color = Color.white;
            transform.GetChild(0).GetComponent<TMP_Text>().color = Color.black;
        }
        else
        {
            GetComponent<Image>().color = Color.black;
            transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        }
    }
}
