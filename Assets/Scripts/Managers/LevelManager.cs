using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    // ----------------------------------------------
    // SOUND
    // ----------------------------------------------   
    //AudioSource source; // audiosource    
    //public AudioClip gameOverSound; // game over sound    
    //AudioSource bgMusic; // background music
    // ----------------------------------------------
    // FADE SCREEN
    // ----------------------------------------------
    private GameObject fadeOutScreen; // parent for the fade out screen
    private Canvas fadeCanvas; // overlay canvas
    public Image coverImage; // black overlay
    public Animator animator; // Animator for the fade screen
    public string levelToLoad; // what level to load
    // ----------------------------------------------

    private void Start()
    {
        // run the game as fast as possible.
        Application.targetFrameRate = 300;

        // set up audio source
        //source = GetComponent<AudioSource>();

        // get background music
        //bgMusic = Camera.main.GetComponentInChildren<AudioSource>();

        // FADE SCREEN SETUP
        fadeOutScreen = new GameObject("FadeOutScreen"); // create a gameobject for the fade out canvas
        fadeCanvas = fadeOutScreen.gameObject.AddComponent<Canvas>(); // add the canvas to the game object

        // set up the canvas properites
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay; // set the render move to overlay screen space

        coverImage = fadeCanvas.gameObject.AddComponent<Image>();
        coverImage.name = "COVER_IMAGE";
        coverImage.color = Color.black; // set the color to black
        coverImage.canvasRenderer.SetAlpha(1.0f);
        coverImage.rectTransform.anchorMin = new Vector2(1.0f, 0f);
        coverImage.rectTransform.anchorMax = new Vector2(0f, 1.0f);
        coverImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        coverImage.enabled = false;
    }
        
    public void LoadScene(string name) {
        //SceneManager.LoadScene(name);

        StartLoad(name);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // start the animation to load the level.
    // once the animation is finished then load the scene.
    public void StartLoad(string sceneName)
    {
        levelToLoad = sceneName; // set the scene name to be loaded
        animator.SetTrigger("FadeOut"); // trigger the fade out animation
    }

    // once the fade animation is complete load the level set in StartLoad()
    public void OnFadeComplete()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoad);
    }
}
