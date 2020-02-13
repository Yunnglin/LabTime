using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Class.Frame
{
    /// <summary>
    /// 节点类：用于仪器的连接与电路结构的分析
    /// </summary>
    public class Node
    {
        private EletronicUnit unit;
        private List<Node> feet;
        public int index;
        
        public int Count
        {
            get
            {
                return feet.Count;
            }
        }

        public Node(EletronicUnit unit)
        {
            this.unit = unit;
            feet = new List<Node>();
        }

        /// <summary>
        /// 从该节点连接到目标节点
        /// </summary>
        /// <param name="target">目标节点</param>
        public void ConnectTo(Node target)
        {
            feet.Add(target);
            target.ConnectFrom(this);
        }

        /// <summary>
        /// 断开该节点的所有连接
        /// </summary>
        public void Disconnect()
        {
            foreach(Node node in feet)
            {
                node.DisconnectFrom(this);
            }
            feet.Clear();
        }

        /// <summary>
        /// 获取节点所处的元器件
        /// </summary>
        public EletronicUnit GetUnit()
        {
            return unit;
        }

        /// <summary>
        /// 是否是分支节点
        /// </summary>
        public bool IsForking()
        {
            if (feet.Count > 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取该节点所连接的第i个节点
        /// </summary>
        public Node GetNode(int index)
        {
            return feet[index];
        }

        private void ConnectFrom(Node source)
        {
            feet.Add(source);
        }

        private void DisconnectFrom(Node source)
        {
            feet.Remove(source);
        }

       
    }
}
