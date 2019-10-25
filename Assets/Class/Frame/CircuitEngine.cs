using System;
using System.Collections.Generic;
using Assets.Class.Entity;
using LinearAlgebra;
using LinearAlgebra.LinearEquations;

namespace Assets.Class.Frame
{
    /// <summary>
    /// 用于单电源电路的物理引擎
    /// </summary>
    public class CircuitEngine
    {
        
        private static List<Branch> branchList = null;
        private List<Node> nodeList = null;
        private PowerSource source;

        public CircuitEngine(PowerSource powerSource)
        {
            source = powerSource;
        }

        /// <summary>
        /// 解析电路结构并计算各处电流与电压
        /// </summary>
        public void AnalyseCircuit()
        {
            BuildBranch();
            double[] X = SolveEquation();
            SetValue(X);
        }

        private void BuildBranch()
        {
            branchList = new List<Branch>();
            nodeList = new List<Node>();
            Node node;
            EletronicUnit unit = source;
            Branch branch;
            node = source.GetNegativePole();
            Stack<Node> nodes = new Stack<Node>();
            Stack<Branch> branches = new Stack<Branch>();
            branch = new Branch(node);
            node.index = nodeList.Count;
            bool IN = true;
            do
            {
                if (node.Count == 0)
                {
                    if (nodes.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        node = nodes.Pop();
                        branch = branches.Pop();
                        IN = false;
                        continue;
                    }
                }
                else if (node.IsForking())
                {
                    if (nodeList.Contains(node))
                    {
                        SaveBranch(node, branch);
                        if (nodes.Count == 0)
                        {
                            break;
                        }
                        else
                        {
                            node = nodes.Pop();
                            branch = branches.Pop();
                            IN = true;
                            continue;
                        }
                    }
                    else if (!IN)
                    {
                        SaveBranchAndNode(node, branch);
                        int count = node.Count;
                        for (int i = 0; i < count; i++)
                        {
                            nodes.Push(node.GetNode(i));
                            branches.Push(new Branch(node));
                        }
                        node = nodes.Pop();
                        branch = branches.Pop();
                        IN = true;
                        continue;
                    }
                    else if (IN)
                    {
                        SaveBranchAndNode(node, branch);
                        unit = node.GetUnit();
                        branch = new Branch(node);
                        node = unit.GetOutNode(node);
                        branch.Append(unit);
                    }
                }
                else if (!IN && !node.IsForking())
                {
                    node = node.GetNode(0);
                    if (nodeList.Contains(node))
                    {
                        SaveBranch(node, branch);
                        if (nodes.Count == 0)
                        {
                            break;
                        }
                        else
                        {
                            node = nodes.Pop();
                            branch = branches.Pop();
                            IN = true;
                            continue;
                        }
                    }
                }
                else
                {
                    
                    unit = node.GetUnit();
                    branch.Append(unit);
                    node = unit.GetOutNode(node);
                }
                IN = !IN;
            } while (true);
        }

        private void SetValue(double[] X)
        {
            if(X.Length == 1)
            {
                branchList[0].SetI(X[0]);
                return;
            }
            int nodeCount = nodeList.Count;
            for(int i = 0; i < nodeCount; i++)
            {
                branchList[i].SetI(X[i]);
            }
        } 


        /// <summary>
        /// 建立电学方程并求解
        /// </summary>
        /// <returns>线性方程组求解结果</returns>
        private double[] SolveEquation()
        {
            int branchCount = branchList.Count;
            //有一个零电势点是默认的，所以矩阵少一个维度。
            int nodeCount = nodeList.Count - 1;
            double EMP = source.GetEMF();

            //串联电路
            if (branchCount == 1)
            {
                double I = EMP / branchList[0].R;
                return new double[]{ I};
            }

            double[,] equations = new double[branchCount + nodeCount, branchCount + nodeCount];
            for (int i = 0; i < branchCount + nodeCount; i++)
            {
                for (int j = 0; j < branchCount + nodeCount; j++)
                {
                    equations[i, j] = 0;
                }
            }
            for (int i = 0; i < branchCount; i++)
            {
                if(branchList[i].PP.index != nodeCount)
                {
                    equations[branchList[i].PP.index, i] = 1;
                    equations[i + nodeCount, branchList[i].PP.index + branchCount] = 1;
                }
                if(branchList[i].NP.index != nodeCount)
                {
                    equations[branchList[i].NP.index, i] = -1;
                    equations[i + nodeCount, branchList[i].NP.index + branchCount] = -1;
                }
                equations[nodeCount + i, i] = -branchList[i].R;
                
            }
            
            Matrix A = new Matrix(equations);
            
            double[] B = new double[nodeCount + branchCount];
            for(int i = 0; i < nodeCount + branchCount; i++)
            {
                B[i] = 0;
            }
            B[nodeCount] = -EMP;

            GaussElimination gauss = new GaussElimination(A, B, false);
            return gauss.X;
        }


        private void SaveBranchAndNode(Node node,Branch branch)
        {
            node.index = nodeList.Count;
            nodeList.Add(node);
            SaveBranch(node, branch);
        }

        private void SaveBranch(Node node, Branch branch)
        {
            if(branch.PP == node && branch.Count == 0)
            {
                return;
            }
            branch.NP = node;
            branchList.Add(branch);
        }

        /*public static void Main(String[] args)
        {
            Resistance r = new Resistance(1.5);
            Resistance r2 = new Resistance(1.5);
            PowerSource E = new PowerSource(1.5, 0);
            r.LeftNode.ConnectTo(E.GetPositivePole());
            E.GetNegativePole().ConnectTo(r.RightNode);
            r2.LeftNode.ConnectTo(E.GetPositivePole());
            E.GetNegativePole().ConnectTo(r2.RightNode);
            CircuitEngine engine = new CircuitEngine(E);
            engine.AnalyseCircuit();
        }*/
    }
}
