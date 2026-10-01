using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static Transform ButtonPlace { private set; get; }
    void Start()
    {
        transform.position = new Vector3(
            AsunaMovmentControler.PlayerPosition.x
            , AsunaMovmentControler.PlayerPosition.y
            , AsunaMovmentControler.PlayerPosition.z
        );
        ButtonPlace = transform;
    }

    void Update()
    {
        if (transform.position != AsunaMovmentControler.PlayerPosition)
            transform.position = new Vector3(
                AsunaMovmentControler.PlayerPosition.x
                , AsunaMovmentControler.PlayerPosition.y
                , AsunaMovmentControler.PlayerPosition.z
            );
        ButtonPlace = transform;
    }
}
