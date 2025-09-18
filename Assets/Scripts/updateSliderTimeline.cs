using Obi.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class updateSliderTimeline : MonoBehaviour
{
    public PlayableDirector timeline;
    public Animator stateMachine;
    public FollowPose followPose;

    private Slider slider; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = transform.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.interactable = false;
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("PlayTimeline"))
        {
            if (timeline.state == PlayState.Playing)
            {
                slider.value = (float)(timeline.time / timeline.duration);
            }

            if (timeline.state == PlayState.Paused && !followPose.enabled)
            {
                slider.interactable = true;
                timeline.time = slider.value * timeline.duration;
            }
        }
    }
}
