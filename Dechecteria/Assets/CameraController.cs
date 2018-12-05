using System.Collections;
using UnityEngine;
using Dechecteria;

public class CameraController : MonoBehaviour
{
    public float MovementSpeed;
    public float RotationSpeed;
    public float ZoomSpeed;

    public float Angle;
    public float Radius;

    public float MinY;
    public float MaxY;

    public Vector3 CameraPosition;

    protected Vector3 LastMousePosition;

    public Creature Creature;

    [Header("Move to creature animation")]
    public AnimationCurve MoveToCreatureCurve;
    public float MoveToCreatureDuration;

    enum Direction
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT
    };

	// Use this for initialization
	void Start ()
    {
        UpdateOrbitPosition();
        UpdateOrbitRotation();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawLine(transform.position, CameraPosition, Color.yellow);
        Vector3 vectorDir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Debug.DrawLine(transform.position, transform.position + vectorDir, Color.red);

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveCamera(Direction.FORWARD);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MoveCamera(Direction.BACKWARD);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveCamera(Direction.LEFT);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveCamera(Direction.RIGHT);
        }

        if (Input.mouseScrollDelta.y != 0.0f)
        {
            ZoomCamera();
        }

        if (Input.GetMouseButtonDown(2))
        {
            LastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            Vector2 offset = new Vector2(LastMousePosition.x - Input.mousePosition.x, 0);
            Debug.Log("offset: " + offset.x);
            Angle += offset.x * RotationSpeed * Time.unscaledDeltaTime;

            LastMousePosition = Input.mousePosition;
            UpdateOrbitPosition();
            UpdateOrbitRotation();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(MoveToCreature());
        }
    }

    IEnumerator MoveToCreature()
    {
        Vector3 start = CameraPosition;
        float elapsed = 0.0f;
        while (elapsed < MoveToCreatureDuration)
        {
            Vector3 v = Creature.transform.position - start;
            CameraPosition = start + v * MoveToCreatureCurve.Evaluate(elapsed / MoveToCreatureDuration);
            UpdateOrbitPosition();
            elapsed += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    void UpdateOrbitPosition()
    {
        float distance = Mathf.Tan(transform.rotation.eulerAngles.x * Mathf.Deg2Rad) * transform.position.y;
        Debug.Log(distance);
        transform.position = new Vector3(CameraPosition.x + Mathf.Cos(Angle * Mathf.Deg2Rad) * distance, transform.position.y, CameraPosition.z + Mathf.Sin(Angle * Mathf.Deg2Rad) * distance);
    }

    void UpdateOrbitRotation()
    {
        float a = Mathf.Atan2(CameraPosition.z - transform.position.z, CameraPosition.x - transform.position.x);
        transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -a * Mathf.Rad2Deg + 90, transform.rotation.eulerAngles.z);
    }

    void ZoomCamera()
    {
        Vector3 velocity = transform.forward * Input.mouseScrollDelta.y * ZoomSpeed * Time.unscaledDeltaTime;
        if ((transform.position + velocity).y >= MinY && (transform.position + velocity).y <= MaxY)
        {
            transform.position += velocity;
            UpdateOrbitPosition();
        }
    }

    void MoveCamera(Direction dir)
    {
        Vector3 vectorDir;

        switch (dir)
        {
            case Direction.FORWARD:
                vectorDir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
                CameraPosition += vectorDir * MovementSpeed * Time.unscaledDeltaTime;
                break;
            case Direction.BACKWARD:
                vectorDir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
                CameraPosition -= vectorDir * MovementSpeed * Time.unscaledDeltaTime;
                break;
            case Direction.LEFT:
                vectorDir = new Vector3(transform.right.x, 0, transform.right.z).normalized;
                CameraPosition -= vectorDir * MovementSpeed * Time.unscaledDeltaTime;
                break;
            case Direction.RIGHT:
                vectorDir = new Vector3(transform.right.x, 0, transform.right.z).normalized;
                CameraPosition += vectorDir * MovementSpeed * Time.unscaledDeltaTime;
                break;
        }

        UpdateOrbitPosition();
    }
}
