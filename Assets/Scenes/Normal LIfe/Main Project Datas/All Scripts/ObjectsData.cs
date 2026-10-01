using UnityEngine;
public class ObjectsData : Interactable
{
    [SerializeField] private string level;
    public GameObject NextLevelPrefab;
    private GameObject nextLevelGameObject;
    private void MakeTable()
    {
        if (NextLevelPrefab != null)
        {
            nextLevelGameObject = Instantiate(NextLevelPrefab ,transform.parent.transform.parent);
            nextLevelGameObject.transform.localPosition = Vector3.zero;
            nextLevelGameObject.transform.localScale = transform.parent.localScale;
            InteractionManager.Instance.Unregister(this);
            Destroy(transform.parent.gameObject);
            
        } 
    }

    public override void Interact()
    {
        MakeTable();
    }
    
}
