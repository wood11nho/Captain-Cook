using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    bool gameOver = false;
    bool recipeBookOpen = false;
    int currentRecipeBookPage = 0;
    int totalRecipeBookPages = 0;

    [SerializeField]
    AudioSource splashAudioSource;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GameObject recipeBookPanel;

    [SerializeField]
    Text textOpenRecipe;

    private GameObject[] recipeBookPages;

    // Start is called before the first frame update
    void Start()
    {
        currentRecipeBookPage = 0;
        totalRecipeBookPages = recipeBookPanel.transform.childCount;

        controller = GetComponent<CharacterController>();

        recipeBookPages = new GameObject[recipeBookPanel.transform.childCount];
        for (int i = 0; i < recipeBookPanel.transform.childCount; i++)
        {
            recipeBookPages[i] = recipeBookPanel.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (transform.position.y <= 4.03 && !gameOver)
        {
            if (!splashAudioSource.isPlaying)
            {
                splashAudioSource.time = 0.3f; // Because the intro was too long
                splashAudioSource.Play();
            }
            if (transform.position.y <= 3.83)
            {
                gameManager.GetComponent<GameManager>().StopGame();
                gameManager.GetComponent<GameManager>().SetStrikes(3);
                gameOver = true;
            }
        }
    }

    // Receive the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        
        // Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void OpenRecipeBook()
    {
        recipeBookPanel.SetActive(true);
        recipeBookPages[0].SetActive(true);
        recipeBookOpen = true;

        textOpenRecipe.text = "CLOSE RECIPE BOOK (H)";
    }

    public void CloseRecipeBook()
    {
        recipeBookPanel.SetActive(false);
        for (int i = 0; i < recipeBookPages.Length; i++)
        {
            recipeBookPages[i].SetActive(false);
        }
        recipeBookOpen = false;
        currentRecipeBookPage = 0;

        textOpenRecipe.text = "OPEN RECIPE BOOK (H)";
    }

    public void setRecipeBookOpened(bool opened)
    {
        recipeBookOpen = opened;
    }

    public bool getRecipeBookOpened()
    {
        return recipeBookOpen;
    }

    public void setCurrentRecipeBookPage(int page)
    {
        currentRecipeBookPage = page;
    }

    public int getCurrentRecipeBookPage()
    {
        return currentRecipeBookPage;
    }

    public int getTotalRecipeBookPages()
    {
        return totalRecipeBookPages;
    }

    public void activateCurrentRecipeBookPage(int currentRecipeBookPage)
    {
        for (int i = 0; i < recipeBookPages.Length; i++)
        {
            recipeBookPages[i].SetActive(false);
        }
        recipeBookPages[currentRecipeBookPage].SetActive(true);
    }
}
