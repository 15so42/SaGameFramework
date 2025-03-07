using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventClass //因为类名要和文件匹配所以不得不使用这个类,
{
   
}

public class UiEscEvent:EventClass
{
    
}

public class LoadingProgressEvent
{
    public float progress;

    public LoadingProgressEvent(float progress)
    {
        this.progress = progress;
    }
}
