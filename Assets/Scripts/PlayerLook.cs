using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public Transform playerCamera; //solo le damos un vector
    //public Camera playerCamera; // podemos pasarle la camara como tal

    private float xRotation = 0f;
    private Vector2 mouseInput; //cada vez que movamos el mouse nos da valores en X y Y

    private Vector3 camPosition;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // para que el cursor no se vea y quede fijo en el centro de la pantalla
        Cursor.visible = false;
        camPosition = playerCamera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
    }

    public void OnLook(InputValue data)
    {
        mouseInput = data.Get<Vector2>();

    }

    public void LookAround()
    {
        xRotation -= mouseInput.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // para que no gire mas de 90 grados hacia arriba o hacia abajo
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // para que gire solo en el eje X
        transform.Rotate(Vector3.up * mouseInput.x * mouseSensitivity * Time.deltaTime); // para que gire el personaje en el eje Y
    }
}
