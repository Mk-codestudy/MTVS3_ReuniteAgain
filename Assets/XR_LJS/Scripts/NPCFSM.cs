using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFSM : MonoBehaviour
{
    public enum NPCState
    {
        idle =0,
        walk =1,
        jump =2,
        run =4,
        trace=8, 
    }
}
