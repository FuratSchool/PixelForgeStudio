using System.Collections;
using System.Collections.Generic;
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
            _animator.enabled = false;
        }
    }
    public void OnSelect (BaseEventData eventData) 
    {
        _animator.enabled = true;
        _animator.Play(_animation.name);
    }
    
    public void OnDeselect (BaseEventData eventData) 
    {
        _animator.enabled = false;
        transform.localScale = new Vector3(1,1,1);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.enabled = true;
        _animator.Play(_animation.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.enabled = false;
        transform.localScale = new Vector3(1,1,1);
    }
}
