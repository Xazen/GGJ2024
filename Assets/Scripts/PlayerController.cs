using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public void OnMovement(InputValue value)
    {
        var movement = value.Get<Vector2>();
        gameObject.transform.position += new Vector3(movement.x, 0, movement.y);
    }

    public void OnAttack()
    {
        Debug.Log("Attack");
    }
}
