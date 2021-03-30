using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class StateMachine : MonoBehaviour
    {
        public Material deadCell;
        public Material aliveCell;

        public int posx = 0;
        public int posy = 0;

        private bool myState = false;

        public void Init(int x, int y)
        {
            posx = x;
            posy = y;
            myState = false;
        }

        public void Start()
        {
            UpdateObject();
        }

        public void SetState(bool state)
        {
            myState = state;
            UpdateObject();
        }

        public bool GetState()
        {
            return myState;
        }

        private void UpdateObject()
        {
            switch(myState)
            {
                case false:
                    this.gameObject.GetComponent<Renderer>().material = deadCell;
                    break;
                case true:
                    this.gameObject.GetComponent<Renderer>().material = aliveCell;
                    break;
            }
        }

    }
}
