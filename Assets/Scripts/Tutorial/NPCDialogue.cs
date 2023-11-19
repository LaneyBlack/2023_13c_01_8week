using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialNPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _instructionTextMesh;
    [SerializeField] private float _typingSpeed = 0.05f;
    private int _currentInstructionIndex;

    private string[] _instructions = new string[]
    {
        "Welcome to Pirate Bay! Navigate through the realm using 'A' and 'D' to move left and right. Press 'SPACE' to leap over obstacles and reach new heights!",
        "Alert! Hostile creatures lurk in these lands. Click the left mouse button to defend yourself and keep them at bay. Stay vigilant!",
        "Watch your step! Some platforms have a mind of their own and might shift beneath you. Time your moves carefully.",
        "See that trampoline down there? Use it to catapult yourself across the divide. Bounce your way to uncharted territories!",
        "Those boxes might come in handy. Push them towards the ledge to create a makeshift staircase. A little extra height could be just what you need to conquer new challenges!",
        "Ahoy! See that flag up ahead? Cross it and it becomes your new spawn point. Should ye fall, you'll respawn there",
        "I trust you've gathered the coins strewn along your path. When you're ready, press 'F' to open these grand doors. Inside, you'll find a shop brimming with treasures - potions to bolster your strength and swords to enhance your might. After you've stocked up, venture forth and press 'F' at the exit door to embark on the next leg of your journey. Good luck!"
    };

    private Vector3[] _positions = new Vector3[]
    {
        new Vector3(-6.5f, -2.45f, 0f),
        new Vector3(8.98f, 0.55f, 0f),
        new Vector3(19.8f, 4.55f, 0f),
        new Vector3(37.5f, 0.55f, 0f),
        new Vector3(51.5f, 0.55f, 0f),
        new Vector3(70.5f,0.55f,0f),
        new Vector3(100f,0.55f,0f)
    };

    private Coroutine _typingCoroutine;

    private void Start()
    {
        _instructionTextMesh.gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        StartCoroutine(InitialAppearance());
    }

    private IEnumerator InitialAppearance()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TransitionToNextPosition());
    }

    private IEnumerator TransitionToNextPosition()
    {
        if (_currentInstructionIndex >= _positions.Length)
        {
            yield return StartCoroutine(DisappearWithEffect());
            gameObject.SetActive(false);
            yield break;
        }

        yield return StartCoroutine(DisappearWithEffect());
        transform.position = _positions[_currentInstructionIndex];
        yield return StartCoroutine(AppearWithEffect());
    }

    private void DisplayCurrentInstructionText()
    {
        if (_typingCoroutine == null)
        {
            _instructionTextMesh.gameObject.SetActive(true);
            _typingCoroutine = StartCoroutine(TypeSentence(_instructions[_currentInstructionIndex]));
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _instructionTextMesh.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _instructionTextMesh.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DisplayCurrentInstructionText();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _instructionTextMesh.gameObject.SetActive(false);
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }
            MoveToNextInstruction();
        }
    }

    private void MoveToNextInstruction()
    {
        _currentInstructionIndex++;
        StartCoroutine(TransitionToNextPosition());
    }

    private IEnumerator AppearWithEffect()
    {
        yield return StartCoroutine(ScaleEffect(Vector3.zero, Vector3.one, 1f));
    }

    private IEnumerator DisappearWithEffect()
    {
        yield return StartCoroutine(ScaleEffect(transform.localScale, Vector3.zero, 1f));
    }

    private IEnumerator ScaleEffect(Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            yield return null;
        }
        transform.localScale = endScale;
    }
}
