using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RingMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool useGravity = true;
    public float gravity = -9.8f;
    public Transform gravityCenter = null;

    private Rigidbody rb = null;
    private Vector3 moveAmount = Vector3.zero;

    private void Start()
    {
        rb = rb ?? GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (!gravityCenter)
            gravityCenter = new GameObject("Gravity Center").transform;
    }

    private void Update()
    {
        float v = Input.GetAxis("Vertical");
        moveAmount = new Vector3(0, 0, v) * moveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount));

        if (useGravity)
        {
            Vector3 gravityDir = (transform.position - gravityCenter.position).normalized;

            transform.rotation = Quaternion.FromToRotation(transform.up, gravityDir) * transform.rotation;

            rb.AddForce(gravity * gravityDir);
        }
    }
}

[CustomEditor(typeof(RingMovement))]
public class RingMovementEditor : Editor
{
    private RingMovement script = null;

    private void OnEnable()
    {
        script = script ?? target as Movement;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        script.moveSpeed = EditorGUILayout.FloatField("Move Speed", script.moveSpeed);
        script.useGravity = EditorGUILayout.Toggle("Use Gravity", script.useGravity);

        if (script.useGravity)
        {
            script.gravity = EditorGUILayout.FloatField("Gravity", script.gravity);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gravityCenter"), new GUIContent("Center"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
