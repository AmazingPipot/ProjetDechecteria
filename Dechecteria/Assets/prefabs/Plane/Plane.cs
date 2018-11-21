using UnityEngine;

public class Plane : MonoBehaviour
{
    public float Speed;
    [SerializeField]
    protected float PropellerSpeed;
    [SerializeField]
    private Transform Propeller;

	void Update ()
    {
        Propeller.Rotate(new Vector3(0, 0, PropellerSpeed * Time.deltaTime));
        transform.position = transform.position + transform.forward * Speed * Time.deltaTime;
	}
}
