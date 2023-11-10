using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialNPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _instructionTextMesh;
    [SerializeField] private float _typingSpeed = 0.05f;
    private int _currentInstructionIndex;
    private bool _playerInRange;
    private Coroutine _typingCoroutine;

    private string[] _instructions = new string[]
    {
        "Welcome to the game!",
        "Collect coins to score points.",
        "Beware that some of the platforms can move!",
        "You can use the trampoline below to bounce to the other side.",
        "Try to push those boxes closer to the ledge, it should be enough height boost for you to jump!"
    };

    private Vector3[] _positions = new Vector3[]
    {
        new Vector3(-10f, -2.45f, 0f),
        new Vector3(8.98f, 0.55f, 0f),
        new Vector3(19.8f, 4.55f, 0f),
        new Vector3(37.5f, 0.55f, 0f),
        new Vector3(51.5f, 0.55f, 0f)
    };

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

        if (_playerInRange)
        {
            DisplayCurrentInstructionText();
        }
    }

    private void DisplayCurrentInstructionText()
    {
        _instructionTextMesh.gameObject.SetActive(true);
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }
        _typingCoroutine = StartCoroutine(TypeSentence(_instructions[_currentInstructionIndex]));
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
            _playerInRange = true;
            if (_currentInstructionIndex < _positions.Length)
            {
                DisplayCurrentInstructionText();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            _instructionTextMesh.gameObject.SetActive(false);
            _currentInstructionIndex++;
            StartCoroutine(TransitionToNextPosition());
        }
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
