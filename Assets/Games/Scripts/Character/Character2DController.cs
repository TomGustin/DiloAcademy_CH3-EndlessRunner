using UnityEngine;

public class Character2DController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;

    [Header("Physic Properties")]
    [SerializeField] private PhysicBase physicBase;

    [Header("References")]
    [SerializeField] private CharacterVisual visual;

    private const float gravityChangeDelay = 2f;
    private float timerGravity = 2f;

    private bool stopped;

    private void Awake()
    {
        physicBase.SetBody(transform, GetComponent<Rigidbody2D>());
        physicBase.SetGravity(PhysicBase.GravityMode.Downside);
    }

    private void Update()
    {
        if (stopped) return;
        ReadInput();

        if (timerGravity < gravityChangeDelay)
        {
            timerGravity += Time.deltaTime;
        } else
        {
            timerGravity = gravityChangeDelay;
        }

        HUD.Instance.UpdateGravityDelay(timerGravity / gravityChangeDelay);
    }

    private void FixedUpdate()
    {
        if (stopped) return;
        physicBase.CastSensor();
        UpdateMove();
        physicBase.Update();
    }

    private void UpdateMove()
    {
        if (physicBase.isJumping)
        {
            physicBase.velocity.y += physicBase.gravityMode.Equals(PhysicBase.GravityMode.Downside) ? jumpPower : - jumpPower;
            physicBase.isJumping = false;
        }

        physicBase.velocity.x = Mathf.Clamp(physicBase.velocity.x + acceleration * Time.deltaTime, 0f, maxSpeed);
    }

    private void ReadInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (physicBase.isOnGround)
            {
                physicBase.isJumping = true;
                physicBase.onJumping = true;
                AudioSystem.Instance.PlaySFX("jump");
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && timerGravity == gravityChangeDelay)
        {
            ToggleGravity();
            timerGravity = 0;
        }

        visual.UpdateAnimation("OnGround", physicBase.isOnGround);
    }

    public void Stop(bool stop)
    {
        stopped = stop;
        physicBase.Stop(stopped);
    }

    private void ChangeGravity(PhysicBase.GravityMode gravityMode)
    {
        physicBase.SetGravity(gravityMode);

        switch (gravityMode)
        {
            case PhysicBase.GravityMode.Upside:
                transform.localScale = new Vector3(1f, -1f, 1f);
                break;
            case PhysicBase.GravityMode.Downside:
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }
    }

    public void ToggleGravity()
    {
        if (physicBase.gravityMode.Equals(PhysicBase.GravityMode.Downside))
        {
            ChangeGravity(PhysicBase.GravityMode.Upside);
        } else ChangeGravity(PhysicBase.GravityMode.Downside);
    }

    private void OnDrawGizmos()
    {
        physicBase?.OnDebug();
    }

    [System.Serializable]
    public class PhysicBase
    {
        public float gravityScale;
        public float sensorLength;
        public LayerMask sensorLayer;
        public bool isJumping;
        public bool onJumping;
        public bool isOnGround;
        public GravityMode gravityMode;
        public enum GravityMode { Upside, Downside }

        public Vector2 velocity;
        private Transform body;
        private Rigidbody2D physic;

        private Vector3 CastDirection
        {
            get
            {
                return gravityMode.Equals(GravityMode.Upside) ? body.up : -body.up;
            }
        }

        public void SetGravity(GravityMode mode)
        {
            gravityMode = mode;

            switch (gravityMode)
            {
                case GravityMode.Upside:
                    physic.gravityScale = -gravityScale;
                    break;
                case GravityMode.Downside:
                    physic.gravityScale = gravityScale;
                    break;
            }
        }

        public void SetBody(Transform body, Rigidbody2D physic)
        {
            this.body = body;
            this.physic = physic;
        }

        public void CastSensor()
        {
            velocity = physic.velocity;

            RaycastHit2D hit = Physics2D.Raycast(body.position, CastDirection, sensorLength, sensorLayer);

            if (hit)
            {
                if (!isOnGround && velocity.y <= 0)
                {
                    isOnGround = true;
                    onJumping = false;
                }
            }
            else
            {
                isOnGround = false;
                if (!onJumping) velocity.y = gravityMode.Equals(GravityMode.Upside) ? 10f : -10f;
            }
        }

        public void Update()
        {
            physic.velocity = velocity;
        }

        public void Stop(bool stopped)
        {
            physic.simulated = !stopped;
        }

        public void OnDebug()
        {
            if (body) Debug.DrawLine(body.position, body.position + (CastDirection * sensorLength), Color.white);
        }
    }
}
