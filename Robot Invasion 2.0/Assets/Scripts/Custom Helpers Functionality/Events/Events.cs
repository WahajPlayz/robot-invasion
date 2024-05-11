using System;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour {
	public static Evt<Vector2> onMove = new Evt<Vector2>();
	public static Evt onFire = new Evt();
	public static Evt<Vector2> onMousePositionChange = new Evt<Vector2>();
	public static Evt onJump = new Evt();
}

public class Evt {
	private event Action _action = delegate { };

	public void Invoke() => _action?.Invoke();

	public void AddListener(Action listener) {
		_action += listener;
	}

	public void AddListeners(List<Action> listeners) {
		if (listeners.Count == 0) return;
		if (listeners.Count == 1)
		{
			AddListener(listeners[0]);
			return;
		};
		foreach (Action listerner in listeners)
		{
			_action += listerner;
		}
	}

	public void RemoveListener(Action listener) {
		_action -= listener;
	}

	public void RemoveListeners(List<Action> listeners) {
		if (listeners.Count == 0) return;
		if (listeners.Count == 1)
		{
			RemoveListener(listeners[0]);
			return;
		};
		foreach (Action listerner in listeners)
		{
			_action -= listerner;
		}
	}
}

public class Evt<T> {
	private event Action<T> _action = delegate { };
	private List<Action<T>> _actions = new List<Action<T>>();

	public void Invoke(T param) => _action?.Invoke(param);

	public void AddListener(Action<T> listener) => _action += listener;

	public void AddListeners(List<Action<T>> listListener) {
        foreach (var listener in listListener) {
			_actions.Add(listener);
		}
    }


	public void RemoveListener(Action<T> listener) => _action -= listener;
}
