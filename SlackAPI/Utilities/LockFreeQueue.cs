using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SlackAPI.Utilities
{
    public class LockFreeQueue<T> : ILockFree<T>
        where T : class
    {
        private int _count;
        private SingleLinkNode _head;
        private SingleLinkNode _tail;

        public LockFreeQueue()
        {
            _head = new SingleLinkNode();
            _tail = _head;
        }

        public T Latest => _head.Next == null ? default : _tail.Item;

        public T Next => _head.Next == null ? default : _head.Next.Item;

        public int Count => _count;

        private static bool CompareAndExchange(ref SingleLinkNode pLocation, SingleLinkNode pComparand,
            SingleLinkNode pNewValue)
        {
            return
                pComparand ==
                Interlocked.CompareExchange(ref pLocation, pNewValue, pComparand);
        }

        public void Unshift(T pItem)
        {
            SingleLinkNode oldHead = null;

            var newNode = new SingleLinkNode();
            newNode.Item = pItem;

            var newNodeWasAdded = false;
            while (!newNodeWasAdded)
            {
                oldHead = _head.Next;
                newNode.Next = oldHead;

                if (_head.Next == oldHead)
                {
                    var tmp = _head.Next;
                    newNodeWasAdded = CompareAndExchange(ref tmp, oldHead, newNode);
                    _head.Next = tmp;
                }
            }

            CompareAndExchange(ref _head, oldHead, newNode);
        }

        public override void Push(T pItem)
        {
            SingleLinkNode oldTail = null;
            SingleLinkNode oldTailNext;

            var newNode = new SingleLinkNode();
            newNode.Item = pItem;

            var newNodeWasAdded = false;
            while (!newNodeWasAdded)
            {
                oldTail = _tail;
                oldTailNext = oldTail.Next;

                if (_tail == oldTail)
                    if (oldTailNext == null)
                    {
                        var tmp = _tail.Next;
                        newNodeWasAdded = CompareAndExchange(ref tmp, null, newNode);
                        _tail.Next = tmp;
                    }
                    else
                        CompareAndExchange(ref _tail, oldTail, oldTailNext);
            }

            CompareAndExchange(ref _tail, oldTail, newNode);
            Interlocked.Increment(ref _count);
        }

        public override bool Pop(out T pItem)
        {
            pItem = default;
            SingleLinkNode oldHead = null;

            var haveAdvancedHead = false;
            while (!haveAdvancedHead)
            {
                oldHead = _head;
                var oldTail = _tail;
                var oldHeadNext = oldHead.Next;

                if (oldHead == _head)
                {
                    if (oldHead == oldTail)
                    {
                        if (oldHeadNext == null)
                            return false;
                        CompareAndExchange(ref _tail, oldTail, oldHeadNext);
                    }

                    else
                    {
                        pItem = oldHeadNext.Item;
                        haveAdvancedHead =
                            CompareAndExchange(ref _head, oldHead, oldHeadNext);
                    }
                }
            }

            Interlocked.Decrement(ref _count);
            return true;
        }

        public T Shift()
        {
            T result;
            Shift(out result);
            return result;
        }

        public bool Shift(out T pItem)
        {
            pItem = default;
            if (_head == null)
                return false;
            SingleLinkNode oldHead = null;

            var haveAdvancedHead = false;
            while (!haveAdvancedHead)
            {
                oldHead = _head;
                if (oldHead != null)
                {
                    var oldHeadNext = oldHead.Next;
                    if (CompareAndExchange(ref _head, oldHead, oldHeadNext))
                    {
                        pItem = oldHead.Item;
                        return true;
                    }
                }
            }

            return false;
        }

        public override T Pop()
        {
            T result;
            Pop(out result);
            return result;
        }

        public override string ToString()
        {
            return string.Format("Item count: {0}", _count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LockFreeEnumerator(this);
        }

        /// <summary>
        ///     Does *not* provide any kind of stateful guarantee.  Should only be used in cases where we know that the queue is
        ///     not volatile.
        /// </summary>
        internal class LockFreeEnumerator : IEnumerator<T>
        {
            private SingleLinkNode currentNode;
            private LockFreeQueue<T> parent;

            public LockFreeEnumerator(LockFreeQueue<T> list)
            {
                parent = list;
            }

            T IEnumerator<T>.Current => currentNode.Item;

            object IEnumerator.Current => currentNode.Item;

            public bool MoveNext()
            {
                if (currentNode == null)
                    currentNode = parent._head.Next;
                else
                    currentNode = currentNode.Next;
                return currentNode != null;
            }

            public void Dispose()
            {
                parent = null;
                currentNode = null;
            }

            public void Reset()
            {
                currentNode = parent._head.Next;
            }
        }
    }
}