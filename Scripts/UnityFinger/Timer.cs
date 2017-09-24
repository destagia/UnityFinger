using UnityEngine;

namespace UnityFinger
{
    class Timer : ITimer
    {
        float startTime;

        public float ElapsedTime {
            get { return Time.time - startTime; }
        }

        public void Start()
        {
            startTime = Time.time;
        }
    }
}
