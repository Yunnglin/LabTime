using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Class.Frame;

namespace Assets.Class.Entity
{
    /// <summary>
    /// 电源基实体类：用直接用作普通电源，也可以派生其他复杂电源
    /// </summary>
    public class PowerSource:EletronicUnit
    {
        protected double EMF = 0;//电动势
        
        public PowerSource(double emf,double r)
        {
            EMF = emf;
            R = r;
            V = -emf;
        }

        /// <summary>
        /// 获取电源电动势
        /// </summary>
        public double GetEMF()
        {
            return EMF;
        }

        /// <summary>
        /// 获取正极，默认为右节点
        /// </summary>
        /// <returns></returns>
        public Node GetPositivePole()
        {
            return RightNode;
        }

        /// <summary>
        /// 获取负极，默认为左节点
        /// </summary>
        /// <returns></returns>
        public Node GetNegativePole()
        {
            return LeftNode;
        }
    }
}
