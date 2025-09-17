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
        slider.interactable = stateMachine.GetCurrentAnimatorStateInfo(0).IsName("PlayTimeline") && !followPose.enabled;
        if (slider.interactable)
        {
            if (timeline.state == PlayState.Playing)
            {
                slider.value = (float)(timeline.time / timeline.duration);
            }

            if (timeline.state == PlayState.Paused)
            {
                timeline.time = slider.value * timeline.duration;
                timeline.Play();
                timeline.Pause();
            }
        }
    }
}
