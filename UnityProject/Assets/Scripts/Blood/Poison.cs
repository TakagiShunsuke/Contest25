//using UnityEngine;
//using System.Collections;
//[CreateAssetMenu(menuName ="Status Effects/Poison Debuff")]

//public class Poison : CAffect
//{
//    public float tickDamage = 5f;
//    public float tickInterval = 1f;
//    public override void Apply(IDebuffable target,float duration)
//    {
//        target.startcoroutine(ApplyPoison(target, duration));
//    }
//    public override void Remove(IDebuffable target)
//    {
        
//    }
//    private IEnumerator ApplyPoison(IDebuffable target,float duration)
//    {
//        float elapsed = 0f;
//        while(elapsed<duration)
//        {
//            target.TakeDamage(tickDamage);
//            yield return new WaitForSeconds(tickInterval);
//            elapsed += tickInterval;
//        }
//        target.RemoveDebuff(this);
//    }
//}
