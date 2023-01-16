using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class InputsModel
    {
        public bool Jump { get; set; }
        public bool Hover { get; set; }
        public bool Dodge { get; set; }
        public bool AttackUp { get; set; }
        public bool AttackDown { get; set; }
        public bool AttackLeft { get; set; }
        public bool AttackRight { get; set; }
        public bool Heal { get; set; }
        public bool Fire { get; set; }
    }
}