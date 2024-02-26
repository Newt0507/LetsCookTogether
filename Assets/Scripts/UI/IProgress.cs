using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgress
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressTime;
        public float maxValue;
    }
}