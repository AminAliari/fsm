using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_FinalProject {

    public class Edge {
        public int w;
        public List<string> labels;

        public Edge() {
            labels = new List<string>();
        }

        public Edge(int w) {
            this.w = w;
            labels = new List<string>();
        }

        public Edge(int w, List<string> labels) {
            this.w = w;
            this.labels = labels;
        }
    }

    public class Graph {

        public int nV;
        int nE;

        Hash<int, List<Edge>> adj;

        public Graph(int nV) {

            this.nV = nV;
            nE = 0;
            adj = new Hash<int, List<Edge>>();

            for (int v = 0; v < nV; v++) {
                adj.put(v, new List<Edge>());
            }
        }

        public void addEdge(int v, int w, List<string> labels) {
            if (v < 0 || v >= nV) throw new Exception("vertex " + v + " is not between 0 and " + (nV - 1));
            if (w < 0 || w >= nV) throw new Exception("vertex " + w + " is not between 0 and " + (nV - 1));
            adj.get(v).value.Add(new Edge(w, labels));
            nE++;
        }

        public List<Edge> Adj(int v) {
            if (v < 0 || v >= nV) throw new Exception();
            return adj.get(v).value;
        }


        public bool hasCycle() {
            List<int> whiteSet = new List<int>();
            List<int> graySet = new List<int>();
            List<int> blackSet = new List<int>();
            Hash<int, int> ex = new Hash<int, int>();

            for (int k=0;k<nV;k++) {
                whiteSet.Add(k);
            }

            int i = 0;
            while (i < whiteSet.Count) {
                int current = whiteSet[i];
                i++;
                if (dfs(current,-1, whiteSet, graySet, blackSet,ex)) {
                    return true;
                }
            }
            return false;
        }

        bool dfs(int current,int p, List<int> whiteSet, List<int> graySet, List<int> blackSet, Hash<int, int> ex) {
            ex.put(current, p);
            moveVertex(current, whiteSet, graySet);
            foreach (Edge e in Adj(current)) {

                if (blackSet.Contains(e.w)) {
                    continue;
                }

                if (graySet.Contains(e.w)) {
                    Console.Write("{" + current + "->" + e.w+", ");
                    done = false;
                    follow(e.w, current, ex);
                    return true;
                }
                if (dfs(e.w,current, whiteSet, graySet, blackSet,ex)) {
                    Console.Write("{" + current + "->" + e.w+", ");
                    done = false;
                    follow(e.w, current, ex);
                    return true;
                }
            }

            moveVertex(current, graySet, blackSet);
            return false;
        }
        public bool done = false;
        void follow(int t, int q, Hash<int, int> ex) {
            if (done) return;
            if (ex.get(q) == null) {  return; }
            if (ex.get(q).value == -1) { Console.WriteLine("}");done = true; return; }
            Console.Write(ex.get(q).value +"->"+ q + ", ");
            follow(t, ex.get(q).value, ex);
        }

        void moveVertex(int v, List<int> sourceSet, List<int> destinationSet) {
            sourceSet.Remove(v);
            destinationSet.Add(v);
        }

        public override string ToString() {
            List<string> c = new List<string>();
            List<Edge> edgs;
            StringBuilder sb = new StringBuilder();
            sb.Append("digraph{");
            for (int i = 0; i < nV; i++) {
                edgs = Adj(i);
                if (edgs.Count > 0) {
                    foreach (Edge t in edgs) {
                        sb.Append(string.Format("{0} -> {1} [label=\"", i, t.w));
                        foreach (string l in t.labels) {
                            sb.Append(string.Format("{0}, ", l));
                        }
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append("\"];");
                    }
                }else {
                    sb.Append(string.Format("{0};", i));
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
