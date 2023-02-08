using Invector.vMelee;
using UnityEngine;
namespace Galo
{
    public enum Gender { MALE, FEMALE };
    public class CharacterData : MonoBehaviour
    {
        public Gender gender;
        public string myName;
        public string ability;
        public int runSpeed;
        public int jumpHeight;
        public int climbSpeed;
        public int multiJump;
        public int attackMax;
        public vMeleeAttackObject[] myAttackObjects;
        public PlayerType playerType;
        // other things to add
        // attachments
        // special items
        //etc


    }
}
