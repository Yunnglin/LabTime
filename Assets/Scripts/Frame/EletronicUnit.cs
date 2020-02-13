using System;
using System.Collections;
using System.Collections.Generic;
using LinearAlgebra;
using LinearAlgebra.LinearEquations;
using Assets.Class.Entity;

namespace Assets.Class.Frame
{
    public abstract class EletronicUnit
    {
        public double I { get; protected set; }//电流
        public double R { get; protected set; }//电阻
        public double V { get; protected set; }//电压

        public Node LeftNode { get; protected set; }
        public Node RightNode { get; protected set; }
        

        public EletronicUnit()
        {
            LeftNode = new Node(this); ;
            RightNode = new Node(this); ;
        }

        public virtual Node GetOutNode(Node inNode)
        {
            if(LeftNode == inNode)
            {
                return RightNode;
            }
            return LeftNode;
        }

        internal void SetI(double i)
        {
            I = i;
            V = i * R;
        }
        

        
    }

    
}

