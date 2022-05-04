using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _animator;
    public GameObject player;
    public bool _idle;
    public enum eAnimations{
        ENABLE,
        IDLE,
        WALK,
        BACK,
        JUMP,
        e,f,g,h,i,j,k,l}
    struct AnimationInfo{
        public int id;
        public string name;
        // constructor 
        public AnimationInfo(int _id, string _name) {
            id = _id;
            name = _name;
        }
    }
    private Dictionary <int, string > animDictionary = new Dictionary < int, string >();

    void Awake(){
        animSetup();
        _idle = true;
        _animator = GameObject.Find("/"+player.name+"ybot").GetComponent<Animator>();
        _animator.SetBool("Enable",true);
        Debug.Log(player.name);

    }

    private void animSetup(){
        animDictionary.Add((int)eAnimations.ENABLE,"Enable");
        animDictionary.Add((int)eAnimations.IDLE,"Idle");
        animDictionary.Add((int)eAnimations.WALK,"Walk");
        animDictionary.Add((int)eAnimations.BACK,"Back");
    }

    public void idle(){
        if(!_animator.GetBool("Idle")){
            Debug.LogWarning("Idleing");
            animationSwitch(eAnimations.IDLE);
        }
    }
    public void walk(){
        if(!_animator.GetBool("Walk")){
            Debug.LogWarning("Walking");
            animationSwitch(eAnimations.WALK);
        }
    }

    public void back(){
        if(!_animator.GetBool("Back")){
            Debug.LogWarning("Backwards");
            animationSwitch(eAnimations.BACK);
        }
    }

    private void animationSwitch(eAnimations state){
        int _state = (int) state;
        for (int i = 1; i < animDictionary.Count; i++)
        {
            if (i==_state)
            {
                _animator.SetBool(animDictionary[i],true);
            }
            _animator.SetBool(animDictionary[i],false);
        }
    }
    private void animationSwitch(List<eAnimations> state){

    }
}
