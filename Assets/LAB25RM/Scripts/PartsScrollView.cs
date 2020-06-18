using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsScrollView : MonoBehaviour
{
    public static PartsScrollView Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<PartsScrollView>();
            return instance;
        }
    }
    private static PartsScrollView instance;

    public Image[] image;

    public Sprite nullSprite;

    public List<Sprite> customSprites;

    public List<Sprite> spritePack;

    public CanvasGroup canvasGroup;

    int index;

    int spriteIndex;

    public int[] spriteQuantity;
    int spriteQuantityIndex = 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        ShowTargetCustomization();
    //    }
    //}

    public void ShowTargetCustomization()
    {
        for (int i = 0; i < spriteQuantity[spriteQuantityIndex]; i++)
        {
            customSprites.Add(spritePack[0]);
            spritePack.RemoveAt(0);
        }

        UpdateCustomPartsInstruction();
        spriteQuantityIndex++;
    }

    public void UpdateCustomPartsInstruction()
    {
        for (int i = 0; i < 5; i++)
        {
            image[i].sprite = nullSprite;
        }

        int count = customSprites.Count;
        for (int i = 0; i < count; i++)
        {
            image[i].sprite = customSprites[0];
            customSprites.RemoveAt(0);
        }
    }
}
