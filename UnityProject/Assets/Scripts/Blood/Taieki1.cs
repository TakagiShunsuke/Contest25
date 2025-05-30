


using UnityEngine;

public class Taieki1 : MonoBehaviour
{
    private float time=0.0f;
    private bool flag = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("当たった");
        
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
                if(damagetarget !=null)
                {
                    hitobj.GetComponent<IDH>().Adddamege(15);
                }
                    Debug.Log("当たった");
                    time = 0.0f;
                }
            }
        

      

    }
}
