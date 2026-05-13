using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;

    [SerializeField] private float rotationSpeed = 1.0f;
    private float currentRotationAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRotationAngle += (Time.deltaTime * 10);
        if (xAxis == true)
        {
            transform.rotation = Quaternion.Euler(currentRotationAngle * rotationSpeed, 0f, 0f);
        }

        if (yAxis == true)
        {
            transform.rotation = Quaternion.Euler(0f, currentRotationAngle * rotationSpeed, 0f);
        }

        if (zAxis == true)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotationAngle * rotationSpeed);
        }
    }
}
