using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Class.Frame;

namespace Assets.Class.Entity
{
    /// <summary>
    /// 电阻基实体类
    /// </summary>
    public class Resistance:EletronicUnit
    {
        public Resistance(double r)
        {
            R = r;
        }
    }
}
