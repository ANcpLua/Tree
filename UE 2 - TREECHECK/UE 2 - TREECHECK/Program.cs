

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

    public void TraverseAndCheckAVL(TreeNode node, ref bool isAVL)        //  Rekursive Berechnung Balance 
    {
        if (node == null)
        {
            return;
        }

        TraverseAndCheckAVL(node.Left, ref isAVL);
        int balance = BalanceFactor(node);
        Console.Write($"bal({node.Key}) = {balance}");
        if (balance > 1 || balance < -1)
        {
            Console.Write(" (AVL violation!)");
            isAVL = false;
        }
        Console.WriteLine();
        TraverseAndCheckAVL(node.Right, ref isAVL);
    }

    public void TraverseAndComputeStats(TreeNode node, ref int min, ref int max, ref int sum, ref int count)  // Die Funktion TraverseAndComputeStats traversiert den Baum ebenfalls in Inorder-Reihenfolge und berechnet dabei
                                                                                                              // die Min/Max/Avg-Werte. Dabei verwendet sie vier Referenzvariablen (min, max, sum und count), um die Zwischenergebnisse während der Traversierung zu speichern.
    {
        if (node == null)
        {
            return;
        }

        TraverseAndComputeStats(node.Left, ref min, ref max, ref sum, ref count);

        min = Math.Min(min, node.Key);
        max = Math.Max(max, node.Key);
        sum += node.Key;
        count++;

        TraverseAndComputeStats(node.Right, ref min, ref max, ref sum, ref count);
    }
    
    public TreeNode Search(TreeNode node, int key)
    {
        if (node == null || node.Key == key)
        {
            return node;
        }

        if (key < node.Key)
        {
            return Search(node.Left, key);
        }
        else
        {
            return Search(node.Right, key);
        }
    }

    
    public bool IsSubtree(TreeNode node, TreeNode subtree)
    {
        if (subtree == null)
        {
            return true;
        }
        if (node == null)
        {
            return false;
        }
        if (AreIdentical(node, subtree))
        {
            return true;
        }

        return IsSubtree(node.Left, subtree) || IsSubtree(node.Right, subtree);
    }
    
    public int FindMin()
    {
        return FindMin(Root);
    }

    private int FindMin(TreeNode node)
    {
        if (node == null)
        {
            throw new InvalidOperationException("Der Baum ist leer.");
        }
        return node.Left == null ? node.Key : FindMin(node.Left);
    }

    public int FindMax()
    {
        return FindMax(Root);
    }

    private int FindMax(TreeNode node)
    {
        if (node == null)
        {
            throw new InvalidOperationException("Der Baum ist leer.");
        }
        return node.Right == null ? node.Key : FindMax(node.Right);
    }

    public double FindAverage()
    {
        (int sum, int count) = FindSumAndCount(Root);
        return (double)sum / count;
    }

    private (int sum, int count) FindSumAndCount(TreeNode node)
    {
        if (node == null)
        {
            return (0, 0);
        }

        var left = FindSumAndCount(node.Left);
        var right = FindSumAndCount(node.Right);

        return (left.sum + node.Key + right.sum, left.count + 1 + right.count);
    }
    
    private bool AreIdentical(TreeNode node1, TreeNode node2)
    {
        if (node1 == null && node2 == null)
        {
            return true;
        }
        if (node1 == null || node2 == null)
        {
            return false;
        }

        return node1.Key == node2.Key &&
               AreIdentical(node1.Left, node2.Left) &&
               AreIdentical(node1.Right, node2.Right);
    }
    
    public void PrintTreeStats()
    {
        int min = int.MaxValue;
        int max = int.MinValue;
        int sum = 0;
        int count = 0;

        TraverseAndComputeStats(Root, ref min, ref max, ref sum, ref count);

        double average = count == 0 ? 0.0 : (double)sum / count;

        Console.WriteLine($"min: {min}, max: {max}, avg: {average:F1}");
    }
    
    public void PrintBalanceFactors()
    {
        bool isAvl = true;
        PrintBalanceFactorsHelper(Root, ref isAvl);
        Console.WriteLine($"AVL: {(isAvl ? "yes" : "no")}");
    }

    private void PrintBalanceFactorsHelper(TreeNode node, ref bool isAvl)
    {
        if (node == null)
        {
            return;
        }

        PrintBalanceFactorsHelper(node.Left, ref isAvl);
        int balance = BalanceFactor(node);
        Console.Write($"bal({node.Key}) = {balance}");
        if (balance > 1 || balance < -1)
        {
            Console.WriteLine(" (AVL violation!)");
            isAvl = false;
        }
        else
        {
            Console.WriteLine();
        }
        PrintBalanceFactorsHelper(node.Right, ref isAvl);
        

    }

    public void SearchAndPrintPath(int key)
    {
        var path = new List<int>();
        if (SearchAndPrintPathHelper(Root, key, path))
        {
            Console.WriteLine($"{key} found {string.Join(", ", path)}");
        }
        else
        {
            Console.WriteLine($"{key} not found!");
        }
    }

    private bool SearchAndPrintPathHelper(TreeNode node, int key, List<int> path)
    {
        if (node == null)
        {
            return false;
        }
        path.Add(node.Key);
        if (node.Key == key)
        {
            return true;
        }

        if (SearchAndPrintPathHelper(node.Left, key, path) || SearchAndPrintPathHelper(node.Right, key, path))
        {
            return true;
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }
    
    public bool IsSubtree(BinaryTree subtree)
    {
        return IsSubtreeHelper(Root, subtree.Root);
    }

    private bool IsSubtreeHelper(TreeNode node, TreeNode subtree)
    {
        if (subtree == null)
        {
            return true;
        }
        if (node == null)
        {
            return false;
        }
        if (AreIdentical(node, subtree))
        {
            return true;
        }
        return IsSubtreeHelper(node.Left, subtree) || IsSubtreeHelper(node.Right, subtree);
    }
    
}

