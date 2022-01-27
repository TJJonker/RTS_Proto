using UnityEngine;

public class UnitRS : MonoBehaviour
{
    private GameObject selectedGameObject;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selection").gameObject;
    }

    public void SetSelectedActive(bool active) => selectedGameObject.SetActive(active);
}
