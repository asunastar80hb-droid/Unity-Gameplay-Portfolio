using UnityEngine;

public class InfinityFishMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;      
    [SerializeField] float size = 5f;       
    private float _t;         
    private Vector3 _startLocalPos;

    void Start()
    {
        _startLocalPos = transform.localPosition;
    }
    void Update()
    {
        _t += Time.deltaTime * speed;

        float x = size * Mathf.Sin(_t);
        float z = size * Mathf.Sin(_t) * Mathf.Cos(_t);

        Vector3 newPos = _startLocalPos + new Vector3(x, transform.position.y, z);
        
        float maxRadius = size;
        if (newPos.magnitude > maxRadius)
        {
            newPos = newPos.normalized * maxRadius;
        }
        transform.localPosition = newPos;

        Vector3 direction = newPos - transform.localPosition;
        if(direction != Vector3.zero)
            transform.localRotation = Quaternion.LookRotation(direction);
    }
}
        
