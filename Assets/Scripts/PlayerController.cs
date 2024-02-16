using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject model;

    private BalancingConfig balancingConfig;

    [Inject]
    [UsedImplicitly]
    public void Inject(BalancingConfig balancingConfig)
    {
        this.balancingConfig = balancingConfig;
    }

    public void OnMovement(InputValue value)
    {
        var movement = value.Get<Vector2>();
        var movement3D = new Vector3(movement.x, 0, movement.y);

        gameObject.transform.position += movement3D * Time.deltaTime * balancingConfig.PlayerMovementSpeed;

        if (movement != Vector2.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement3D);
            model.transform.rotation = newRotation;
        }
    }

    public void OnAttack()
    {
        Debug.Log("Attack");
    }
}
