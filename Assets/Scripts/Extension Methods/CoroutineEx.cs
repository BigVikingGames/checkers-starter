using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace BVG.ExtensionMethods {
	public static class CoroutineEx {

		private static IEnumerable<System.Object> Wait(float delay) {
			yield return new WaitForSeconds(delay);
		}

		private static IEnumerable Wait(Action action, float delay) {
			yield return new WaitForSeconds(delay);
			action();
		}

		public static Coroutine StartCoroutine(this MonoBehaviour instance, IEnumerable coroutine) {
			return instance.StartCoroutine(coroutine.GetEnumerator());
		}

		public static Coroutine StartCoroutine(this MonoBehaviour instance, IEnumerable<System.Object> coroutine, float delay) {
			return instance.StartCoroutine(Wait(delay).Concat(coroutine));
		}

		public static Coroutine StartCoroutine(this MonoBehaviour instance, Action action, float delay) {
			return instance.StartCoroutine(Wait(action, delay));
		}
	}
}
