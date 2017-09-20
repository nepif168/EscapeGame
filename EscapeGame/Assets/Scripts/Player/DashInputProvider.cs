using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class DashInputProvider : MonoBehaviour {

    public IObservable<IList<double>> wDoubleTapStream, aDoubleTapStream, sDoubleTapStream, dDoubleTapStream;

    double doubleTapInterval = 250d;

    private void Awake()
    {
        var wStream = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.W));
        var aStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.A));
        var sStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.S));
        var dStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.D));

        wDoubleTapStream = wStream.TimeInterval().Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval);
        aDoubleTapStream = aStream.TimeInterval().Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval);
        sDoubleTapStream = sStream.TimeInterval().Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval);
        dDoubleTapStream = dStream.TimeInterval().Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval);
    }

    private void Start()
    {
        Debug.Log(wDoubleTapStream);
    }

}
