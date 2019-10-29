//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2017 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the sprite's fill.
/// </summary>

[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Tween/Tween Slider Value")]
public class TweenSliderValue : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    [Range(0f, 1f)]
    public float to = 1f;

    bool mCached = false;
    UISlider mSlider;

    void Cache()
    {
        mCached = true;
        mSlider = GetComponent<UISlider>();
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get
        {
            if (!mCached) Cache();
            if (mSlider != null) return mSlider.value;
            return 0f;
        }
        set
        {
            if (!mCached) Cache();
            if (mSlider != null) mSlider.value = value;
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenSliderValue Begin(GameObject go, float duration, float value)
    {
        TweenSliderValue comp = UITweener.Begin<TweenSliderValue>(go, duration);
        comp.from = comp.value;
        comp.to = value;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
