namespace binaryTreeWithGraphviz
{
    class Pila
    {
        public int size;
        public char[] items;
        public int head;
        bool empty;
        bool full;
        public Pila(int size)
        {
            this.size = size;
            this.items = new char[size];
            this.head = 0;
            this.empty = true;
            this.full = false;
        }

        public bool push(char item)
        {
            if (head < size)
            {
                empty = false;
                items[head++] = item;
                full = true;
                return true;
            }
            return false;
        }
        public char pop()
        {
            if (head <= 0)
            {
                var a = '0';
                empty = true;
                full = false;
                return a;
            }
            return items[--head];

        }
        public char headValue()
        {
            if (head <= 0)
            {
                var a = '0';
                return a;
            }
            return items[head - 1];
        }

        public bool pilaIsFull()
        {
            return full;
        }
        public bool pilaIsEmpty()
        {
            return empty;
        }
    }
}
