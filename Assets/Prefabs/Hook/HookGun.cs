using UnityEngine;
using UnityEngine.SceneManagement;

public class HookGun : Equippable
{
    //[HideInInspector] public bool isEquipped = true;

    //[Header("Scripts Ref:")]

    [Header("Launch Key:")]
    public KeyCode launchKey;

    [Header("Layers Settings:")]
    //[SerializeField] private int grappableLayerNumber;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    private GrapplingRope grappleRope;
    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;


    [Header("TESTING")]
    [SerializeField] private float detachAngle = 45f;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    [HideInInspector] public bool canHook;

    private Health playerHealth;
    private SpriteRenderer spriteRenderer;
    private BasicPlayerMovement playerMovement;

    private void Awake()
    {
        playerHealth = GetComponentInParent<Health>();
        playerMovement = GetComponentInParent<BasicPlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        grappleRope = GetComponentInChildren<GrapplingRope>();
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

    private void Update()
    {
        if (!m_camera) return;

        if (playerHealth.IsDead() || !isEquipped)
        {
            spriteRenderer.enabled = false;
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            return;
        }
        else
            spriteRenderer.enabled = true;

        if (canHook)
        {
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
            //Debug.DrawLine(grapplePoint, gunPivot.position);
            //bool rightApproach = Vector2.Dot(Vector2.right, ((Vector2)gunPivot.position - grapplePoint)) > 0;
            float dir = playerMovement.getDirection();
            float right = gunPivot.position.x - grapplePoint.x;

            //Debug.Log(dir + "\t" + right);

            //if (rightApproach)
            //    Debug.Log("coming from right");
            //else
            //    Debug.Log("coming from left");

            RotateGun(grapplePoint, true);
            

            //Debug.Log(Mathf.Acos(angle * Mathf.Rad2Deg));

            if (Input.GetKeyDown(launchKey))
            {
                grappleRope.enabled = true;
                RotateGun(grapplePoint);
            }
            if (dir * right > 0)    //if player went past the middle point in the swing
            {
                var angle = Vector2.Dot(Vector2.down, ((Vector2)gunPivot.position - grapplePoint).normalized);
                if (angle < Mathf.Cos(detachAngle * Mathf.Deg2Rad))
                {
                    Debug.Log("detach");
                    grappleRope.enabled = false;
                    m_springJoint2D.enabled = false;
                }
            }
        }
        else
            RotateGun(gunPivot.position + Vector3.forward * 3, true);


        //else
        //{
        //    Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //    RotateGun(mousePos);
        //}

        //if (Input.GetKey(launchKey))
        //{
        //    if (grappleRope.enabled) //if left-clicked and got a grapple point hit
        //    {
        //        RotateGun(grapplePoint);
        //    }
        //    else  //if left-clicked and didnt get a grapple point hit just rotate relative to the mouse
        //    {
        //        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //        RotateGun(mousePos);
        //    }

        //    if (launchToPoint && grappleRope.isGrappling)
        //    {
        //        if (launchType == LaunchType.Transform_Launch)
        //        {
        //            Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
        //            Vector2 targetPos = grapplePoint - firePointDistnace;
        //            gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
        //        }
        //    }
        //}

        //if (Input.GetKeyUp(launchKey))
        //{
        //    grappleRope.enabled = false;
        //    m_springJoint2D.enabled = false;
        //    //m_rigidbody.gravityScale = 1;
        //}
        //else
        //{
        //    Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //    RotateGun(mousePos);
        //}
    }

    void RotateGun(Vector3 lookPoint, bool easeIn = false)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if(!easeIn)
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * 3.5f);
    } 

    //void SetGrapplePoint()
    //{
    //    //Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
    //    //if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
    //    //{
    //    //    RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
    //    //    if (_hit.transform.gameObject.layer == grappableLayerNumber)
    //    //    {
    //    //        //if(canHook)
    //    //        //{
    //    //        //    grapplePoint = _hit.point;
    //    //        //    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
    //    //        //    grappleRope.enabled = true;
    //    //        //}

    //    //        grapplePoint = _hit.point;
    //    //        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
    //    //    }
    //    //}

    //    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
    //}

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }

        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }
}