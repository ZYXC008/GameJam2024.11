using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldFollowPlayer : MonoBehaviour
{
    public Transform playerPosition;

    private void FixedUpdate()
    {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, transform.position.z);
    }
}
