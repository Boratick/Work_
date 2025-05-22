using System;

class Node
{
    public int Key;
    public Node Left, Right;

    public Node(int item)
    {
        Key = item;
        Left = Right = null;
    }
}

class BinarySearchTree
{
    private Node root;

    public void Insert(int key)
    {
        root = InsertRec(root, key);
    }

    private Node InsertRec(Node root, int key)
    {
        if (root == null)
            return new Node(key);

        if (key < root.Key)
            root.Left = InsertRec(root.Left, key);
        else if (key > root.Key)
            root.Right = InsertRec(root.Right, key);

        return root;
    }

    public void PrintAll()
    {
        Console.WriteLine("Всі ключі дерева (in-order):");
        InOrder(root);
        Console.WriteLine();
    }

    private void InOrder(Node node)
    {
        if (node != null)
        {
            InOrder(node.Left);
            Console.Write(node.Key + " ");
            InOrder(node.Right);
        }
    }

    public bool Search(int key)
    {
        return SearchRec(root, key) != null;
    }

    private Node SearchRec(Node node, int key)
    {
        if (node == null || node.Key == key)
            return node;

        return key < node.Key
            ? SearchRec(node.Left, key)
            : SearchRec(node.Right, key);
    }

    public void PrintLeaves()
    {
        Console.WriteLine("Листки дерева:");
        PrintLeavesRec(root);
        Console.WriteLine();
    }

    private void PrintLeavesRec(Node node)
    {
        if (node != null)
        {
            if (node.Left == null && node.Right == null)
                Console.Write(node.Key + " ");
            PrintLeavesRec(node.Left);
            PrintLeavesRec(node.Right);
        }
    }

    public int? FindNext(int key)
    {
        Node current = root;
        Node successor = null;

        while (current != null)
        {
            if (key < current.Key)
            {
                successor = current;
                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }

        return successor?.Key;
    }

    
    public void PrintTree()
    {
        Console.WriteLine("Візуалізація дерева:");
        PrintTreeRec(root, 0);
        Console.WriteLine();
    }

    private void PrintTreeRec(Node node, int level)
    {
        if (node == null)
            return;

        
        PrintTreeRec(node.Right, level + 1);

        
        Console.WriteLine(new string(' ', level * 4) + node.Key);

        
        PrintTreeRec (node.Left, level + 1);
    }

    static void Main()
    {
        BinarySearchTree tree = new BinarySearchTree();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Додати ключ");
            Console.WriteLine("2. Вивести всі ключі");
            Console.WriteLine("3. Пошук ключа");
            Console.WriteLine("4. Вивести листки дерева");
            Console.WriteLine("5. Знайти наступний елемент");
            Console.WriteLine("6. Візуалізувати дерево"); 
            Console.WriteLine("0. Вийти");

            Console.Write("Виберіть опцію: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Введіть значення для додавання: ");
                    if (int.TryParse(Console.ReadLine(), out int valToAdd))
                    {
                        tree.Insert(valToAdd);
                        Console.WriteLine("Ключ додано.");
                    }
                    else
                    {
                        Console.WriteLine("Неправильне значення.");
                    }
                    break;

                case "2":
                    tree.PrintAll();
                    break;

                case "3":
                    Console.Write("Введіть значення для пошуку: ");
                    if (int.TryParse(Console.ReadLine(), out int valToFind))
                    {
                        Console.WriteLine(tree.Search(valToFind) ? "Ключ знайдено." : "Ключ не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine("Неправильне значення.");
                    }
                    break;

                case "4":
                    tree.PrintLeaves();
                    break;

                case "5":
                    Console.Write("Введіть значення, для якого знайти наступний: ");
                    if (int.TryParse(Console.ReadLine(), out int key))
                    {
                        int? next = tree.FindNext(key);
                        Console.WriteLine(next.HasValue
                            ? $"Наступний елемент після {key} — {next}"
                            : "Наступного елемента не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine("Неправильне значення.");
                    }
                    break;

                case "6":
                    tree.PrintTree();
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Завершення програми...");
                    break;

                default:
                    Console.WriteLine("Невірна опція. Спробуйте ще раз.");
                    break;
            }
        }
    }
}