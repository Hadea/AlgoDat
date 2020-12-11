using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dat
{
    public class QuadTree<T>
    {
        QuadTreeNode<T> root;
        
        
        public void Add((int X, int Y) Coordinates, T Data)
        {
            

        }
    }

    public class QuadTreeNode<T>
    {
        List<T> Elements = new();
        QuadTreeNode<T>[] SubNodes = new QuadTreeNode<T>[4];
    }


}