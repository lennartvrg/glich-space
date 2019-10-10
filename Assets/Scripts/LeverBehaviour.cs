using UnityEngine;

public class LeverBehaviour : MonoBehaviour
{
    private Animator Animation;
    
    public PowerlineBehaviour Powerline;
    
    // Start is called before the first frame update
    public void Start()
    {
        Animation = GetComponent<Animator>();
    }

    public void PullLever()
    {
        Animation.Play("Lever_PullDown");
        Powerline.SetPower(true);
    }
}