class Program
{   
    static void Main(string[] args)
    {
        // Assuming you have 6 input files (3 pairs of search tree and subtree files)
        if (args.Length < 6 || args.Length % 2 != 0)
        {
            Console.WriteLine("Usage: <search_tree_filename_1> <subtree_filename_1> <search_tree_filename_2> <subtree_filename_2> <search_tree_filename_3> <subtree_filename_3>");
            return;
        }

        for (int i = 0; i < args.Length; i += 2)
        {
            string searchTreeFilename = args[i];
            string subtreeFilename = args[i + 1];

            Console.WriteLine($"Processing files: {searchTreeFilename} and {subtreeFilename}");

            var searchTree = new BinaryTree();

            using (StreamReader reader = new StreamReader(searchTreeFilename))       //   Einlesen und Aufbau des Baums 
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int key = int.Parse(line);
                    searchTree.Insert(key);
                }
            }

            var subtree = new BinaryTree();
            int subtreeNodeCount = 0;
            using (StreamReader reader = new StreamReader(subtreeFilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int key = int.Parse(line);
                    subtree.Insert(key);
                    subtreeNodeCount++;
                }
            }

            // Print the balance factors and whether the tree is an AVL tree
            searchTree.PrintBalanceFactors();

            // Print the tree statistics (min, max, avg)
            searchTree.PrintTreeStats();

            if (subtreeNodeCount == 1) // Rekursive Suche Schlüsselwert    -------- einfache suche
            {
                int keyToFind = subtree.Root.Key;
                searchTree.SearchAndPrintPath(keyToFind);
            }
            else // Rekursive Suche Teilbaum
            {
                bool isSubtree = searchTree.IsSubtree(subtree);
                Console.WriteLine(isSubtree ? "Subtree found" : "Subtree not found!");
            }

            Console.WriteLine("====================================");
        }
    }
}

