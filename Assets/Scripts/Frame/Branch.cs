using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Class.Frame
{
    class Branch
    {
        public Node PP = null;
        public Node NP = null;

        public double R = 0;
        public double I = 0;
        public double U = 0;

        public List<EletronicUnit> Units { get; private set; }

        public Branch(Node node)
        {
            PP = node;
            Units = new List<EletronicUnit>();
        }

        public double Count
        {
            get
            {
                return Units.Count;
            }
        }

        public void Append(EletronicUnit unit)
        {
            Units.Add(unit);
            R = R + unit.R;
        }

        public void Insert(int index, EletronicUnit unit)
        {
            Units.Insert(index, unit);
        }

        public void SetI(double I)
        {
            this.I = I;
            U = I * R;
            foreach(EletronicUnit unit in Units)
            {
                unit.SetI(I);
            }
        }

    }
}
