using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 t = new Vector3(0, 0,80);
    // Update is called once per frame
    void Update()
    {
        transform.Translate(t * Time.deltaTime);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision != null) { transform.Translate(new Vector3(0,100,0)                 ); }
    //}

}
