using UnityEngine;

public class Blood4 : MonoBehaviour
{
    private float time = 0.0f;
    private bool flag = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay(Collision collision)
    {

        GameObject hitobj = collision.gameObject;
        if ((hitobj.CompareTag("Enemy")) || (hitobj.CompareTag("Player")))
        {

            time += Time.deltaTime;

            if (time >= 0.5f)
            {
                //だめーーーーじ
                //
                // hitobj.GetComponent();
                var damagetarget = hitobj.GetComponent<IDH>();
                if (damagetarget != null)
                {
                    hitobj.GetComponent<IDH>().Addheal(5);
                }
                Debug.Log("当たった");
                time = 0.0f;
            }
        }




    }
}
