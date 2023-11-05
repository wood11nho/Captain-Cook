using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IUsable
{
    public abstract void Use(GameObject player);
    UnityEvent OnUse { get; }
}
