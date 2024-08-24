using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform from;
    [SerializeField] private Transform ballTarget;
    [SerializeField] private GameObject ballPrefab; 

    [Header(" Settings ")]
    [SerializeField] private float flightDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchBall()
    {
        Vector3 pt = ballTarget.position;
        Vector3 gt2 = Physics.gravity * flightDuration * flightDuration / 2;
        Vector3 p0 = from.position;

        Vector3 initialVelocity = (pt - gt2 - p0) / flightDuration;

        GameObject ballInstance = Instantiate(ballPrefab, from.position, Quaternion.identity, transform);

        ballInstance.GetComponent<Rigidbody>().velocity = initialVelocity;
    }

    public void LaunchBall(Vector3 from, Vector3 to, float flightDuration)
    {
        Vector3 pt = to;
        Vector3 gt2 = Physics.gravity * flightDuration * flightDuration / 2;
        Vector3 p0 = from;

        Vector3 initialVelocity = (pt - gt2 - p0) / flightDuration;

        GameObject ballInstance = Instantiate(ballPrefab, from, Quaternion.identity, transform);

        ballInstance.GetComponent<Rigidbody>().velocity = initialVelocity;
    }
}
