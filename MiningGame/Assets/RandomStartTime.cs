using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class RandomStartTime : MonoBehaviour
{
    /*
	<summary>
	Index of the layer of the default state
	</summary>
	*/
    [Tooltip("Index of the layer of the default state")]
    public int _layerIndex = 0;

    void Start()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(_layerIndex);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
