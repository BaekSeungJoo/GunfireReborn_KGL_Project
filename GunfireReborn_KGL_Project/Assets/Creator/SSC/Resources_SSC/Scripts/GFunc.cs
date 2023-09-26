using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GFunc 
{
    // Start is called before the first frame update
  

    public static GameObject FindRootObj(GameObject obj)
    {
        Transform targetObj = obj.transform;

        while(targetObj.parent != null)
        {
            if (targetObj.parent.GetComponent<EnemyHealth>() != null) 
            {
                return targetObj.gameObject;
            }

            targetObj = targetObj.parent;
        }
        
        return targetObj.gameObject;
    }

}
