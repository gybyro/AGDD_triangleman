using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class StartAsHidden : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

}
