using UnityEngine;

public class DayNightCicle : MonoBehaviour
{
    private Light lIght;
    public float rotationSpeed = 1.0f;

    private void Start()
    {
        lIght = GetComponent<Light>();
    }
    void Update()
    {
        lIght.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
