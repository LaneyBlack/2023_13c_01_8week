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

    private void Awake()
    {
        playerHealth = GetComponentInParent<Health>();
        playerMovement = GetComponentInParent<BasicPlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        grappleRope = GetComponentInChildren<GrapplingRope>();
        playerrb = GetComponentInParent<Rigidbody2D>();
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
            spriteRenderer.enabled = true;

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
        //var gpoints = GameObject.FindGameObjectsWithTag("GrapplePoint");
        //foreach (var g in gpoints)
        //{
        //    var gp = g.GetComponent<GrapplePoint>();
        //    if (gp.canAttach(transform.position) && !canHook)
        //    {
        //        canHook = true;
        //        grapplePoint = g.transform.position;
        //        targetDistance = gp.minSwingRadius;
        //        return;
        //    }
        //}

        float minDistance = Mathf.Infinity;
        GameObject selectedGP = null;

        var gpoints = GameObject.FindGameObjectsWithTag("GrapplePoint");
        foreach (var g in gpoints)
        {
            var gp = g.GetComponent<GrapplePoint>();
            float dst = Vector2.Distance(g.transform.position, gunHolder.transform.position);
            //add a ray cast!
            if (gp.canAttach(gunHolder.transform.position) && dst < minDistance)
            {
                selectedGP = g;
                minDistance = dst;
            }
        }

        if (selectedGP != null && !canHook)
        {
            canHook = true;
            grapplePoint = selectedGP.transform.position;
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
}