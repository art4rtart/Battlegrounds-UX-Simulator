using UnityEngine;
using UnityEngine.UI;


public class WeaponSelect : MonoBehaviour
{
    public Sprite defaultsprite;
    public Sprite targetSprite;

    Image image;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        image = this.gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void HoverEnter()
    {
        image.sprite = targetSprite;
        animator.SetBool("Hover", true);
    }

    public void HoverExit()
    {
        image.sprite = defaultsprite;
        animator.SetBool("Hover", false);
    }

    public void SelectWeapon()
    {

    }
}
