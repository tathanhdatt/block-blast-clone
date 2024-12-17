using System;
using System.Linq;
using UnityEngine;

namespace Dt.Extension
{
    public static class AnimatorExtension
    {
        public static AnimationClip GetAnimationClip(this Animator animator, string clipName)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == clipName)
                {
                    return clip;
                }
            }

            return null;
        }

        public static string[] GetAnimations(this Animator animator)
        {
            try
            {
                AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
                return clips.Select(clip => clip.name).ToArray();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return Array.Empty<string>();
            }
        }
    }
}