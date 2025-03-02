using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelector : MonoBehaviour
{
    [System.Serializable]
    public class SpriteData
    {
        public string name;
        public Sprite sprite;
    }

    public List<SpriteData> sprites = new List<SpriteData>(); // List of sprites with names
    public Image displayImage;   // UI Image to display the sprite
    public Button nextButton;    // Button to go to the next sprite
    public Button prevButton;    // Button to go to the previous sprite
    public Image nextButtonImage; // Image component on the next button
    public Image prevButtonImage; // Image component on the previous button

    private int currentIndex = 0;

    void Start()
    {
        if (sprites.Count == 0 || displayImage == null || nextButton == null || prevButton == null) 
        {
            Debug.LogError("Please assign all required references in the Inspector.");
            return;
        }

        // Add listeners to buttons
        nextButton.onClick.AddListener(NextSprite);
        prevButton.onClick.AddListener(PreviousSprite);

        // Set initial sprite
        UpdateSprite();
    }

    void NextSprite()
    {
        if (sprites.Count == 0) return;
        currentIndex = (currentIndex + 1) % sprites.Count;
        UpdateSprite();
    }

    void PreviousSprite()
    {
        if (sprites.Count == 0) return;
        currentIndex = (currentIndex - 1 + sprites.Count) % sprites.Count;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (sprites.Count == 0) return;

        // Set the main displayed image
        displayImage.sprite = sprites[currentIndex].sprite;

        // Update button images
        nextButtonImage.sprite = sprites[(currentIndex + 1) % sprites.Count].sprite;
        prevButtonImage.sprite = sprites[(currentIndex - 1 + sprites.Count) % sprites.Count].sprite;

        Debug.Log("Selected Sprite: " + sprites[currentIndex].name);
    }

    public string GetSelectedSpriteName()
    {
        return sprites.Count > 0 ? sprites[currentIndex].name : "";
    }
}
