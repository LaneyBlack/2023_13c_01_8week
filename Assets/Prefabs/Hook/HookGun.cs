using UnityEngine;
using UnityEngine.SceneManagement;

public class HookGun : Equippable
{
    [Header("Launch Key:")]
    public KeyCode launchKey;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;
    public BoxCollider2D boxcol;


    [Header("Launching:")]
    [SerializeField] private float targetFrequncy = 1;

    [Header("Detach")]
    [SerializeField] private float detachAngle = 45f;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    [HideInInspector] public bool canHook;

    private GrapplingRope grappleRope;
    private Health playerHealth;
    private float targetDistance;
    private SpriteRenderer spriteRenderer;
    private BasicPlayerMovement playerMovement;
    private Rigidbody2D playerrb;

    //private bool hasGrapplePoint = false;

    RaycastHit2D[] dupa;

    private void Awake()
    {
        playerHealth = GetComponentInParent<Health>();
        playerMovement = GetComponentInParent<BasicPlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        grappleRope = GetComponentInChildren<GrapplingRope>();
        playerrb = GetComponentInParent<Rigidbody2D>();
        boxcol = GetComponentInParent<BoxCollider2D>();
    }

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        canHook = false;
        if (!m_camera)
        {
            m_camera = Camera.main;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!m_camera)
        {
            m_camera = Camera.main;
        }
    }

    public void disableGrappling()
    {
        spriteRenderer.enabled = false;
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;

        grappleRope.arrow.enabled = false;
    }

    private void Update()
    {
        //flip the sprite in the correct direcion:

        if (!m_camera) return;

        if (playerHealth.IsDead() || !isEquipped)
        {
            disableGrappling();
            return;
        }
        else
        {
            spriteRenderer.enabled = true;
            grappleRope.arrow.enabled = true;
        }

        //spriteRenderer.flipX = playerMovement.getDirection() < 0;

        findGrapplePoint();
        if (canHook)
        {
            float dir = playerMovement.getDirection();
            //Debug.Log(Mathf.Sign(dir));
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;

            float right = gunPivot.position.x - grapplePoint.x;

            RotateGun(grapplePoint, true);
            
            if (Input.GetKeyDown(launchKey))
            {
                grappleRope.enabled = true;
                playerMovement.jumpFullReset();
                RotateGun(grapplePoint);
                //playerrb.AddForce(new Vector2(-Mathf.Sign(right) * 40, 20), ForceMode2D.Impulse);
            }
            if (dir * right > 0)    //if player went past the middle point in the swing
            {
                var angle = Vector2.Dot(Vector2.down, ((Vector2)gunPivot.position - grapplePoint).normalized);
                if (angle < Mathf.Cos(detachAngle * Mathf.Deg2Rad))
                {
                    grappleRope.enabled = false;
                    m_springJoint2D.enabled = false;
                    canHook = false;
                }
            }
        }
        else
            RotateGun(gunPivot.position + (Vector3.forward * 3), true);
    }

    void findGrapplePoint()
    {
        float minDistance = Mathf.Infinity;
        GameObject selectedGP = null;
        Vector2 attachPointPos = new Vector2(0, 0);

        var gpoints = GameObject.FindGameObjectsWithTag("GrapplePoint");
        //Debug.Log(gpoints.Length);
        foreach (var g in gpoints)
        {
            var gp = g.GetComponent<GrapplePoint>();

            var firePos = firePoint.transform.position;
            var holderPos = gunHolder.transform.position;

            float dst = Vector2.Distance(g.transform.position, holderPos);
            //add a ray cast!
            if (gp.canAttach(holderPos) && dst < minDistance)
            {
                dupa = Physics2D.RaycastAll(firePos, (g.transform.position - firePos).normalized, (g.transform.position - firePos).magnitude);
                Debug.DrawRay(firePos, (g.transform.position - firePos).normalized, Color.red);
                //var hits = Physics2D.RaycastAll(firePos, (g.transform.position - firePos).normalized, Mathf.Infinity);

                //foreach(var hit in hits)
                //{
                //    if (hit.collider != null)
                //    {
                //        Debug.DrawLine(firePos, hit.point, Color.yellow);
                //        Gizmos.color = Color.yellow;
                //        Gizmos.DrawCube(hit.point, new Vector3(.1f, .1f));
                //        Debug.Log(hit.collider.gameObject.tag);
                //    }
                //}

                //if(hit.collider != null && hit.collider.gameObject.CompareTag("GrapplePoint"))
                //{
                //    //Debug.DrawLine(holderPos, hit.point, Color.magenta);

                //    Debug.Log("got hit");
                //    selectedGP = g;
                //    minDistance = dst;
                //    attachPointPos = hit.point;
                //}

                //TODO: write code that ignores enemies, collectibles etc when raycasting and sets the attach point correctly
            }
        }

        if (selectedGP != null && !canHook)
        {
            canHook = true;
            grapplePoint = attachPointPos;
            targetDistance = selectedGP.GetComponent<GrapplePoint>().minSwingRadius;
        }
        if(selectedGP == null)
            canHook = false;
    }

    void RotateGun(Vector3 lookPoint, bool easeIn = false)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if(!easeIn)
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * 7f);
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        m_springJoint2D.distance = targetDistance;
        m_springJoint2D.frequency = targetFrequncy;

        m_springJoint2D.connectedAnchor = grapplePoint;
        m_springJoint2D.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Debug.Log("hits = " + dupa.Length);
        foreach(var hit in dupa)
        {
            Debug.DrawLine(firePoint.transform.position, hit.point, Color.yellow);
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(hit.point, new Vector3(.3f, .3f));
            Debug.Log(hit.collider.gameObject.tag);
        }
    }
}