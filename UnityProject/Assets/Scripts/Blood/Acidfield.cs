using UnityEngine;
using System.Collections;
public class Blood2 : MonoBehaviour
{
    private float time = 0.0f;
    private bool flag = false;
  //  public Poison poison;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay(Collision collision,Collider other)
    {
      //  IDebuffable target = other.GetComponent<IDebuffable>();
        GameObject hitobj = collision.gameObject;
        if ((hitobj.CompareTag("Enemy")) || (hitobj.CompareTag("Player")))
        {
         //   target.ApplyDebuff(poison);

            time += Time.deltaTime;

            if (time >= 0.5f)
            {
                //だめーーーーじ
                //
                // hitobj.GetComponent();
                var damagetarget = hitobj.GetComponent<IDH>();
                if (damagetarget != null)
                {
                    hitobj.GetComponent<IDH>().Addacid(15);
                }
                Debug.Log("当たった");
                time = 0.0f;
            }
        }




    }
}
