using UnityEngine;
public class TimePhoton : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 2f;
    private float counter;
   private void Update()
    {
        Destroyed();
    }

    private void Destroyed()
    {
        counter += Time.deltaTime;
        if(counter >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
