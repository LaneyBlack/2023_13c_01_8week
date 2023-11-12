using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_isOpen && stateInfo.IsName("DoorOpen") && stateInfo.normalizedTime >= 1)
        {
            _animator.speed = 0;
        }
        else if (!_isOpen && stateInfo.IsName("DoorClose") && stateInfo.normalizedTime >= 1)
        {
            _animator.speed = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.speed = 1;
            _animator.SetBool("isOpen", true);
            _isOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.speed = 1;
            _animator.SetBool("isOpen", false);
            _isOpen = false;
        }
    }
}