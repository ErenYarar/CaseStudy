using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform tr;
    Transform player;

    Vector3 vec;

    private void Start()
    {
        tr = transform;
        player = GameObject.FindWithTag("Player").transform;

        vec = -tr.forward * 25f; //25

        tr.position = player.position + vec;
    }

    private void FixedUpdate()
    {
        tr.position = Vector3.Lerp(tr.position, player.position + vec, 0.075f); 
    }

}
