using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput PlayerInput;

    // Start is called before the first frame update
    public void OnMovement(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        gameObject.transform.position += new Vector3(movement.x, 0, movement.y);
    }

    public void OnAttack()
    {
        Debug.Log("Attack");
    }
}
