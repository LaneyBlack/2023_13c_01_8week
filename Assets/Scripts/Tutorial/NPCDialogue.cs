using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialNPC : MonoBehaviour
{
    public TextMeshProUGUI instructionTextMesh;
    public float typingSpeed = 0.05f; // The delay between each letter
    private int currentInstructionIndex = 0;
    private bool playerInRange = false;
    private Camera mainCamera;

    // Define your instructions and positions
    private string[] instructions = new string[]
    {
        "Welcome to the game!",
        "Collect coins to score points."
    };

    private Vector3[] positions = new Vector3[]
    {
        new Vector3(-10f, -2.45f, 0f), // Position A
        new Vector3(8.98f, 0.55f, 0f), // Position B
    };

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
        instructionTextMesh.gameObject.SetActive(false); // Hide the text initially
        MoveToNextPosition();
    }

    void MoveToNextPosition()
    {
        if(currentInstructionIndex >= positions.Length)
        {
            gameObject.SetActive(false); // Deactivate the NPC if all instructions are done
            return;
        }

        transform.position = positions[currentInstructionIndex]; // Move the NPC
        CheckVisibility(); // Check if the NPC is within camera's view
        if(playerInRange) // Start typing the text only if the player is in range
        {
            instructionTextMesh.gameObject.SetActive(true); // Ensure the text object is active before typing starts
            StartCoroutine(TypeSentence(instructions[currentInstructionIndex])); // Start typing the instruction text
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        instructionTextMesh.text = ""; // Clear the text
        foreach (char letter in sentence.ToCharArray())
        {
            instructionTextMesh.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait a bit before adding the next one
        }
    }

    void Update()
    {
        CheckVisibility(); // Constantly check for visibility in case camera moves
    }

    void CheckVisibility()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        // Activate/Deactivate the NPC based on camera's view
        gameObject.SetActive(onScreen);

        // If NPC is on screen and player is in range, but the text object is not active, start the typing coroutine
        if(playerInRange && onScreen && !instructionTextMesh.gameObject.activeInHierarchy)
        {
            instructionTextMesh.gameObject.SetActive(true);
            StartCoroutine(TypeSentence(instructions[currentInstructionIndex]));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Do not set the text directly here. It will be set by the TypeSentence coroutine
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            instructionTextMesh.gameObject.SetActive(false); // Hide the text immediately
            StartCoroutine(CheckForPlayerDistance());
        }
    }

    IEnumerator CheckForPlayerDistance()
    {
        yield return new WaitForSeconds(2.0f);

        if (!playerInRange)
        {
            currentInstructionIndex++;
            MoveToNextPosition();
        }
    }
}
