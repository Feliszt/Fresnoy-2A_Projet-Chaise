using UnityEngine;
using UnityEngine.Playables;

public class ControlTimeline : MonoBehaviour
{
    public Animator stateMachine;
    private PlayableDirector timeline;
    public bool playTimeline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.stopped += OnTimelineFinished;
    }

    // Update is called once per frame
    void Update()
    {
        if (playTimeline)
        {
            PlayTimeline();
            playTimeline = false;
        }
    }

    public void PlayTimeline()
    {
        timeline.time = 0;
        timeline.Play();
    }

    public void ResumeTimeline()
    {
        timeline.Play();
    }

    public void StopTimeline()
    {
        timeline.Stop();
    }
    
    public void PauseTimeline()
    {
        timeline.Pause();
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        stateMachine.SetBool("TimelineFinished", true);
    }
}
