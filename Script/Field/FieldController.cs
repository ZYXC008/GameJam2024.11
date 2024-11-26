using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldController : MonoBehaviour
{
    public Transform playerPosition;
    private Animator anim;
    public Vector2 fixPosition;
    public bool isField;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(playerPosition.position.x + (fixPosition.x*playerPosition.localScale.x), playerPosition.position.y + fixPosition.y, transform.position.z);
    }

    public void SetField(bool isField)
    {
        this.isField = isField;
        anim.SetBool("isField", isField);
    }

}
