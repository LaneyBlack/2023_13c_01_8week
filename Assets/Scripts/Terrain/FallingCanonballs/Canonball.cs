using UnityEngine;

public class Cannonball : MonoBehaviour
{
    private CannonballTrap _parentTrap;
    private Animator _animator;
    private Rigidbody2D _rb;
    private Collider2D _col;
    private bool _hasHitPlayer = false;

    private void Start()
    {
        _parentTrap = transform.parent.GetComponent<CannonballTrap>();
        _animator = GetComponent<Animator>(); 
        _rb = GetComponent<Rigidbody2D>(); 
        _col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_hasHitPlayer)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                _hasHitPlayer = true;
            }
        }

        if (_rb != null)
            _rb.simulated = false;

        if (_col != null)
            _col.enabled = false;

        PlayExplosionAnimation();
    }

    private void PlayExplosionAnimation()
    {
        _animator.SetTrigger("Explode");
        Invoke("DeactivateAfterAnimation", 1f);
    }

    private void DeactivateAfterAnimation()
    {
        if (_rb != null)
            _rb.simulated = true;

        if (_col != null)
            _col.enabled = true;

        _hasHitPlayer = false;
        _parentTrap.ReturnToPool(gameObject);
    }
}