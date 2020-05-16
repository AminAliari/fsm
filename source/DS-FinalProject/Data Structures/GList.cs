using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_FinalProject {

    public class GList<T> {

        public long capacity;
        public long length;

        int i;
        ListNode<T> root, p, q;

        public GList() {
            root = new ListNode<T>();
            length = i = 0;
        }

        class ListNode<T> {

            public T data;
            public ListNode<T> link;

            public ListNode() { }
            public ListNode(T data) {
                this.data = data;
            }
        }    

        public void add(T data) {
            p = root;
            while(p != null) {
                q = p;
                p = p.link;
            }
            p = new ListNode<T>(data);
            q.link = p;
            length++;
        }

        public void addFirst(T data) {
            p = new ListNode<T>(data);
            p.link = root.link;
            root.link = p;
            length++;
        }

        public T this[int index] {
            get {
                if (index < 0 || index + 1 > capacity) throw new Exception("index out of range");
                i = -1;
                p = root;

                while (p!=null) {
                    if (i == index) {
                        return p.data;
                    }
                    i++;
                    p = p.link;
                }

                return default(T);
            }

            set {
                if (index < 0 || index + 1 > capacity) throw new Exception("index out of range");
                i = -1;
                p = root;

                while (p != null) {
                    if (i == index) {
                        break;
                    }
                    i++;
                    p = p.link;
                }
                p.data = value;
            }
        }
    }
}
