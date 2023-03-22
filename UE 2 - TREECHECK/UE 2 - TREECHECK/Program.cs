using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class TreeNode
{
    public int Key;
    public TreeNode Left;
    public TreeNode Right;
}

class BinaryTree
{
    public TreeNode Root;

    public void Insert(int key)
    {
        Root = Insert(Root, key);
    }

    private TreeNode Insert(TreeNode node, int key)
    {
        if (node == null)
        {
            node = new TreeNode { Key = key };
        }
        else if (key < node.Key)
        {
            node.Left = Insert(node.Left, key);
        }
        else if (key > node.Key)
        {
            node.Right = Insert(node.Right, key);
        }

        return node;
    }

    public int Height(TreeNode node)
    {
        if (node == null)
        {
            return 0;
        }

        int leftHeight = Height(node.Left);
        int rightHeight = Height(node.Right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    public int BalanceFactor(TreeNode node)
    {
        if (node == null)
        {
            return 0;
        }

        return Height(node.Right) - Height(node.Left);
    }

    public void TraverseAndCheckAVL(TreeNode node)
{
    if (node == null)
    {
        return;
    }

    int balance = BalanceFactor(node);
    Console.Write($"bal({node.Key}) = {balance}");
    if (balance > 1 || balance < -1)
    {
        Console.Write(" (AVL violation!)");
    }
    Console.WriteLine();

    TraverseAndCheckAVL(node.Left);
    TraverseAndCheckAVL(node.Right);
}

    public bool IsAVL(TreeNode node)
    {
        if (node == null)
        {
            return true;
        }

        int balance = BalanceFactor(node);

        if (balance > 1 || balance < -1)
        {
            return false;
        }

        return IsAVL(node.Left) && IsAVL(node.Right);
    }

    public List<int> TraverseAndCollectKeys(TreeNode node)
    {
        var keys = new List<int>();

        if (node == null)
        {
            return keys;
        }

        keys.AddRange(TraverseAndCollectKeys(node.Left));
        keys.Add(node.Key);
        keys.AddRange(TraverseAndCollectKeys(node.Right));

        return keys;
    }

    public static void PrintStats(BinaryTree tree)
    {
        var keys = tree.TraverseAndCollectKeys(tree.Root);
        Console.WriteLine($"min: {keys.Min()}, max: {keys.Max()}, avg: {keys.Average():F1}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: treecheck filename");
            return;
        }

        string filename = args[0];

        Console.WriteLine($"Processing file: {filename}");

        var tree = new BinaryTree();

        using (StreamReader reader = new StreamReader(filename))
        {
            HashSet<int> addedKeys = new HashSet<int>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int key = int.Parse(line);
                if (!addedKeys.Contains(key))
                {
                    tree.Insert(key);
                    addedKeys.Add(key);
                }
            }
        } 
        bool isAvl = tree.IsAVL(tree.Root);
        Console.WriteLine("AVL: " + (isAvl ? "yes" : "no"));

        tree.TraverseAndCheckAVL(tree.Root);

        BinaryTree.PrintStats(tree);
    }
    
}
